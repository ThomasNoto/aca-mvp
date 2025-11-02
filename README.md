### ACA-MVP 
## Concept:
Webapp used to store flight data. Expose APIs to create and retrieve flights. Only Admins can add flights. Users can book flights.

## Tech Stack:
- Front: Angular, TypeScript
- Back: C#, .Net Core
- Data: PostgreSQL db running in Docker container
- Package Manager: NuGet

## Setup Instructions
1. Clone the repository  
2. Run `docker compose up` to start the database  
3. Navigate to `/backend` and run `dotnet run` to start the API

## UML Diagrams
- [Class Diagram](https://github.com/ThomasNoto/aca-mvp/blob/main/Project_Diagrams/UML_Class_Diagram.png)
- [Use Case Diagram](https://github.com/ThomasNoto/aca-mvp/blob/main/Project_Diagrams/UML_Use_Case_Diagram.png)

## Environment Variables
The `.env` file includes local development credentials only  
In the future, I would make a secrets manager to store credential in a secrets manager such as the one provided by AWS
