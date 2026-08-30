# Momentum - AI & Developer Guidelines

This document establishes strict engineering guidelines, conventions, and pitfall prevention rules for AI coding assistants and human developers contributing to **Momentum**.

---

## 1. Naming & Rebranding Rules

> [!IMPORTANT]
> - **Product Display Name:** **Momentum** (Use in all UI text, header titles, user documentation, and public strings).
> - **Code Namespaces & Solution:** **`Lifenote`** (Do NOT rename C# namespaces `Lifenote.Domain`, `Lifenote.Application`, `Lifenote.Infrastructure`, `Lifenote.API` or folder structures unless explicitly instructed).

---

## 2. Data Typing & Identifier Rules (CRITICAL)

- **Primary & Foreign Keys:** ALL identifiers are **`Guid`** in C# and **`string`** (UUID format) in TypeScript.
- **Forbidden Types:** **Do NOT use `int` or `number` for IDs anywhere in the stack.**
- **Router Parameters:** When parsing IDs in Angular router snapshots, grab them directly as strings without parsing to numbers:
  ```typescript
  // Correct
  const id = this.route.snapshot.paramMap.get('id');
  
  // Incorrect (DO NOT DO THIS)
  const id = Number(this.route.snapshot.paramMap.get('id'));
  ```

---

## 3. Backend Code Standards (.NET 10 Web API)

1. **Clean Architecture Boundaries:**
   - `Domain`: Enterprise entities and interfaces ONLY. Zero third-party dependencies.
   - `Application`: Business contracts, use cases, DTOs.
   - `Infrastructure`: EF Core DbContext, PostgreSQL configurations, Firebase Admin.
   - `API`: Thin controllers parsing identity and returning `ApiResponse<T>`.

2. **Async/Await Rule:**
   - Always use `async/await` completely through the invocation stack.
   - **Never call `.Result` or `.Wait()`**, which causes thread-pool starvation and deadlocks.

3. **Exception Pipeline:**
   - Controllers must NOT use inline `try/catch` blocks for expected application flow.
   - Throw specific exceptions (`NotFoundException`, `UnauthorizedAccessException`) and allow `ExceptionHandlingMiddleware` to catch and format the HTTP response.

4. **Tenant Isolation:**
   - Every service method fetching or mutating user data must accept `Guid userId` and append `.Where(x => x.UserId == userId)`.

---

## 4. Frontend Code Standards (Angular 19)

1. **Signals-First State Management:**
   - Use Angular Signals (`signal<T>`, `computed()`, `effect()`) for component local state and reactivity.
   - Use RxJS `Observable<T>` primarily for HTTP calls in services. Subscribe in components and set the emission into a Signal.

2. **No Silent Failures:**
   - Every background API error or successful action MUST notify the user via `ToastService.show(message, type)`.
   - Never rely solely on `console.error()`.

3. **Disabled Loading States:**
   - All buttons, forms, and interactive triggers must visually indicate loading states via `[disabled]="isLoading()"` to prevent double execution.

4. **Feature Isolation:**
   - Features (`/features/notes`, `/features/goals`, etc.) should remain self-contained.
   - A feature can import from `/core` or `/shared`, but must NOT import directly from another feature directory to prevent circular dependencies.

5. **Design System & Styling:**
   - Use SCSS with CSS Custom Properties defined in `styles.scss`.
   - Use modern typography (Inter, Outfit), smooth CSS micro-animations, glassmorphism, and responsive hover effects.
   - **Do NOT introduce Tailwind CSS unless explicitly requested.**

---

## 5. Summary Checklist Before Submitting Code

- [ ] Does the UI refer to the app as **Momentum**?
- [ ] Are all entity IDs defined as `Guid` (C#) / `string` (TS)?
- [ ] Are backend controllers thin, returning `ApiResponse<T>`?
- [ ] Are all database operations asynchronous (`async/await`)?
- [ ] Is tenant data isolation (`.Where(x => x.UserId == userId)`) enforced?
- [ ] Are Angular components using Standalone syntax and Signals?
- [ ] Is user feedback dispatched via `ToastService` on error/success?

---

## 6. Summary Navigation

- [Product Overview](file:///d:/Lifenote/docs/core/00_overview.md)
- [Architecture Guide](file:///d:/Lifenote/docs/core/01_architecture.md)
- [Backend Technical Deep Dive](file:///d:/Lifenote/docs/core/02_backend.md)
- [Frontend Technical Deep Dive](file:///d:/Lifenote/docs/core/03_frontend.md)
- [Data Models & Schema](file:///d:/Lifenote/docs/core/04_data_models_and_schema.md)
- [Authentication & Security](file:///d:/Lifenote/docs/core/05_auth_and_security.md)
