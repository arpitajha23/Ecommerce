import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { ButtonModule } from 'primeng/button';
import { FloatLabelModule } from 'primeng/floatlabel';
import { InputGroupModule } from 'primeng/inputgroup';
import { AuthService } from '../../Service/auth.service';


@Component({
  selector: 'app-register',
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    InputTextModule,
    PasswordModule,
    ButtonModule,
    FloatLabelModule,
    InputGroupModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent  implements OnInit {
  registerForm!: FormGroup;

  constructor(private fb: FormBuilder, private router: Router, private authService: AuthService) {}

  ngOnInit(): void {
    this.registerForm = this.fb.group({
      fullname: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [
                  Validators.required,
                  Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$/)]],
      phone: ['', [Validators.required, Validators.maxLength(10), this.validPhoneNumberValidator()]]
    });
  }

  onRegister(): void {
    if (this.registerForm.valid) {
      this.authService.register(this.registerForm.value).subscribe({
        next: (res) => {
          // this.toastr.success('Registration successful!');
          this.router.navigate(['']); 
        },
        error: (err) => {
          console.error('Registration failed', err);
          // this.toastr.error('Registration failed');
        }
      });
    } else {
      this.registerForm.markAllAsTouched();
    }
  }

  // Allow only digits and no more than 10 characters
  allowPhoneInput(event: KeyboardEvent): boolean {
    const charCode = event.charCode;
    return charCode >= 48 && charCode <= 57; // digits 0-9 only
  }

// Custom validator for phone numbers
  validPhoneNumberValidator() {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;
      if (!value) return null;

      const phoneRegex = /^[6-9]\d{9}$/; // Starts with 6–9 and 10 digits
      const invalidPatterns = ['0000000000', '1234567890', '0123456789'];

      if (!phoneRegex.test(value) || invalidPatterns.includes(value)) {
        return { invalidPhone: true };
      }

      return null;
    };
  }
  onBackToLogin(): void {
    this.router.navigate(['']);
  }
}
