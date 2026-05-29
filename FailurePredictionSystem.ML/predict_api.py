import os
import joblib
import pandas as pd

from dotenv import load_dotenv
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel, Field


class MetricPredictionRequest(BaseModel):
    cpuUsagePercent: float = Field(..., ge=0, le=100)
    ramUsagePercent: float = Field(..., ge=0, le=100)
    diskUsagePercent: float = Field(..., ge=0, le=100)
    freeDiskSpaceGb: float = Field(..., ge=0)
    temperatureCelsius: float
    errorCount: int = Field(..., ge=0)
    uptimeHours: float = Field(..., ge=0)


class PredictionProbabilities(BaseModel):
    normal: float
    warning: float
    critical: float


class MetricPredictionResponse(BaseModel):
    predictedStateCode: int
    predictedState: str
    probabilities: PredictionProbabilities


load_dotenv()

MODEL_PATH = os.getenv("MODEL_PATH", "models/failure_prediction_model.joblib")

FEATURE_COLUMNS = [
    "CpuUsagePercent",
    "RamUsagePercent",
    "DiskUsagePercent",
    "FreeDiskSpaceGb",
    "TemperatureCelsius",
    "ErrorCount"
]

STATE_NAMES = {
    1: "Normal",
    2: "Warning",
    3: "Critical"
}

app = FastAPI(
    title="Failure Prediction ML Service",
    description="ML-сервис для прогнозирования состояния оборудования",
    version="1.0.0"
)

model = None


@app.on_event("startup")
def load_model():
    global model

    if not os.path.exists(MODEL_PATH):
        raise FileNotFoundError(f"Файл модели не найден: {MODEL_PATH}")

    model = joblib.load(MODEL_PATH)
    print(f"ML-модель загружена: {MODEL_PATH}")


@app.get("/health")
def health():
    return {
        "status": "ok",
        "modelLoaded": model is not None
    }


@app.post("/predict", response_model=MetricPredictionResponse)
def predict(request: MetricPredictionRequest):
    if model is None:
        raise HTTPException(status_code=500, detail="ML-модель не загружена")

    input_data = pd.DataFrame([{
    "CpuUsagePercent": request.cpuUsagePercent,
    "RamUsagePercent": request.ramUsagePercent,
    "DiskUsagePercent": request.diskUsagePercent,
    "FreeDiskSpaceGb": request.freeDiskSpaceGb,
    "TemperatureCelsius": request.temperatureCelsius,
    "ErrorCount": request.errorCount
    }], columns=FEATURE_COLUMNS)

    predicted_state_code = int(model.predict(input_data)[0])
    predicted_state = STATE_NAMES.get(predicted_state_code, "Unknown")

    probabilities_raw = model.predict_proba(input_data)[0]

    class_probabilities = dict(zip(model.classes_, probabilities_raw))

    normal_probability = float(class_probabilities.get(1, 0.0))
    warning_probability = float(class_probabilities.get(2, 0.0))
    critical_probability = float(class_probabilities.get(3, 0.0))

    return MetricPredictionResponse(
        predictedStateCode=predicted_state_code,
        predictedState=predicted_state,
        probabilities=PredictionProbabilities(
            normal=round(normal_probability, 4),
            warning=round(warning_probability, 4),
            critical=round(critical_probability, 4)
        )
    )