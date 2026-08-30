# Momentum - Frontend Technical Specifications

This document provides technical details on the frontend architecture of **Momentum**, built with Angular 19 Standalone Components, Angular Signals, RxJS, and a custom SCSS design system.

---

## 1. Tech Stack & Key Principles

- **Framework:** Angular 19 (Standalone Components, `imports: [...]`)
- **Language:** TypeScript 5+ (Strict Mode enabled)
- **State Management:** Angular Signals (`signal`, `computed`, `effect`) for UI state; RxJS (`Observable`) for HTTP services
- **Styling:** SCSS with CSS Custom Properties, modern typography, glassmorphism, and micro-animations (No Tailwind CSS)
- **Authentication Client:** Firebase JavaScript SDK

---

## 2. Component Architecture & Directory Layout

The application code resides under `frontend/src/app/`:

```
src/app/
├── core/                  # Global singletons (Services, Guards, Interceptors, Models)
│   ├── constants/         # Magic string replacements, API routes, defaults
│   ├── guards/            # Auth Guard, Guest Guard
│   ├── interceptors/      # JwtInterceptor (Injects Bearer Token), ErrorInterceptor
│   ├── models/            # Shared interfaces & API contracts
│   ├── services/          # AuthService, ThemeService, ToastService, LayoutService
│   └── utils/             # Date formatters, string helper utilities
│
├── features/              # Feature modules (Isolated domains)
│   ├── auth/              # Sign-in, sign-up, password reset views
│   ├── goals/             # Goal cards, milestone checklists, creation modal
│   ├── home/              # Dashboard summary widgets
│   ├── notes/             # Note editor, list view, categorization
│   ├── pomodoro/          # Interactive timer component, session history metrics
│   └── settings/          # User preferences, theme settings, profile controls
│
├── shared/                # Reusable UI components & directives
│   ├── components/        # app-button, app-modal, app-toast, app-card
│   ├── directives/        # Tooltip directives, auto-focus
│   └── pipes/             # Time formatting, markdown rendering
│
└── layout/                # Main Application Frame
    ├── sidebar/           # Navigation sidebar
    ├── topbar/            # Search, theme toggle, user profile avatar menu
    └── app-layout.component.ts
```

---

## 3. Reactive State Management Pattern

Momentum uses a **Signals-First** state management pattern combined with RxJS for asynchronous operations:

1. **Services** execute HTTP requests via Angular's `HttpClient` returning RxJS `Observable<T>`.
2. **Page Components** subscribe to the service call and push data directly into an Angular `signal<T>()`.
3. **Template UI** consumes the signal directly (e.g., `{{ notes() }}`) or via `computed()` signals.

```typescript
@Component({
  standalone: true,
  selector: 'app-notes-page',
  templateUrl: './notes-page.component.html'
})
export class NotesPageComponent implements OnInit {
  private notesService = inject(NotesService);
  private toastService = inject(ToastService);

  // State Signals
  readonly notes = signal<Note[]>([]);
  readonly isLoading = signal<boolean>(false);
  readonly noteCount = computed(() => this.notes().length);

  ngOnInit(): void {
    this.fetchNotes();
  }

  fetchNotes(): void {
    this.isLoading.set(true);
    this.notesService.getNotes().subscribe({
      next: (data) => {
        this.notes.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.toastService.show('Failed to load notes', 'error');
        this.isLoading.set(false);
      }
    });
  }
}
```

---

## 4. Core Services Overview

| Service | Location | Responsibility |
| :--- | :--- | :--- |
| `AuthService` | `/core/services/auth.service.ts` | Handles Firebase login/logout, persists ID token, exposes `currentUser` signal. |
| `ThemeService` | `/core/services/theme.service.ts` | Manages `dark` / `light` theme signal, toggles CSS classes on `<body>`. |
| `ToastService` | `/core/services/toast.service.ts` | Dispatches temporary toast alerts (`success`, `error`, `info`). |
| `LayoutService` | `/core/services/layout.service.ts` | Controls sidebar collapsed/expanded state signal. |
| `BreakpointService` | `/core/services/breakpoint.service.ts` | Observes viewport width to support responsive UI logic. |

---

## 5. UI / UX & Design System Guidelines

- **No Silent Failures:** Every user-initiated API action MUST trigger feedback using `ToastService.show(message, type)`. Console logging alone is forbidden.
- **Disabled State Standard:** Forms, buttons, and interactive inputs must reflect loading state via `[disabled]="isLoading()"` to prevent double submissions.
- **CSS Variables & Tokens:** Colors, gradients, shadows, and spacing are controlled via global CSS variables in `src/styles.scss` (e.g., `--color-bg-primary`, `--color-accent`, `--glass-background`).
- **Typography:** Modern sans-serif typefaces (e.g., Inter, Outfit).

---

## 6. Summary Navigation

- [Product Overview](file:///d:/Lifenote/docs/core/00_overview.md)
- [Architecture Guide](file:///d:/Lifenote/docs/core/01_architecture.md)
- [Backend Technical Deep Dive](file:///d:/Lifenote/docs/core/02_backend.md)
- [Data Models & Schema](file:///d:/Lifenote/docs/core/04_data_models_and_schema.md)
- [Authentication & Security](file:///d:/Lifenote/docs/core/05_auth_and_security.md)
- [AI & Developer Guidelines](file:///d:/Lifenote/docs/core/06_ai_and_developer_guidelines.md)
