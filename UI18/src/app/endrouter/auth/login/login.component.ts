import { Component, OnInit } from '@angular/core';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { DividerModule } from 'primeng/divider';
import { ToastModule } from 'primeng/toast';
import { RippleModule } from 'primeng/ripple';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { FloatLabelModule } from 'primeng/floatlabel';
import { AuthService } from '../../Service/auth.service';
import { LoginRequest } from '../../Models/login-request.model';

@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule, RouterModule, InputTextModule, PasswordModule, ButtonModule, CardModule, DividerModule, ToastModule, RippleModule, InputGroupModule, InputGroupAddonModule, FloatLabelModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  errorMessage: string = '';

  constructor(private fb: FormBuilder, private router: Router, private authService: AuthService,
      private route: ActivatedRoute, // ✅ Add this

  ) {}

  ngOnInit(): void {
    
      this.loginForm = this.fb.group({
          email: ['', [Validators.required, Validators.email]],
          password: ['', [Validators.required, Validators.minLength(6)]]
      });

     this.route.queryParams.subscribe(params => {
      const token = params['token'];
      const email = params['email'];
      const name = params['name'];

      if (token) {
        localStorage.setItem('token', token);
        if (email) localStorage.setItem('email', email);
        if (name) localStorage.setItem('name', decodeURIComponent(name));

        console.log('Token:', token);
        console.log('Email:', email);
        console.log('Name:', name);

        // ✅ Clean URL after storing
        this.router.navigate([], {
          queryParams: {},
          replaceUrl: true
        });
      } else {
        console.log("⚠️ Token not found in query params.");
      }
    });
  }

  onLogin() {
    debugger
    if (this.loginForm.valid) {
      const { email, password } = this.loginForm.value;
      const request = new LoginRequest(email, password);

      this.authService.login(request).subscribe({
        next: (response) => {
          console.log('Login successful:', response);
          const token = response.token;
          if (response.StatusCode === 200) {
            localStorage.setItem('token', token);
            this.errorMessage = '';
            this.router.navigate(['/endroute/dashboard']);
            // this.router.navigate(['/endroute/forgetpassword']);
          } else {
            this.errorMessage = 'Something went wrong. No token returned.';
          }
        },
        error: (err) => {
          console.error('Login failed:', err);
          if (err.status === 401) {
            this.errorMessage = 'Invalid credentials. Please try again.';
          } else if (err.status === 500) {
            this.errorMessage = 'Server error. Please try later.';
          } else {
            this.errorMessage = 'Unexpected error occurred.';
          }
        }
      });

    } else {
      this.loginForm.markAllAsTouched();
      console.log('Login form is invalid', this.loginForm.errors);
    }
  }

  
  onForgotPassword(): void {
    console.log('Forgot Password clicked');
    this.router.navigate(['/endroute/forgetpassword']);

  }

  onContinueWithGoogle(): void {
    console.log('Continue with Google clicked');
    this.authService.googleLogin();
  }

  onSignUp(): void {
    console.log('Sign Up clicked');
    this.router.navigate(['/endroute/register']);
  }

}
