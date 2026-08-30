# Momentum - Product & Ecosystem Overview

Welcome to **Momentum** (codebase namespace: `Lifenote`), a modern, high-performance personal productivity ecosystem. Momentum integrates long-term goal planning, structured task/milestone management, rich text/markdown note management, and Pomodoro focus tracking into a unified, aesthetically rich application.

---

## 1. Product Vision & Core Mission

Momentum was built to solve the fragmentation of personal management tools. Rather than switching between separate apps for notes, timers, and goal tracking, Momentum unites them into a responsive, single-pane productivity dashboard.

### Core Value Pillars
- **Unified Productivity:** Seamless connection between abstract goals, actionable milestones, active focus timers, and supporting notes.
- **Privacy & Isolation:** Strict tenant-level isolation ensuring user data privacy across all API boundaries.
- **Premium User Experience:** Modern aesthetic standards (glassmorphism, smooth micro-animations, theme flexibility) paired with non-blocking, responsive UI interactions.

---

## 2. Key Modules & Features

### 🎯 Goal & Milestone Tracking
- **Goal Management:** Create, update, track, and categorize personal/professional goals.
- **Milestone Breakdown:** Deconstruct large goals into actionable, incremental sub-milestones with target completion dates and progress calculation.
- **Status & Metrics:** Track active, completed, and archived goals with visual progress bars.

### 📝 Notes & Knowledge Base
- **Note Management:** Create, edit, and organize notes.
- **Rich Editor:** Interactive note editor supporting structured formatting, tags, categories, and fast text filtering.
- **Context Linking:** Ability to reference and attach notes to productivity workflows.

### ⏱️ Pomodoro & Focus Sessions
- **Live Active Timer:** Real-time Pomodoro timer supporting configurable focus intervals, short breaks, and long breaks.
- **Session Telemetry:** Persistence of completed focus sessions (`FocusSession`) for habit analytics and productivity tracking.
- **State Resilience:** Active timer state persistence in PostgreSQL to prevent data loss on browser refresh or device handoff.

### ⚙️ User Settings & Personalization
- **Profile & Identity:** Firebase Authentication integration mapped to an internal system user profile (`UserInfo`).
- **UI Customization:** Theme mode switcher (Dark/Light mode), layout settings, and customizable timer defaults stored as JSON documents in EF Core (`UserPreference`).
- **Notification Controls:** User-configurable alerts and reminder preferences.

---

## 3. Rebranding & Naming Convention

> [!IMPORTANT]
> **Naming Rule for Developers & AI Assistants:**
> The user-facing product name is **Momentum**. However, backend C# namespaces, solution files (`Lifenote.sln`), and folder structures retain the legacy prefix `Lifenote` to avoid breaking changes across project dependencies.
> - **UI / User Facing Text:** Use **Momentum**.
> - **Code Namespaces / Files:** Keep `Lifenote.*` (e.g., `Lifenote.Domain`, `Lifenote.Application`, `Lifenote.API`).

---

## 4. Technology Stack Overview

| Layer | Technology | Key Details |
| :--- | :--- | :--- |
| **Frontend Framework** | Angular 19 | Standalone Components, Signal-based reactivity, RxJS HTTP handling |
| **Frontend Styling** | SCSS | Custom Design System, CSS variables, glassmorphism, responsive UI |
| **Backend Framework** | .NET 10 Web API | C# 13, Clean Architecture pattern, Thin Controllers |
| **Database** | PostgreSQL | Managed via EF Core 10, UUID Primary Keys (`Guid`), JSON columns |
| **Authentication** | Firebase Auth | Firebase Admin SDK, Bearer JWT validation, tenant claim mapping |
| **Containerization** | Docker | Multi-stage Dockerfile builds for API deployment |

---

## 5. Summary Navigation

- [Architecture Guide](file:///d:/Lifenote/docs/core/01_architecture.md)
- [Backend Deep Dive](file:///d:/Lifenote/docs/core/02_backend.md)
- [Frontend Deep Dive](file:///d:/Lifenote/docs/core/03_frontend.md)
- [Data Models & Schema](file:///d:/Lifenote/docs/core/04_data_models_and_schema.md)
- [Authentication & Security](file:///d:/Lifenote/docs/core/05_auth_and_security.md)
- [AI & Developer Guidelines](file:///d:/Lifenote/docs/core/06_ai_and_developer_guidelines.md)
