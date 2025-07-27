import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

// PrimeNG modules
import { InputTextModule } from 'primeng/inputtext';
import { FloatLabelModule } from 'primeng/floatlabel';
import { PasswordModule } from 'primeng/password';
import { ButtonModule } from 'primeng/button';
import { AuthService } from '../../Service/auth.service';


@Component({
  selector: 'app-forgeturlpage',
  imports: [CommonModule,
    FormsModule,
    ReactiveFormsModule,
    InputTextModule,
    FloatLabelModule,
    PasswordModule,
    ButtonModule,
  ],
  templateUrl: './forgeturlpage.component.html',
  styleUrl: './forgeturlpage.component.scss'
})
export class ForgeturlpageComponent implements OnInit {
  resetForm!: FormGroup;
  token: string = '';
  userId: number =0;
  tokenValid: boolean = false;  
    otpId: number = 0;
  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService
    ) {}

ngOnInit(): void {
  this.route.queryParams.subscribe(params => {
    this.token = params['token'] || '';
    this.otpId = params['otpId'] ? Number(params['otpId']) : 0;
    this.userId = params['userId'] ? Number(params['userId']) : 0;
    
    console.log('otpId:', this.otpId);
    console.log('userId:', this.userId);
    console.log('token:', this.token);

    if (this.token) {
      this.authService.validateResetToken(this.token).subscribe({
        next: (res) => {
          if (res.statusCode === 200) {
            this.tokenValid = true;
          } else {
            this.router.navigate(['/link-expired']);
          }
        },
        error: () => {
          this.router.navigate(['/link-expired']);
        }
      });
    } else {
      this.router.navigate(['/link-expired']);
    }
  });

  this.resetForm = this.fb.group({
    otp: ['', Validators.required],
    newPassword: ['', Validators.required],
    confirmPassword: ['', Validators.required]
  }, { validators: this.passwordMatchValidator });
}

  passwordMatchValidator(group: AbstractControl): { [key: string]: boolean } | null {
      const password = group.get('newPassword')?.value;
      const confirm = group.get('confirmPassword')?.value;
      return password === confirm ? null : { passwordMismatch: true };
    }

 onSubmit(): void {
  if (this.resetForm.valid) {
    debugger
    const payload = {
    token: this.token,
    otp: this.resetForm.value.otp,
    newPassword: this.resetForm.value.newPassword,
    otpId: this.otpId,
    userId: this.userId
  };

    this.authService.resetPassword(payload).subscribe({
      next: () => {
        console.log('Password reset successful');
        this.router.navigate(['']);
      },
      error: (err: any) => {
        console.error('Reset password failed:', err);
      }
    });
  }}

  onResendOtp(): void {
    debugger
    console.log('Resending OTP...');
    this.authService.resendOTP(this.userId).subscribe({
      next: () => {
        console.log('OTP resent successfully');
      },
      error: (err: any) => {
        console.error('Error resending OTP:', err);
      }
    });
  }

  onGoToLogin(): void {
    this.router.navigate(['']);
  }
}