import { HttpClient, HttpClientModule, HttpParams } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Observable } from 'rxjs';
import { AsyncPipe, DatePipe } from '@angular/common';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';


import { User } from '../models/user.model';
import { Flight } from '../models/flight.model';
import { UserRoles } from '../models/user-role.enum';
// import { Airport } from '../models/airport.model';
// import { Flight } from '../models/flight.model';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet, 
    HttpClientModule, 
    AsyncPipe, 
    FormsModule,
    ReactiveFormsModule,
    DatePipe,
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  isAdminSession: boolean = false; // mimics behavior jwt token on frontend
  http = inject(HttpClient);
  // create flight logic
  isCreatingFlight = false;
  flightSuccess: boolean = false;
  flightMessage: string = '';


  toggleCreateFlight() {
    this.isCreatingFlight = !this.isCreatingFlight;
  }

  createFlightForm = new FormGroup({
    flight_Number: new FormControl<string>(''),
    origin_Airport_Id: new FormControl<number>(0),
    destination_Airport_Id: new FormControl<number>(0),
    departure_Time: new FormControl<string | null>(null)
  })

  onFlightSubmit() {
    const localTime = this.createFlightForm.value.departure_Time ?? ''; 

    // converts datettime-local to UTC to fit industry standard time storage conventions
    let utcTime: string | null = null;
    if (localTime) {
      const parsedDate = new Date(localTime);
      utcTime = parsedDate.toISOString();
    }

    const addFlight = {
      flight_Number: this.createFlightForm.value.flight_Number,
      origin_Airport_Id: this.createFlightForm.value.origin_Airport_Id,
      destination_Airport_Id: this.createFlightForm.value.destination_Airport_Id,
      departure_Time: utcTime
    }

    console.log('Attempting to create flight');

    this.http.post('http://localhost:8080/api/Flights', addFlight)
    .subscribe({
      next: (value) => {
        this.flightSuccess = true;
        this.flightMessage = 'Flight created successfully!';
        this.createFlightForm.reset();
        this.isCreatingFlight = false;

        // Hide the message after a few seconds
        setTimeout(() => {
          this.flightSuccess = false;
          this.flightMessage = '';
        }, 4000);
      }
    });
  }

  // search flight logic
  flights: Flight[] = [];
  isSearching = false;
  isClearing = false;
  searchOrigin: string = '';
  searchDestination: string = '';


  searchFlightForm = new FormGroup({
    origin_Airport_Iata: new FormControl<string>(''),
    destination_Airport_Iata: new FormControl<string>('')
  })

  onFlightSearch() {
    if (this.isClearing) {
      this.searchFlightForm.reset();
      this.flights = [];
      this.currentPage = 1;
      this.isClearing = false;
      this.isSearching = false;
      return;
    }

    this.isSearching = true;
    const origin = this.searchFlightForm.value.origin_Airport_Iata ?? '';
    const destination =  this.searchFlightForm.value.destination_Airport_Iata  ?? '';
    this.searchOrigin = origin || 'Anywhere';
    this.searchDestination = destination || 'Anywhere';

    console.log(`Searching for ${origin} to ${destination}`)

    // subscribing to the observable once delivered
  this.getFlights(origin, destination).subscribe({
    next: (flights) => {
      // Sort flights by departure time ascending
      this.flights = flights.sort(
        (a, b) =>
          new Date(a.departure_Time).getTime() - new Date(b.departure_Time).getTime()
      );

      console.log('Flights found:', this.flights);

      // Reset form and toggle button to "Clear"
      this.searchFlightForm.reset();
      this.isClearing = true;
      this.isSearching = false;
      this.currentPage = 1;
      }
    });
  }

  private getFlights(origin: string, destination: string): Observable<Flight[]> {
    const url = `http://localhost:8080/api/Flights/search?origin=${origin}&destination=${destination}`;

    console.log('Calling backend with:', origin, destination);
    console.log('url: ', url)

    return this.http.get<Flight[]>(url);
  }

  pageSize = 4;
  currentPage = 1;

  get totalPages(): number {
    return Math.ceil(this.flights.length / this.pageSize);
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
    }
  }

  previousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
    }
  }

  // user logic
  users$ = this.getUsers();

  isDropdownOpen = false;
  selectedUser: User | null = null;

  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  selectUser(user: User) {
    this.selectedUser = user;
    this.isAdminSession = user.role === 0;
    this.isDropdownOpen = false;

    // put user and token in local storage to persist across resets
    localStorage.setItem('isAdminSession', String(this.isAdminSession));
    localStorage.setItem('selectedUser', JSON.stringify(user));
  }

  private getUsers(): Observable<User[]> {
    return this.http.get<User[]>('http://localhost:8080/api/Users');
  }
}