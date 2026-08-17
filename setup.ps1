if (-not (Test-Path ".env")) {
    Write-Host "Creating .env file..."
    "MSSQL_SA_PASSWORD=OrderDev!2026" | Out-File -FilePath ".env" -Encoding utf8
}
else {
    Write-Host ".env already exists. Keeping existing configuration."
}

Write-Host "Starting application..."
docker compose up --build