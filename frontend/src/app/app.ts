import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Observable } from 'rxjs';
import { AsyncPipe } from '@angular/common';


import { User } from '../models/user.model';
// import { UserRoles } from '../models/user-role.enum';
// import { Airport } from '../models/airport.model';
// import { Flight } from '../models/flight.model';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, HttpClientModule, AsyncPipe],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  http = inject(HttpClient);

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