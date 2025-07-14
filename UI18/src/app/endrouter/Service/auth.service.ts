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

 register(userData: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/Registration`, userData);
  }

  login(credentials: LoginRequest): Observable<LoginResponse> {
    const url = `${this.apiUrl}/Login`;
    console.log('Hitting API:', url); // Debug log
    
    return this.http.post<LoginResponse>(url, credentials);
  }
  googleLogin(): void {
    window.location.href = `${this.apiUrl}/GoogleLogin`;
  }

}
