import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
    {
        path: 'login',
        loadComponent: () => import('./features/auth/pages/login-page/login-page.component').then(m => m.LoginPageComponent)
    },
    {
        path: '',
        canActivate: [authGuard],
        children: [
            {
                path: '',
                redirectTo: '/notes',
                pathMatch: 'full'
            },
            {
                path: 'notes',
                loadComponent: () => import('./features/notes/pages/notes-page/notes-page.component')
                    .then(m => m.NotesPageComponent)
            },
            {
                path: 'pomodoro',
                loadComponent: () => import('./features/pomodoro/pages/pomodoro-page/pomodoro-page.component')
                    .then(m => m.PomodoroPageComponent)
            },
            {
                path: 'habits',
                loadComponent: () => import('./features/habits/pages/habits-page/habits-page.component')
                    .then(m => m.HabitsPageComponent)
            },
            {
                path: 'settings',
                loadComponent: () => import('./features/settings/pages/settings-page/settings-page.component')
                    .then(m => m.SettingsPageComponent)
            },
        ]
    },
    // Redirect any other path to the login page
    { path: '**', redirectTo: 'login' }
];