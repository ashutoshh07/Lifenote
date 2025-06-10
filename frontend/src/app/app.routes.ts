import { Routes } from '@angular/router';
import {  } from './features/tasks/pages/tasks-page/tasks-page.component'

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/tasks',
    pathMatch: 'full'
  },
  {
    path: 'tasks',
    loadComponent: () => import('./features/tasks/pages/tasks-page/tasks-page.component')
      .then(m => m.TasksPageComponent)
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
  {
    path: '**',
    redirectTo: '/tasks' // Fallback route
  }
];