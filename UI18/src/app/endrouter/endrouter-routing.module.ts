import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './auth/register/register.component';
import { FirstproductpageComponent } from './Dashboard/firstproductpage/firstproductpage.component';
import { ForgotPasswordComponent } from './auth/forgot-password/forgot-password.component';
import { ForgeturlpageComponent } from './auth/forgeturlpage/forgeturlpage.component';
import { ProfileComponent } from './userprofile/profile/profile.component';
import { ProfileEditComponent } from './userprofile/profile-edit/profile-edit.component';
import { AddressesPageComponent } from './userprofile/addresses-page/addresses-page.component';
import { AddressesEditComponent } from './userprofile/addresses-edit/addresses-edit.component';

const routes: Routes = [
  {path:'register', component:RegisterComponent},
  {path:'forgetpassword', component:ForgotPasswordComponent},
  {path:'dashboard', component:FirstproductpageComponent},
  {path:'resetlink', component:ForgeturlpageComponent},
  {path:'profile', component:ProfileComponent},
  {path:'profileedit/:id', component:ProfileEditComponent},
  {path:'profile/address', component:AddressesPageComponent},
  {path:'profile/address/:id', component:AddressesEditComponent},

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EndrouterRoutingModule { }
