import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
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
      password: ['', Validators.required],
      phone: ['', [Validators.required, Validators.pattern(/^[0-9]*$/)]]
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

  allowPhoneInput(event: KeyboardEvent): boolean {
  const charCode = event.charCode;
  const input = event.target as HTMLInputElement;

  // Allow '+' only at the beginning
  if (charCode === 43 && input.selectionStart === 0 && !input.value.includes('+')) {
    return true;
  }

  // Allow digits (0–9)
  if (charCode >= 48 && charCode <= 57) {
    return true;
  }

  event.preventDefault();
  return false;
  }

  onBackToLogin(): void {
    this.router.navigate(['']);
  }
}
