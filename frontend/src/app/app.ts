import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Observable } from 'rxjs';
import { AsyncPipe } from '@angular/common';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';


import { User } from '../models/user.model';
// import { UserRoles } from '../models/user-role.enum';
// import { Airport } from '../models/airport.model';
// import { Flight } from '../models/flight.model';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet, 
    HttpClientModule, 
    AsyncPipe, 
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  http = inject(HttpClient);

  // create flight logic
  isCreatingFlight = false;

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
        console.log(`Success!`, value);
      }
    });
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
    this.isDropdownOpen = false;
  }

  private getUsers(): Observable<User[]> {
    return this.http.get<User[]>('http://localhost:8080/api/Users');
  }
}