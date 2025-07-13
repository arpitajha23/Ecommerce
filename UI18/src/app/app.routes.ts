import { Routes } from '@angular/router';
import { LoginComponent } from './endrouter/auth/login/login.component';

export const routes: Routes = [
    {path:'', component:LoginComponent},
    {path: 'endroute', loadChildren: () => import('./endrouter/endrouter-routing.module').then(m => m.EndrouterRoutingModule)},
];
