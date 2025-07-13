import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms'; // Angular forms

// PrimeNG modules
import { InputTextModule } from 'primeng/inputtext';
import { InputGroupModule } from 'primeng/inputgroup';
import { FloatLabelModule } from 'primeng/floatlabel';
import { PasswordModule } from 'primeng/password';
import { ButtonModule } from 'primeng/button';
import { DividerModule } from 'primeng/divider';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { Router } from '@angular/router';


@Component({
  selector: 'app-forgot-password',
  imports: [FormsModule, CommonModule, CommonModule,
    FormsModule,
    ReactiveFormsModule,

    // PrimeNG
    InputTextModule,
    InputGroupModule,
    FloatLabelModule,
    PasswordModule,
    ButtonModule,
    DividerModule,
    ToastModule,],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.scss'
})
export class ForgotPasswordComponent  implements OnInit {
  forgotPasswordForm!: FormGroup;

  constructor(private fb: FormBuilder, 
      private router: Router,
    // private messageService: MessageService
  ) {}

  ngOnInit(): void {
    this.forgotPasswordForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  onSubmit(): void {
    if (this.forgotPasswordForm.valid) {
      const email = this.forgotPasswordForm.value.email;
      // Call API to send reset password link
      // this.messageService.add({
      //   severity: 'success',
      //   summary: 'Reset Link Sent',
      //   detail: 'Please check your email.',
      // });
      console.log('Sending reset password link to:', email);
    }
  }

  onBackToLogin(): void {
    // Navigate back to login (implement routing or use router.navigate)
    console.log('Navigate to login page');
    this.router.navigate(['']);
  }
}

