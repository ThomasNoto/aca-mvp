### ACA-MVP 
## Concept:
Webapp used to store flight data. Expose APIs to create and retrieve flights. Only Admins can add flights. Users can book flights.

## Tech Stack:
Front: Angular, TypeScript
Back: C#, .Net Core
Data: PostgreSQL db running in Docker container

## Class Diagram
make in lucid
consider encapsulation
# Airport
+ id: int
+ iata_code: string
+ name: string
+ city: string
+ state: string
+ timezone: Timezones

<<enumeration>> Timezones
- EST
- CST
- MST
- PST 

# Flight
+ id: int
+ flight_number: string
+ origin_airport_id: int
+ destination_airport_id: int
+ departure_time: DateTimeOffset
+ arrival_time: DateTimeOffset
+ aircraft_type: string
+ status: FlightStatus

<<enumeration>> FlightStatus
- OnTime
- Delayed
- Cancelled

# AppUser
+ id: int
+ email: string
+ role: UserRole

<<enumeration>> UserRole
- Admin
- User

Airport: 1 -- * Flight (as origin)
Airport: 1 -- * Flight (as destination)
