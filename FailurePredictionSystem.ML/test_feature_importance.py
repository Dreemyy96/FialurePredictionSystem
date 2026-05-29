import joblib

model = joblib.load("models/failure_prediction_model.joblib")

feature_names = [
    "CpuUsagePercent",
    "RamUsagePercent",
    "DiskUsagePercent",
    "FreeDiskSpaceGb",
    "TemperatureCelsius",
    "ErrorCount"
]

print("Classes:", model.classes_)
print()

print("Feature importances:")
for name, importance in zip(feature_names, model.feature_importances_):
    print(f"{name}: {importance:.4f}")