# Momentum Core Documentation (`docs/core/`)

This directory contains the central technical and architectural documentation for **Momentum** (codebase namespace: `Lifenote`). It is designed for both human developer onboarding and AI agent context parsing.

---

## 📚 Core Documentation Index

| # | Document | Purpose & Description |
| :-: | :--- | :--- |
| **00** | **[Product Overview](file:///d:/Lifenote/docs/core/00_overview.md)** | Product vision, core modules (Goals, Notes, Pomodoro, Settings), branding rules (Momentum vs Lifenote), and tech stack summary. |
| **01** | **[System Architecture](file:///d:/Lifenote/docs/core/01_architecture.md)** | End-to-end client-server topology, backend Clean Architecture layers, and feature-based Angular client structure. |
| **02** | **[Backend Specifications](file:///d:/Lifenote/docs/core/02_backend.md)** | .NET 10 Web API deep dive, EF Core PostgreSQL configuration, Dependency Injection, and `ExceptionHandlingMiddleware`. |
| **03** | **[Frontend Specifications](file:///d:/Lifenote/docs/core/03_frontend.md)** | Angular 19 Standalone components, Angular Signals + RxJS state management, layout framework, core services, and SCSS design system. |
| **04** | **[Data Models & Schema](file:///d:/Lifenote/docs/core/04_data_models_and_schema.md)** | Domain entity definitions, `Guid` key rules, database tables, owned JSON column mappings, and DTO contracts. |
| **05** | **[Auth & Security](file:///d:/Lifenote/docs/core/05_auth_and_security.md)** | Firebase Auth integration, JWT Bearer Token validation, custom claim mapping, and multi-tenant data isolation rules. |
| **06** | **[AI & Developer Guidelines](file:///d:/Lifenote/docs/core/06_ai_and_developer_guidelines.md)** | Mandatory engineering conventions, coding rules, common pitfall prevention, and pre-submission checklist. |

---

## 🚀 Quick Reference for AI Assistants

When analyzing or extending the **Momentum** repository:
1. Read **[06_ai_and_developer_guidelines.md](file:///d:/Lifenote/docs/core/06_ai_and_developer_guidelines.md)** first to ensure strict adherence to project patterns.
2. Refer to **[04_data_models_and_schema.md](file:///d:/Lifenote/docs/core/04_data_models_and_schema.md)** for entity models and `Guid` identifier rules.
3. Keep backend namespace `Lifenote.*` unchanged, but present the UI application as **Momentum**.
