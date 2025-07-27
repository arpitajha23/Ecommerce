import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { AvatarModule } from 'primeng/avatar';
import { ButtonModule } from 'primeng/button';
import { PanelMenuModule } from 'primeng/panelmenu';
import { SidebarModule } from 'primeng/sidebar';
import { ProfileService } from '../../Service/profile.service';

@Component({
  selector: 'app-profile-sidebar',
  imports: [
    CommonModule,
    SidebarModule,
    ButtonModule,
    AvatarModule,
    PanelMenuModule
  ],
  templateUrl: './profile-sidebar.component.html',
  styleUrl: './profile-sidebar.component.scss'
})
export class ProfileSidebarComponent implements OnInit {
  items: MenuItem[] = [];

  constructor( private route: Router, private userProfileService: ProfileService) {}

  ngOnInit() {
    this.loadUserProfile();
    this.items = [
      {
        label: 'Order History',
        icon: 'pi pi-history',
        routerLink: ['/order-history']
      },{
        label: 'Profile',
        icon: 'pi pi-user',
        routerLink: ['/endroute/profile']
      },
      {
        label: 'Addresses',
        icon: 'pi pi-map-marker',
        routerLink: ['/endroute/profile/address']
      },
      {
        label: 'Saved Cards',
        icon: 'pi pi-credit-card',
        routerLink: ['/endroute/profile/saved-cards']
      }
    ];
  }

 displaySidebar: boolean = false;

  showSidebar() {
    this.displaySidebar = true;
  }

    user: any;
  usertest: any;

loadUserProfile(): void {
    const userId = 12; // Replace with dynamic logic when needed
    this.userProfileService.getUserProfileById(userId).subscribe({
      next: (data: any) => {
        this.user = data;
        // Set avatar URL here
        this.usertest = {
          avatarUrl: 'https://i.pravatar.cc/100?img=3'  // Replace with actual avatar if needed
        };
      },
      error: (err: any) => {
        console.error('Error fetching profile:', err);
      }
    });
  }
}

