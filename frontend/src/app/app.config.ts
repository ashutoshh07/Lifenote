import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter, withViewTransitions } from '@angular/router';
import { LucideAngularModule, CheckSquare, Timer, Repeat2, Settings, PanelLeftClose, PanelRightClose, StickyNote, StickyNoteIcon, Notebook } from 'lucide-angular';

import { routes } from './app.routes';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

export const appConfig: ApplicationConfig = {
  providers: [
    BrowserAnimationsModule,
    provideZoneChangeDetection({ eventCoalescing: true }), 
    provideRouter(routes, withViewTransitions()),
    importProvidersFrom(LucideAngularModule.pick({ Timer, Repeat2, Settings, PanelLeftClose, PanelRightClose, StickyNote, StickyNoteIcon, Notebook }))
  ]
};
