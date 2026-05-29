import joblib
import pandas as pd

model = joblib.load("models/failure_prediction_model.joblib")

print("Classes:", model.classes_)

input_data = pd.DataFrame([{
    "CpuUsagePercent": 92.5,
    "RamUsagePercent": 89.1,
    "DiskUsagePercent": 95.3,
    "FreeDiskSpaceGb": 23.5,
    "TemperatureCelsius": 91.2,
    "ErrorCount": 14
}])

prediction = model.predict(input_data)[0]
probabilities = model.predict_proba(input_data)[0]

print("Prediction:", prediction)
print("Probabilities:", probabilities)