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
import { AuthService } from '../../Service/auth.service';


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
      private authService: AuthService
    // private messageService: MessageService
  ) {}

  ngOnInit(): void {
    this.forgotPasswordForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  onSubmit(): void {
    debugger
    if (this.forgotPasswordForm.valid) {
      const email = this.forgotPasswordForm.value.email;

      this.authService.forgotPassword(email).subscribe({
        next: (res) => {
          console.log('Reset password link sent to:', email);
          const { otpId, token, userId, email: maskedEmail } = res.data;
          // Store only the necessary info
          sessionStorage.setItem('otpId', otpId);
          sessionStorage.setItem('resetToken', token);
          sessionStorage.setItem('userId', userId);
          sessionStorage.setItem('email', maskedEmail);

          this.forgotPasswordForm.reset();
          this.router.navigate(['']); 

          // this.messageService.add({
          //     severity: 'success',
          //     summary: 'Reset Link Sent',
          //     detail: 'Please check your email.',
          // });
        },
        error: (err: any) => {
          console.error('Error sending reset password link:', err);
          // this.messageService.add({
          //   severity: 'error',
          //   summary: 'Error',
          //   detail: 'Could not send reset link. Try again later.',
          // });
        }
      });
    } else {
      this.forgotPasswordForm.markAllAsTouched();
    }
  }


  onBackToLogin(): void {
    // Navigate back to login (implement routing or use router.navigate)
    console.log('Navigate to login page');
    this.router.navigate(['']);
  }
}

