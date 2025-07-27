import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-link-expired',
  imports: [],
  templateUrl: './link-expired.component.html',
  styleUrl: './link-expired.component.scss'
})
export class LinkExpiredComponent {
  constructor(private router: Router) {}

  // Navigate to the forgot password page
  goToForgot() {
    this.router.navigate(['/endroute/forgetpassword']);
  }
}
