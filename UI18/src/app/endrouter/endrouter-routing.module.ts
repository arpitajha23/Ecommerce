import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './auth/register/register.component';
import { FirstproductpageComponent } from './Dashboard/firstproductpage/firstproductpage.component';

const routes: Routes = [
  {path:'register', component:RegisterComponent},
  {path:'forgetpassword', component:RegisterComponent},
  {path:'dashboard', component:FirstproductpageComponent},

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EndrouterRoutingModule { }
