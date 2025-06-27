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
import { RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { FloatLabelModule } from 'primeng/floatlabel';

@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule, RouterModule, InputTextModule, PasswordModule, ButtonModule, CardModule, DividerModule, ToastModule, RippleModule, InputGroupModule, InputGroupAddonModule, FloatLabelModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
      this.loginForm = this.fb.group({
          email: ['', [Validators.required, Validators.email]],
          password: ['', [Validators.required, Validators.minLength(6)]]
      });
  }

  onLogin(){
    if(this.loginForm.valid){
      console.log('Login successful', this.loginForm.value);
      this.loginForm.reset();
      
    }
    else{
      this.loginForm.markAllAsTouched();
      console.log('Login failed', this.loginForm.errors);
    }
  }
  onForgotPassword(): void {
    console.log('Forgot Password clicked');
    // Add your forgot password logic here
  }

  onContinueWithGoogle(): void {
    console.log('Continue with Apple clicked');
    // Add your Apple login logic here
  }

  onSignUp(): void {
    console.log('Sign Up clicked');
    // Add your sign-up logic here
  }

}
