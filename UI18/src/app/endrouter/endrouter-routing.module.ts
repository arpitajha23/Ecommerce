import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './auth/register/register.component';
import { FirstproductpageComponent } from './Dashboard/firstproductpage/firstproductpage.component';
import { ForgotPasswordComponent } from './auth/forgot-password/forgot-password.component';
import { ForgeturlpageComponent } from './auth/forgeturlpage/forgeturlpage.component';

const routes: Routes = [
  {path:'register', component:RegisterComponent},
  {path:'forgetpassword', component:ForgotPasswordComponent},
  {path:'dashboard', component:FirstproductpageComponent},
  {path:'resetlink', component:ForgeturlpageComponent},

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EndrouterRoutingModule { }
