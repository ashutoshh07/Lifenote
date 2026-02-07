import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection, SecurityContext } from '@angular/core';
import { provideRouter, withViewTransitions } from '@angular/router';
import { LucideAngularModule, CheckSquare, Timer, Repeat2, Settings, PanelLeftClose, PanelRightClose, StickyNote, StickyNoteIcon, Notebook } from 'lucide-angular';
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './core/interceptors/auth.interceptor';
import { errorInterceptor } from './core/interceptors/error.interceptor';
import { provideMarkdown } from 'ngx-markdown';
import ClipboardJS from 'clipboard';
import 'prismjs';
import 'prismjs/plugins/line-numbers/prism-line-numbers';
import 'prismjs/plugins/command-line/prism-command-line';
import 'prismjs/components/prism-typescript.min.js';
import 'prismjs/components/prism-javascript.min.js';
import 'prismjs/components/prism-json.min.js';
import 'prismjs/components/prism-css.min.js';
import 'prismjs/components/prism-markup.min.js';
import 'prismjs/components/prism-csharp.min.js';
import 'prismjs/components/prism-bash.min.js';
import 'prismjs/components/prism-python.min.js';


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
