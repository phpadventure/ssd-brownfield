# Research: Document Management (Phase 0)

**Feature**: Document Management
**Date**: 2026-06-09

## Decision: Malware scanning approach

**Chosen**: Option D — implement `IFileScanner` abstraction with a safe default for training.

**Rationale**: Training environments must be reproducible and lightweight. A pluggable `IFileScanner` preserves the security requirement while allowing the default training deployment to avoid heavyweight dependencies. Documentation will describe how to enable ClamAV (or other scanners) in CI or production.

**Alternatives considered**:
- A: Integrate ClamAV immediately — realistic but increases setup complexity for learners.
- B: Manual/admin review — insecure and not automated.
- C: Only content-type checks — insufficient to meet NFR-004 for realistic deployments.

**Implementation Notes**:
- Add `IFileScanner` interface to `ContosoDashboard/Services/Storage` alongside `IFileStorageService`.
- Provide `NoOpFileScanner` for training (returns clean) and document `ClamAvFileScanner` integration steps.
- Add configuration flag `FileScanner:Provider` in `appsettings.json` to select implementation.

## Decision: Storage path pattern

**Chosen**: `{userId}/{projectId or "personal"}/{guid}.{extension}` (matches stakeholder recommendation)

**Rationale**: Prevents collisions, supports migration to blob storage, and avoids using user-supplied filenames in paths.

## Decision: Storage location

**Chosen**: Store files outside `wwwroot` in `AppData/uploads` (or configurable path), served via authorized controller endpoints.

## Open Questions / NEEDS CLARIFICATION (none remain)

All stakeholder requirements and clarifications have been resolved in the spec and research above.