# Momentum - Data Models & Database Schema

This document details the Domain entities, Entity Framework Core PostgreSQL database mapping, Primary Key strategy, DTO contracts, and multi-tenant data isolation rules for **Momentum**.

---

## 1. Primary Key & Identifier Strategy

> [!CRITICAL]
> **GUID / UUID Rule:**
> All entities use **`Guid`** (PostgreSQL `uuid`) for Primary Keys and Foreign Keys across the entire backend stack (`Lifenote.Domain`, `Lifenote.Application`, `Lifenote.Infrastructure`, `Lifenote.API`).
> On the Angular frontend, these map strictly to **`string`**.
> **Never use `int` or `number` for IDs.**

---

## 2. Entity Specifications & ER Schema

### 1. `UserInfo`
Represents the mapped system user profile synchronized from Firebase Auth.

```csharp
public class UserInfo : BaseEntity<Guid>
{
    public string FirebaseUid { get; set; }  // Unique identifier from Firebase JWT
    public string Email { get; set; }
    public string DisplayName { get; set; }
    public string PhotoUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastLoginAt { get; set; }
}
```

### 2. `UserPreference`
Stores user profile preferences. Utilizes EF Core native JSON column mapping (`ToJson()`) for flexible configuration objects.

```csharp
public class UserPreference : BaseEntity<Guid>
{
    public Guid UserId { get; set; }
    public UserUIPreference UI { get; set; }           // Mapped to JSON column
    public NotificationPreference Notifications { get; set; } // Mapped to JSON column
}
```

### 3. `Goal` & `Milestone`
Represents user goals and actionable sub-milestones.

```csharp
public class Goal : AggregateRoot<Guid>
{
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public DateTime? TargetDate { get; set; }
    public bool IsCompleted { get; set; }
    public int ProgressPercentage { get; set; }
    public ICollection<Milestone> Milestones { get; set; }
}

public class Milestone : BaseEntity<Guid>
{
    public Guid GoalId { get; set; }
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? DueDate { get; set; }
}
```

### 4. `Note`
Represents markdown notes created by users.

```csharp
public class Note : BaseEntity<Guid>
{
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Category { get; set; }
    public List<string> Tags { get; set; }
    public bool IsPinned { get; set; }
    public bool IsArchived { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

### 5. `FocusSession` & `ActiveTimer`
Represents completed focus sessions and active timer state persistence.

```csharp
public class FocusSession : BaseEntity<Guid>
{
    public Guid UserId { get; set; }
    public int DurationMinutes { get; set; }
    public string Mode { get; set; }            // "Focus", "ShortBreak", "LongBreak"
    public DateTime CompletedAt { get; set; }
    public string Notes { get; set; }
}

public class ActiveTimer : BaseEntity<Guid>
{
    public Guid UserId { get; set; }
    public DateTime StartTime { get; set; }
    public int DurationSeconds { get; set; }
    public string State { get; set; }           // "Running", "Paused", "Idle"
    public string Mode { get; set; }            // "Focus", "ShortBreak", "LongBreak"
}
```

---

## 3. Entity Framework Core Configuration & Mapping

Table names and property constraints are mapped via `IEntityTypeConfiguration<T>` in `Lifenote.Infrastructure/Persistence/Configurations/`:

| Entity | DB Table Name | Foreign Keys | Indexing & Constraints |
| :--- | :--- | :--- | :--- |
| `UserInfo` | `user_infos` | None | Unique index on `FirebaseUid` |
| `UserPreference` | `user_preferences` | `UserId` -> `user_infos(id)` | Owned JSON columns (`UI`, `Notifications`) |
| `Goal` | `goals` | `UserId` -> `user_infos(id)` | Index on `UserId` |
| `Milestone` | `milestones` | `GoalId` -> `goals(id)` | Cascade delete on goal deletion |
| `Note` | `notes` | `UserId` -> `user_infos(id)` | Index on `UserId`, `Category` |
| `FocusSession` | `focus_sessions` | `UserId` -> `user_infos(id)` | Index on `UserId`, `CompletedAt` |
| `ActiveTimer` | `active_timers` | `UserId` -> `user_infos(id)` | Unique index on `UserId` |

---

## 4. Multi-Tenant Data Isolation Rule

Every user-owned entity implements a `UserId` property (`Guid`). To prevent cross-tenant data leaks:

```csharp
// Application Layer Enforcement Pattern
public async Task<List<NoteDto>> GetUserNotesAsync(Guid userId)
{
    return await _context.Notes
        .AsNoTracking()
        .Where(n => n.UserId == userId && !n.IsArchived)
        .Select(n => new NoteDto { ... })
        .ToListAsync();
}
```

---

## 5. Summary Navigation

- [Product Overview](file:///d:/Lifenote/docs/core/00_overview.md)
- [Architecture Guide](file:///d:/Lifenote/docs/core/01_architecture.md)
- [Backend Technical Deep Dive](file:///d:/Lifenote/docs/core/02_backend.md)
- [Frontend Technical Deep Dive](file:///d:/Lifenote/docs/core/03_frontend.md)
- [Authentication & Security](file:///d:/Lifenote/docs/core/05_auth_and_security.md)
- [AI & Developer Guidelines](file:///d:/Lifenote/docs/core/06_ai_and_developer_guidelines.md)
