# exit on error
set -e

echo "Starting ACA Flight Tracker MVP..."

# start postgres docker container
echo "Starting PostgreSQL Docker container..."
docker compose up -d

# start backend
echo "Starting backend..."
cd backend

dotnet run &
BACKEND_PID=$!

cd ..

# start frontend
echo "Starting frontend..."
cd frontend

ng serve --open &
FRONTEND_PID=$!

cd ..

# shutdown
trap "echo '=Stopping...'; kill $BACKEND_PID $FRONTEND_PID 2>/dev/null; docker compose stop" SIGINT
wait
