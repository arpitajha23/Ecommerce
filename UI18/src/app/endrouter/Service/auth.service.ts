import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginRequest } from '../Models/login-request.model';
import { Observable } from 'rxjs';
import { LoginResponse } from '../Models/login-response.model';
import { GoogleLoginRequest } from '../Models/google-login-request.model';
import { UserProfile } from '../Models/user-profile.model';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl2 = 'http://localhost:5052/api';
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

  forgotPassword(email: string): Observable<any> {
    debugger
    return this.http.post(`${this.apiUrl}/ForgotPassword`, { email })
  }

  resetPassword(data: { token: string, otp: number, newPassword: string, otpId: any }): Observable<any> {
    return this.http.post(`${this.apiUrl}/ResetPassword`, data);
  }

  validateResetToken(token: string): Observable<any> {
  return this.http.get(`${this.apiUrl}/validate-reset-token?token=${token}`);
  }

  resendOTP(userId : number): Observable<any>{
    debugger
    console.log('Resending OTP for userId:', userId); 
    return this.http.post(`${this.apiUrl2}/Otp/resend`, { userId });
  }

 
}
