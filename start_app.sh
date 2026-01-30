#!/bin/bash

# ShotSync Full Environment Starter
# This script ensures Docker is running, starts the database, installs dependencies, and runs the application.

set -e # Exit on error

# 1. Check if Docker is running
if ! docker info > /dev/null 2>&1; then
  echo "Error: Docker is not running. Please start Docker Desktop and try again."
  exit 1
fi

# 2. Start Database via Docker Compose
echo "Starting Database (Docker)..."
docker-compose up -d

# 3. Wait for Database to be ready (optional but good)
echo "Waiting for database to initialize..."
sleep 5

# 4. Setup Backend
echo "Setting up Backend..."
cd PoolBrackets-backend-dotnet-main
dotnet restore
# Build once to ensure everything is fine
dotnet build
echo "Starting Backend..."
dotnet run &
BACKEND_PID=$!
cd ..

# 5. Setup Frontend
echo "Setting up Frontend..."
cd ShotSync-fontend-vuejs
# Check if node_modules exists, if not, install
if [ ! -d "node_modules" ]; then
  echo "Installing Frontend dependencies (this may take a minute)..."
  npm install
fi
echo "Starting Frontend..."
npm run dev &
FRONTEND_PID=$!
cd ..

# Cleanup function
cleanup() {
    echo ""
    echo "Stopping all services..."
    kill $BACKEND_PID $FRONTEND_PID 2>/dev/null
    # Optional: stop docker? User likely wants DB running in bg, but we can stop it if preferred.
    # docker-compose stop
    exit
}

trap cleanup SIGINT

echo "-------------------------------------------------------"
echo "ShotSync is running!"
echo "Backend: http://localhost:5147 (API)"
echo "Frontend: http://localhost:9001 (Web)"
echo "Database: localhost:3306"
echo "Press Ctrl+C to stop the application processes."
echo "-------------------------------------------------------"

wait $BACKEND_PID $FRONTEND_PID
