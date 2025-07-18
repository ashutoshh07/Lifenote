import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';
import { AppComponent } from './app/app.component';
import { routes } from './app/app.routes';
import { Check, Home, LucideAngularModule, PanelLeftClose, PanelRightClose, Repeat2, Settings, Timer } from 'lucide-angular';
import { importProvidersFrom } from '@angular/core';

bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(routes),
    importProvidersFrom(LucideAngularModule.pick({ Home, PanelLeftClose, PanelRightClose, Check, Timer, Repeat2, Settings })),
    provideAnimations(), // For Angular Material animations
    // Add more providers as needed
  ]
}).catch(err => console.error(err));