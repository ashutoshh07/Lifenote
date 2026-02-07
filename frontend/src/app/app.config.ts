import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection, SecurityContext } from '@angular/core';
import { provideRouter, withViewTransitions } from '@angular/router';
import { LucideAngularModule, CheckSquare, Timer, Repeat2, Settings, PanelLeftClose, PanelRightClose, StickyNote, StickyNoteIcon, Notebook } from 'lucide-angular';
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './core/interceptors/auth.interceptor';
import { errorInterceptor } from './core/interceptors/error.interceptor';
import { provideMarkdown } from 'ngx-markdown';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }), 
    provideRouter(routes, withViewTransitions()),
    importProvidersFrom(LucideAngularModule.pick({ Timer, Repeat2, Settings, PanelLeftClose, PanelRightClose, StickyNote, StickyNoteIcon, Notebook })),
    provideHttpClient(
      withInterceptors([authInterceptor, errorInterceptor]) // <-- Add your generated interceptor here
    ),
    provideMarkdown()
  ]
};
