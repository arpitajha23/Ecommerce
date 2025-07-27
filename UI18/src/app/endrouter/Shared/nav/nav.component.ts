import { Component } from '@angular/core';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';  // For ngModel in dropdowns

// PrimeNG modules
import { DropdownModule } from 'primeng/dropdown';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { RouterModule } from '@angular/router'; // If you're using routerLink
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-nav',
  imports: [
    FormsModule,
    DropdownModule,
    ButtonModule,
    InputTextModule,
    RouterModule,
    CommonModule 
  ],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.scss'
})
export class NavComponent {
 languages = [{ label: 'English', value: 'en' }, { label: 'Hindi', value: 'hi' }];
  currencies = [{ label: 'USD', value: 'usd' }, { label: 'INR', value: 'inr' }];
  selectedLanguage = 'en';
  selectedCurrency = 'usd';
}
