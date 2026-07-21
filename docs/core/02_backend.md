# Momentum - Backend Technical Specifications

This document provides a deep dive into the backend implementation of **Momentum** (C# namespace `Lifenote.*`), detailing technology configurations, layer implementations, database contexts, and API conventions.

---

## 1. Backend Stack & Projects

The backend is built on **.NET 10 (ASP.NET Core Web API)** and C# 13. The solution contains four projects:

```
backend/
├── Lifenote.sln
├── Lifenote.Domain/           # Entities, Enums, Base Classes, Domain Exceptions
├── Lifenote.Application/      # Contracts, DTOs, Service Implementations, Validators
├── Lifenote.Infrastructure/   # DbContext, Configurations, Migrations, Firebase Admin
└── Lifenote.API/              # Controllers, Program.cs, Middlewares, API Extensions
```

---

## 2. Dependency Injection Flow

Dependencies are registered modularly using extension methods on `IServiceCollection`:

- **Application Services (`Lifenote.Application/DependencyInjection.cs`):**
  Registers application services (`INoteService`, `IGoalsService`, `ITimerService`, `IUserInfoService`).
- **Infrastructure Services (`Lifenote.Infrastructure/DependencyInjection.cs`):**
  Configures EF Core PostgreSQL DbContext, registers repositories, and initializes the Firebase Admin SDK authentication services.

```csharp
// Program.cs Initialization Pattern
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration);
```

---

## 3. Database & Entity Framework Core

### Database Configuration
- **Database Engine:** PostgreSQL
- **ORM:** Entity Framework Core (EF Core 10)
- **Primary Keys:** `Guid` (PostgreSQL `uuid` type)
- **Configuration Pattern:** Table configurations are stored as individual classes implementing `IEntityTypeConfiguration<T>` in `Lifenote.Infrastructure/Persistence/Configurations/`.

### `LifenoteDbContext` Setup
`LifenoteDbContext` inherits from `DbContext` and automatically discovers entity configurations:

```csharp
public partial class LifenoteDbContext : DbContext
{
    public virtual DbSet<ActiveTimer> ActiveTimers { get; set; }
    public virtual DbSet<FocusSession> FocusSessions { get; set; }
    public virtual DbSet<Goal> Goals { get; set; }
    public virtual DbSet<Milestone> Milestones { get; set; }
    public virtual DbSet<Note> Notes { get; set; }
    public virtual DbSet<UserInfo> UserInfos { get; set; }
    public virtual DbSet<UserPreference> UserPreferences { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LifenoteDbContext).Assembly);
        
        // Native JSON Column Mapping for PostgreSQL
        modelBuilder.Entity<UserPreference>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.OwnsOne(p => p.UI, b => b.ToJson());
            entity.OwnsOne(p => p.Notifications, b => b.ToJson());
        });
    }
}
```

---

## 4. API & Controller Standards

### `ApiControllerBase`
All ASP.NET Core controllers inherit from `ApiControllerBase`. This base class encapsulates current user context resolution from Firebase JWT bearer claims:

```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]
public abstract class ApiControllerBase : ControllerBase
{
    protected Task<Guid> GetUserIdAsync()
    {
        // Extract custom user ID claim from validated Firebase JWT token
        var userIdClaim = User.FindFirst("UserId") ?? User.FindFirst(ClaimTypes.NameIdentifier);
        return Task.FromResult(Guid.Parse(userIdClaim.Value));
    }
}
```

### Controller Design Pattern
Controllers are strictly responsible for:
1. Receiving HTTP requests.
2. Resolving the calling tenant's `userId`.
3. Calling the appropriate Application service method.
4. Returning a standardized `ApiResponse<T>`.

```csharp
[HttpGet]
public async Task<IActionResult> GetNotes()
{
    var userId = await GetUserIdAsync();
    var notes = await _noteService.GetUserNotesAsync(userId);
    return Ok(ApiResponse<List<NoteDto>>.Success(notes));
}
```

---

## 5. Global Exception Handling Pipeline

No controller should wrap business calls inside explicit `try/catch` blocks. The custom `ExceptionHandlingMiddleware` intercepts all uncaught exceptions centrally:

```csharp
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.Unauthorized, ex.Message);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
        }
    }
}
```

---

## 6. Critical Backend Guidelines

- **Asynchronous Execution:** All service and database operations MUST use `async/await` end-to-end (`Task`, `Task<T>`). Never call `.Result` or `.Wait()`.
- **Tenant Isolation:** Every service method querying user data MUST include `.Where(x => x.UserId == userId)`.
- **DTO Projection:** Domain entities must never be returned directly from API controllers to prevent schema leakage or cyclic JSON serialization issues. Always map to DTOs in the Application layer.

---

## 7. Summary Navigation

- [Product Overview](file:///d:/Lifenote/docs/core/00_overview.md)
- [Architecture Guide](file:///d:/Lifenote/docs/core/01_architecture.md)
- [Frontend Technical Deep Dive](file:///d:/Lifenote/docs/core/03_frontend.md)
- [Data Models & Schema](file:///d:/Lifenote/docs/core/04_data_models_and_schema.md)
- [Authentication & Security](file:///d:/Lifenote/docs/core/05_auth_and_security.md)
- [AI & Developer Guidelines](file:///d:/Lifenote/docs/core/06_ai_and_developer_guidelines.md)
