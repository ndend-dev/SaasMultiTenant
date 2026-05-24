import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: '',
        loadComponent: () => import('./pages/login/login.component').then(m => m.LoginComponent)
    },
    {
        path: 'login',
        loadComponent: () => import('./pages/login/login.component').then(m => m.LoginComponent)
    },
    {
        path: 'dashboard/:workspaceId',
        loadComponent: () => import('./pages/dashboard/dashboard.component').then(m => m.DashboardComponent),
    }
];
