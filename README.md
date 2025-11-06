### ACA-MVP 
## Concept:
Webapp used to store flight data. Expose APIs to create and retrieve flights. Only Admins can add flights. Users can search and view flights.

## Project Diagrams
- [C4 Container Level Diagram](https://github.com/ThomasNoto/aca-mvp/blob/main/Project_Diagrams/C4_Container_Level_Diagram.PNG)
- [Class Diagram](https://github.com/ThomasNoto/aca-mvp/blob/main/Project_Diagrams/UML_Class_Diagram.png)
- [Use Case Diagram](https://github.com/ThomasNoto/aca-mvp/blob/main/Project_Diagrams/UML_Use_Case_Diagram.png)

## Tech Stack:
- **Frontend**: Angular, TypeScript, Tailwind CSS
- **Backend**: C#, .Net 8 (ASP.NET Core Web API)
- **Database**: PostgreSQL (running in Docker container)
- **Package Managers**: NuGet, npm
- **Automation:** Bash (for startup)

## Setup Instructions
1. Clone the repository
   ```bash
   git clone <your_repo_url>
   cd aca-mvp  
2. Build the Docker container
   ```bash
   docker compose build
3. Start the application stack in bash (PostgreSQL + backend + frontend):
      ```bash
   ./start.sh
4. (Optional) Restore sample data with the provided SQL seed:
   ```bash
   docker exec -i aca_postgres psql -U postgres aca_mvp < aca_mvp_seed.sql
5. View the ACA Flight Tracker frontend at http://localhost:4200
6. View the API documentation with Swagger at http://localhost:8080/swagger

## Important Notes on Design Choices
The `.env` file includes local development credentials only  
In the future, I would make a secrets manager to store credential in a secrets manager such as the one provided by AWS

All departure times submitted for creating a flight are converted to UTC to meet industry standard guidelines on time storage. In production, they would be converted to the user's local time on search