import { Component, OnInit } from '@angular/core';
import { NavComponent } from "../../Shared/nav/nav.component";
import { ProfileSidebarComponent } from "../../Shared/profile-sidebar/profile-sidebar.component";
import { CommonModule } from '@angular/common';
import { AvatarModule } from 'primeng/avatar';
import { ButtonModule } from 'primeng/button';
import { AuthService } from '../../Service/auth.service';
import { UserProfile } from '../../Models/user-profile.model';
import { ProfileService } from '../../Service/profile.service';

@Component({
  selector: 'app-profile',
  imports: [NavComponent, ProfileSidebarComponent,
     ButtonModule,
    AvatarModule,
    CommonModule,
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent implements OnInit {
  user: any;
  usertest: any;

  constructor(private userProfileService: ProfileService) {}

  ngOnInit(): void {
    this.loadUserProfile();
  }

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
