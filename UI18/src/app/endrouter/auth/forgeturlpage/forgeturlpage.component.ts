import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { Router } from '@angular/router';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

// PrimeNG modules
import { InputTextModule } from 'primeng/inputtext';
import { FloatLabelModule } from 'primeng/floatlabel';
import { PasswordModule } from 'primeng/password';
import { ButtonModule } from 'primeng/button';


@Component({
  selector: 'app-forgeturlpage',
  imports: [CommonModule,
    FormsModule,
    ReactiveFormsModule,

    // PrimeNG modules
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

  constructor(private fb: FormBuilder, private router: Router) {}

  ngOnInit(): void {
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
      const { otp, newPassword } = this.resetForm.value;
      // Call API here to verify OTP and update password
      console.log('Changing password:', { otp, newPassword });

      // After success
      this.router.navigate(['']);
    }
  }

  onResendOtp(): void {
    // Call API to resend OTP
    console.log('Resending OTP...');
  }

  onGoToLogin(): void {
    this.router.navigate(['']);
  }
}