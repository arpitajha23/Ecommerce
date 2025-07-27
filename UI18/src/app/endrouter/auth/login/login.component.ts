import { AfterViewInit, Component, NgZone, OnInit } from '@angular/core';
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
import { environment } from '../../../../environments/environment';

// declare var grecaptcha: any;

@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule, RouterModule, InputTextModule, PasswordModule, ButtonModule, CardModule, DividerModule, ToastModule, RippleModule, InputGroupModule, InputGroupAddonModule, FloatLabelModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  errorMessage: string = '';
  // siteKey = environment.recaptcha.siteKey;
  captchaToken: string | null = null;
  captchaError: boolean = false;
  captchaResolved = false;


  constructor(
    private fb: FormBuilder, 
    private router: Router, 
    private authService: AuthService,
    private route: ActivatedRoute,
    // private ngZone: NgZone
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

// ngAfterViewInit(): void {
//   this.waitForGrecaptchaToLoad().then(() => {
//     if (grecaptcha) {
//       grecaptcha.render('recaptcha-container', {
//         sitekey: this.siteKey,
//         callback: (token: string) => this.handleCaptcha(token)
//       });
//     }
//   });
// }

// private waitForGrecaptchaToLoad(): Promise<void> {
//   return new Promise((resolve) => {
//     const interval = setInterval(() => {
//       if (typeof grecaptcha !== 'undefined' && grecaptcha.render) {
//         clearInterval(interval);
//         resolve();
//       }
//     }, 200); // check every 200ms
//   });
// }

// handleCaptcha(token: string): void {
//   console.log('reCAPTCHA token:', token);
//   // Store the token or use it as needed for form submission
//   this.recaptchaToken = token;
// }

 
  // onCaptchaResolved(token: any): void {
  //   if (token) {
  //     this.ngZone.run(() => {
  //       this.captchaResolved = true;
  //     });
  //   }
  // }

  // handleCaptchaResponse(token: string) {
  //   this.captchaToken = token;
  //   this.captchaError = false; // Reset error state on successful captcha response
  // }

 onLogin(): void {
// if (!this.captchaToken) {
//       this.captchaError = true;
//       return;
//     }

    // const payload = {
    //   email: this.loginForm.value.email,
    //   password: this.loginForm.value.password,
    //   captcha: this.captchaToken // this can be ignored in backend as per your choice
    // };


     if (this.loginForm.valid ) {
      const { email, password } = this.loginForm.value;
      // const request = new LoginRequest(email, password, recaptchaToken);
      const request = new LoginRequest(email, password);

      this.authService.login(request).subscribe({
        next: (response) => {
          const token = response.token;

          if (response.statusCode === 200 ) {
            localStorage.setItem('token', token);
            this.errorMessage = '';
            // grecaptcha.reset(); // reset the captcha after successful login
            this.router.navigate(['/endroute/dashboard']);
          } else {
            this.errorMessage = 'Something went wrong. No token returned.';
            // grecaptcha.reset(); // reset captcha on failure too
          }
        },
        error: (err) => {
         // grecaptcha.reset(); // reset captcha always on error
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
    }
  }

  
  onForgotPassword(): void {
    this.router.navigate(['/endroute/forgetpassword']);
  }

  onContinueWithGoogle(): void {
    this.authService.googleLogin();
  }

  onSignUp(): void {
    this.router.navigate(['/endroute/register']);
  }
  onTermsOfUse(): void {
    this.router.navigate(['/endroute/terms']);
  }
  onPrivacyPolicy(): void {
    this.router.navigate(['/endroute/privacy']);  
  }

}
