import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserProfile } from '../Models/user-profile.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {

   private apiUrl2 = 'http://localhost:5052/api';
  private apiUrl = 'http://localhost:5052/api/Auth';


  constructor(private http: HttpClient) {}


  getUserProfileById(userId: number): Observable<UserProfile> {
    return this.http.get<UserProfile>(`${this.apiUrl2}/Profile/GetUserDetailsById/${userId}`);
  }

   getUserAddresses(userId: number) {
    return this.http.get<any[]>(`${this.apiUrl2}/Profile/user-addresses/${userId}`);
  }
}
