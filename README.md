# ShotSync Project Setup guide

This repository contains the full ShotSync application (Frontend & Backend) with a portable environment setup.

## Prerequisites

Before running the application on a new machine, ensure you have the following installed:

1.  **Docker Desktop**: Required for the database.
2.  **.NET 8.0 SDK**: Required for the backend.
3.  **Node.js (v18+)**: Required for the frontend.
4.  **Quasar CLI**: `npm install -g @quasar/cli`

## Quick Start

To start the entire environment (Database + Backend + Frontend):

1.  Open your terminal.
2.  Navigate to the project root directory.
3.  Run the setup script:
    ```bash
    sh start_app.sh
    ```

The script will automatically:

- Start the MySQL 8.0 database in Docker.
- Initialize the database with existing data (from `db/init/init.sql`).
- Restore .NET dependencies and build the backend.
- Install NPM dependencies for the frontend (if missing).
- Start both services simultaneously.

## Accessing the Application

- **Frontend**: [http://localhost:9001](http://localhost:9001)
- **Backend API**: [http://localhost:5147](http://localhost:5147)
- **Database**: `localhost:3306` (User: `root`, Password: `rootpassword`)

## Troubleshooting

- **Docker not running**: Ensure Docker Desktop is open and active.
- **Port conflicts**: If port 3306, 5147, or 9001 is already in use, you may need to stop those services before running the script.
- **First run**: The first run will take longer as it needs to download the MySQL image and install NPM packages.

---

Developed by Antigravity
