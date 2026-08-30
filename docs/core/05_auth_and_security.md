# Momentum - Authentication & Security Architecture

This document describes the authentication, token validation, user mapping, and multi-tenant data isolation mechanisms built into **Momentum**.

---

## 1. Authentication Architecture Overview

Momentum relies on **Firebase Authentication** for identity management (email/password, OAuth providers), paired with custom JWT Bearer token validation and claim transformation in the .NET 10 Web API backend.

```
+----------------+          +-------------------+          +--------------------+
|  Angular SPA   | -------- |   Firebase Auth   | -------- | .NET 10 Web API    |
|  (Client App)  |          | (Identity Provider)|         | (Lifenote.API)     |
+----------------+          +-------------------+          +--------------------+
        |                             |                              |
        | 1. Sign In / Authenticate   |                              |
        | --------------------------> |                              |
        |                             |                              |
        | 2. ID Token (JWT)           |                              |
        | <-------------------------- |                              |
        |                                                            |
        | 3. HTTP Request + Header [ Authorization: Bearer <JWT> ]    |
        | ---------------------------------------------------------> |
        |                                                            |
        |                                   4. Validate JWT Token    |
        |                                   via Firebase Admin SDK   |
        |                                                            |
        |                                   5. Extract FirebaseUid   |
        |                                   Map to internal Guid UserId
        |                                                            |
        | 6. ApiResponse<T> Data                                     |
        | <--------------------------------------------------------- |
```

---

## 2. Frontend Security Mechanisms

### 1. `AuthService` (`frontend/src/app/core/services/auth.service.ts`)
- Wraps Firebase Client JS SDK functions (`signInWithEmailAndPassword`, `createUserWithEmailAndPassword`, `signOut`).
- Exposes a `currentUser` Angular Signal representing authenticated user state.
- Stores and automatically refreshes Firebase ID Tokens.

### 2. `JwtInterceptor` (`frontend/src/app/core/interceptors/jwt.interceptor.ts`)
- Intercepts all outgoing HTTP requests originating from the Angular application.
- Injects `Authorization: Bearer <token>` into HTTP headers when targeting the backend API URL (`environment.apiUrl`).

### 3. Route Protection (`frontend/src/app/core/guards/auth.guard.ts`)
- Angular Route Guard checking `AuthService.isAuthenticated()`.
- Unauthenticated users attempting to access protected routes (`/notes`, `/goals`, `/pomodoro`, `/settings`) are automatically redirected to `/auth/login`.

---

## 3. Backend Security & Token Resolution

### 1. Firebase Admin SDK Integration
In `Lifenote.Infrastructure/DependencyInjection.cs`, ASP.NET Core Authentication is configured with JWT Bearer validation:

```csharp
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://securetoken.google.com/{firebaseProjectId}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://securetoken.google.com/{firebaseProjectId}",
            ValidateAudience = true,
            ValidAudience = firebaseProjectId,
            ValidateLifetime = true
        };
    });
```

### 2. Identity Transformation & `ApiControllerBase`
When a valid JWT hits an endpoint:
1. Firebase `sub` (Firebase UID) is extracted from JWT claims.
2. `UserInfoService` resolves or creates the corresponding `UserInfo` entity in PostgreSQL.
3. The internal system **`Guid`** (e.g., `userInfo.Id`) is bound to the controller context.
4. Controllers access identity via `await GetUserIdAsync()`.

---

## 4. Multi-Tenant Data Isolation Rules

To prevent cross-tenant data access attacks or accidental leakage:

1. **Service Parameter Requirement:** Every service method in `Lifenote.Application` that fetches, updates, or deletes records MUST take `Guid userId` as an explicit parameter.
2. **Mandatory EF Core Query Filter:** Queries MUST filter records using `.Where(x => x.UserId == userId)`.
3. **No Direct ID Mutations:** Endpoints that update entities by ID must verify that the target entity belongs to `userId` before mutating data.

```csharp
// Example: Strict Tenant Ownership Check
var note = await _context.Notes
    .FirstOrDefaultAsync(n => n.Id == noteId && n.UserId == userId);

if (note == null)
{
    throw new NotFoundException("Note not found or access denied.");
}
```

---

## 5. Summary Navigation

- [Product Overview](file:///d:/Lifenote/docs/core/00_overview.md)
- [Architecture Guide](file:///d:/Lifenote/docs/core/01_architecture.md)
- [Backend Technical Deep Dive](file:///d:/Lifenote/docs/core/02_backend.md)
- [Frontend Technical Deep Dive](file:///d:/Lifenote/docs/core/03_frontend.md)
- [Data Models & Schema](file:///d:/Lifenote/docs/core/04_data_models_and_schema.md)
- [AI & Developer Guidelines](file:///d:/Lifenote/docs/core/06_ai_and_developer_guidelines.md)
