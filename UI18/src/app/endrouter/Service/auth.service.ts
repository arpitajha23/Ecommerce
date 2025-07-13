import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginRequest } from '../Models/login-request.model';
import { Observable } from 'rxjs';
import { LoginResponse } from '../Models/login-response.model';
import { GoogleLoginRequest } from '../Models/google-login-request.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  //  private apiUrl = 'https://localhost:7173/api/Auth';
     private apiUrl = 'http://localhost:5052/api/Auth';


  constructor(private http: HttpClient) {}

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/Login`, credentials);
  }
  // googleLogin(): Observable<LoginResponse> {
  //   // return this.http.post<LoginResponse>(`${this.apiUrl}/GoogleLogin`, {});
  //   return this.http.get<LoginResponse>(`${this.apiUrl}/GoogleLogin`);
  // }
  googleLogin(): void {
    window.location.href = `${this.apiUrl}/GoogleLogin`;
  }

}
