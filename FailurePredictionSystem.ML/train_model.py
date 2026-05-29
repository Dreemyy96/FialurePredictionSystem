import os
import joblib
import pandas as pd

from dotenv import load_dotenv
from sqlalchemy import create_engine

from sklearn.ensemble import GradientBoostingClassifier
from sklearn.model_selection import train_test_split
from sklearn.metrics import accuracy_score, classification_report, confusion_matrix


def load_settings():
    load_dotenv()

    db_host = os.getenv("DB_HOST")
    db_port = os.getenv("DB_PORT")
    db_name = os.getenv("DB_NAME")
    db_user = os.getenv("DB_USER")
    db_password = os.getenv("DB_PASSWORD")
    model_path = os.getenv("MODEL_PATH", "models/failure_prediction_model.joblib")

    if not all([db_host, db_port, db_name, db_user, db_password]):
        raise ValueError("Не все параметры подключения к БД указаны в .env")

    return db_host, db_port, db_name, db_user, db_password, model_path


def load_data_from_db():
    db_host, db_port, db_name, db_user, db_password, _ = load_settings()

    connection_string = (
        f"postgresql+psycopg2://{db_user}:{db_password}"
        f"@{db_host}:{db_port}/{db_name}"
    )

    engine = create_engine(connection_string)

    query = """
    SELECT
        "CpuUsagePercent",
        "RamUsagePercent",
        "DiskUsagePercent",
        "FreeDiskSpaceGb",
        "TemperatureCelsius",
        "ErrorCount",
        "State"
    FROM "Metrics"
    WHERE "State" IN (1, 2, 3);
    """

    data = pd.read_sql(query, engine)

    return data


def prepare_dataset(data: pd.DataFrame):
    feature_columns = [
    "CpuUsagePercent",
    "RamUsagePercent",
    "DiskUsagePercent",
    "FreeDiskSpaceGb",
    "TemperatureCelsius",
    "ErrorCount"
    ]

    target_column = "State"

    x = data[feature_columns]
    y = data[target_column]

    return x, y


def train_model(x_train, y_train):
    model = GradientBoostingClassifier(
        n_estimators=150,
        learning_rate=0.05,
        max_depth=3,
        random_state=42
    )

    model.fit(x_train, y_train)

    return model


def evaluate_model(model, x_test, y_test):
    y_pred = model.predict(x_test)

    accuracy = accuracy_score(y_test, y_pred)

    print()
    print("Результаты оценки модели")
    print(f"Accuracy: {accuracy:.4f}")

    print()
    print("Classification report:")
    print(classification_report(
        y_test,
        y_pred,
        target_names=["Normal", "Warning", "Critical"]
    ))

    print()
    print("Confusion matrix:")
    print(confusion_matrix(y_test, y_pred))


def save_model(model, model_path: str):
    os.makedirs(os.path.dirname(model_path), exist_ok=True)
    joblib.dump(model, model_path)
    print()
    print(f"Модель сохранена: {model_path}")


def main():
    _, _, _, _, _, model_path = load_settings()

    print("Загрузка данных из PostgreSQL...")
    data = load_data_from_db()

    print(f"Загружено записей: {len(data)}")

    if len(data) == 0:
        raise ValueError("В таблице Metrics нет данных для обучения.")

    print()
    print("Распределение классов:")
    print(data["State"].value_counts().sort_index())

    x, y = prepare_dataset(data)

    x_train, x_test, y_train, y_test = train_test_split(
        x,
        y,
        test_size=0.2,
        random_state=42,
        stratify=y
    )

    print()
    print(f"Train records: {len(x_train)}")
    print(f"Test records: {len(x_test)}")

    print()
    print("Обучение модели GradientBoostingClassifier...")
    model = train_model(x_train, y_train)

    evaluate_model(model, x_test, y_test)

    save_model(model, model_path)


if __name__ == "__main__":
    main()