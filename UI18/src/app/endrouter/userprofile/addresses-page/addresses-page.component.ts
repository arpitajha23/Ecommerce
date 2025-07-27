import { Component, OnInit } from '@angular/core';
import { NavComponent } from "../../Shared/nav/nav.component";
import { ProfileSidebarComponent } from "../../Shared/profile-sidebar/profile-sidebar.component";
import { ProfileService } from '../../Service/profile.service';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button'; // for p-button
import { CardModule } from 'primeng/card';

@Component({
  selector: 'app-addresses-page',
  imports: [NavComponent, ProfileSidebarComponent, CommonModule, ButtonModule, CardModule],
  templateUrl: './addresses-page.component.html',
  styleUrl: './addresses-page.component.scss'
})
export class AddressesPageComponent implements OnInit {
  userId = 12; // dynamically set
  addresses: any[] = [];
  pagedAddresses: any[] = [];
  currentPage = 1;
  itemsPerPage = 4;

  constructor(private userProfileService: ProfileService) {}

  ngOnInit(): void {
    this.loadAddresses();
  }

  loadAddresses(): void {
    this.userProfileService.getUserAddresses(this.userId).subscribe({
      next: (data: any[]) => {
        this.addresses = data;
        this.updatePagination();
      },
      error: (err: any) => console.error('Error loading addresses:', err)
    });
  }
  get pages(): number[] {
  return Array(Math.ceil(this.addresses.length / this.itemsPerPage)).fill(0).map((_,  i) => i + 1);
  }

  updatePagination(): void {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    this.pagedAddresses = this.addresses.slice(startIndex, startIndex + this.itemsPerPage);
  }

  changePage(page: number): void {
    this.currentPage = page;
    this.updatePagination();
  }
  editAddress(address: any): void {
    // Logic to edit the address
    console.log('Editing address:', address);
  }
}
