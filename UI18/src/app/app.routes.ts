import { Routes } from '@angular/router';
import { LoginComponent } from './endrouter/auth/login/login.component';
import { LinkExpiredComponent } from './endrouter/auth/link-expired/link-expired.component';

export const routes: Routes = [
    {path:'', component:LoginComponent},
    { path: 'link-expired', component: LinkExpiredComponent },

    {path: 'endroute', loadChildren: () => import('./endrouter/endrouter-routing.module').then(m => m.EndrouterRoutingModule)},
];
