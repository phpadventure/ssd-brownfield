<!--
SYNC IMPACT REPORT
Version change: TODO(unknown) -> 1.0.0
Modified principles: Added concrete principles for training, offline-first, security, testing, observability/versioning
Added sections: Additional Constraints, Development Workflow
Removed sections: None
Templates requiring updates: .specify/templates/* -> ⚠ pending (no templates found in repository)
Follow-up TODOs: RATIFICATION_DATE (unknown) — TODO(RATIFICATION_DATE): confirm original adoption date
-->

# ContosoDashboard Constitution

## Core Principles

### 1. Training-Centric Documentation (MUST)
All code, examples, and exercises MUST be written for clarity and reproducibility in a training context. Documentation
MUST clearly indicate which parts are mock implementations and which are production-grade guidance. Rationale: the
repository is used for Spec-Driven Development training and must prioritize teachability and predictable outcomes.

### 2. Offline-First & Reproducible (MUST)
The project MUST run in an offline, local environment without external cloud dependencies. Infrastructure abstractions
MUST be used so that production replacements (Azure SQL, Blob Storage, Entra ID) can be swapped without changing
business logic. Rationale: training environments must be reliable and simple to set up.

### 3. Security-Informed Training (MUST)
Security controls included for training (mock authentication, role-based examples) MUST be clearly labeled as training
implementations. Any changes to authentication, authorization, or data access MUST include a section describing how to
harden for production (password hashing, MFA, audit logging). Rationale: prevent accidental promotion of mock code
to production.

### 4. Test-First & Verifiable (SHOULD / MUST for critical flows)
Tests SHOULD be written alongside new features. Critical flows (authentication, authorization, data migrations, core
business logic) MUST have automated tests. Rationale: ensure examples remain verifiable and reproducible across training
runs.

### 5. Simplicity, Observability & Semantic Versioning (MUST)
Keep examples simple and focused. Instrument services with structured logging and clear diagnostics to aid learning and
debugging. Versioning MUST follow semantic versioning for released artifacts and project-level governance. Rationale:
learners must be able to reason about changes and track breaking changes.

## Additional Constraints

Technology constraints for this repository are intentionally prescriptive for training reproducibility:
- Framework: ASP.NET Core 8 (Blazor Server)
- Database: SQL Server LocalDB for local development
- Authentication: Mock cookie-based authentication for training only

All infrastructure integrations MUST be abstracted behind interfaces to enable swap-in of production implementations.

## Development Workflow

- All work MUST be done on feature branches and merged via pull requests.
- Pull requests MUST include a summary of changes, link to related spec/task, and a testing checklist.
- Code reviews MUST verify adherence to the Principles above, including tests for critical flows, clear documentation of
	mock vs. production code, and observability hooks.
- Automated CI SHOULD run tests and basic static analysis before merging.

## Governance

The constitution defines core non-negotiable expectations for this training repository. Amendments MUST be recorded with
rationale, author, and migration steps when they affect developer workflows or student-facing examples. Minor wording
clarifications are allowed as patch updates; additions or redefinitions of principles require a minor or major version bump
depending on impact.

Amendment rules:
- Patch: Non-semantic clarifications, typo fixes.
- Minor: Additive principles or procedural updates that do not break existing guidance.
- Major: Removal or redefinition of principles that change governance expectations.

**Version**: 1.0.0 | **Ratified**: TODO(RATIFICATION_DATE): confirm original adoption date | **Last Amended**: 2026-06-09
