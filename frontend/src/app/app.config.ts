import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { LucideAngularModule, CheckSquare, Timer, Repeat2, Settings, PanelLeftClose, PanelRightClose } from 'lucide-angular';

import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }), 
    provideRouter(routes),
    importProvidersFrom(LucideAngularModule.pick({ CheckSquare, Timer, Repeat2, Settings, PanelLeftClose, PanelRightClose }))
  ]
};
