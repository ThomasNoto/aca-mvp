### ACA-MVP 
## Concept:
Webapp used to store flight data. Expose APIs to create and retrieve flights. Only Admins can add flights. Users can search and view flights.

## Tech Stack:
- Frontend: Angular, TypeScript
- Backend: C#, .Net Core
- Data: PostgreSQL (running in Docker container)
- Package Manager: NuGet

## Setup Instructions
1. Clone the repository  
2. Run `docker compose up` to start the database 
3. (Optional) Restore sample data:
   ```bash
   docker exec -i aca_postgres psql -U postgres aca_mvp < aca_mvp_seed.sql
4. Navigate to `/backend` and run `dotnet run` to start the API
5. view api documentation at http://localhost:8080/swagger

## Projet Diagrams
- [C4 Container Level Diagram](https://github.com/ThomasNoto/aca-mvp/blob/main/Project_Diagrams/C4_Container_Level_Diagram.PNG)
- [Class Diagram](https://github.com/ThomasNoto/aca-mvp/blob/main/Project_Diagrams/UML_Class_Diagram.png)
- [Use Case Diagram](https://github.com/ThomasNoto/aca-mvp/blob/main/Project_Diagrams/UML_Use_Case_Diagram.png)

## Important Notes on Design Choices
The `.env` file includes local development credentials only  
In the future, I would make a secrets manager to store credential in a secrets manager such as the one provided by AWS
