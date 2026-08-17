#!/bin/bash

set -e

if [ ! -f .env ]; then
    echo "Creating .env file..."
    echo "MSSQL_SA_PASSWORD=OrderDev!2026" > .env
else
    echo ".env already exists. Keeping existing configuration."
fi

echo "Starting application..."
docker compose up --build