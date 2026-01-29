#!/bin/bash

# Function to kill all child processes on exit
cleanup() {
    echo "Stopping all services..."
    kill $(jobs -p) 2>/dev/null
    exit
}

# Trap SIGINT (Ctrl+C)
trap cleanup SIGINT

# Start Backend
echo "Starting Backend..."
cd PoolBrackets-backend-dotnet-main
dotnet run &
BACKEND_PID=$!
cd ..

# Start Frontend
echo "Starting Frontend..."
cd ShotSync-fontend-vuejs
npm run dev &
FRONTEND_PID=$!
cd ..

# Wait for both processes
echo "Services started. Press Ctrl+C to stop."
wait $BACKEND_PID $FRONTEND_PID
