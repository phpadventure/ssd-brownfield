- [ ] T001 [P] Create `IFileStorageService` interface in ContosoDashboard/Services/Storage/IFileStorageService.cs
- [ ] T002 [P] Implement `LocalFileStorageService` in ContosoDashboard/Services/Storage/LocalFileStorageService.cs
- [ ] T003 [P] Create `IFileScanner` interface in ContosoDashboard/Services/Storage/IFileScanner.cs
- [ ] T004 [P] Implement `NoOpFileScanner` in ContosoDashboard/Services/Storage/NoOpFileScanner.cs
- [ ] T005 [P] Add `FileStorage:BasePath` and `FileScanner:Provider` settings to ContosoDashboard/appsettings.Development.json
- [ ] T006 Update `ContosoDashboard/Program.cs` to register `IFileStorageService`, `IFileScanner`, and ensure uploads directory exists at startup

Phase 2 — Foundational (blocking prerequisites)
- [ ] T007 Create `Document` model in ContosoDashboard/Models/Document.cs
- [ ] T008 Create `DocumentShare` and `DocumentActivity` models in ContosoDashboard/Models/DocumentShare.cs and ContosoDashboard/Models/DocumentActivity.cs
- [ ] T009 Update `Data/ApplicationDbContext.cs` to add `DbSet<Document>`, `DbSet<DocumentShare>`, and `DbSet<DocumentActivity>`
- [ ] T010 Add EF Core migration script placeholder at scripts/create-migration-document-management.sh and document migration commands in specs/001-document-management/quickstart.md
- [ ] T011 Create `IDocumentService` and stub `DocumentService` in ContosoDashboard/Services/DocumentService.cs (handles validation, storage orchestration, metadata persistence)
- [ ] T012 Add authorization policies and roles in ContosoDashboard/Program.cs for document owner, project manager, and admin checks
- [ ] T013 Add API controller scaffold `ContosoDashboard/Controllers/DocumentsController.cs` with route `/api/documents`
- [ ] T014 Add Blazor UI pages scaffold `ContosoDashboard/Pages/Documents/` including `Upload.razor`, `MyDocuments.razor`, and `DocumentDetails.razor`

Phase 3 — User Stories (priority order)

User Story 1 — Upload Documents (P1)
- [ ] T015 [US1] [P] Implement upload UI component at ContosoDashboard/Pages/Documents/Upload.razor (client-side selection, metadata form, progress indicator)
- [ ] T016 [US1] Implement server upload handler in ContosoDashboard/Controllers/DocumentsController.cs -> `POST /upload` to accept multipart/form-data and return per-file results
- [ ] T017 [US1] Implement upload workflow in ContosoDashboard/Services/DocumentService.cs: validate, scan via `IFileScanner`, save via `IFileStorageService`, persist metadata to `Documents` table
- [ ] T018 [US1] Add unit/integration tests for upload flow in tests/DocumentUploadTests.cs (covers valid upload, oversized file, unsupported type)

User Story 2 — View, Search and Download (P1)
- [ ] T019 [US2] Implement list/search endpoint in ContosoDashboard/Controllers/DocumentsController.cs (`GET /api/documents?q=&page=`) and corresponding query in ContosoDashboard/Services/DocumentService.cs
- [ ] T020 [US2] Implement download/preview endpoint in ContosoDashboard/Controllers/DocumentsController.cs (`GET /api/documents/{id}/download`) with authorization checks
- [ ] T021 [US2] Implement `MyDocuments.razor` UI at ContosoDashboard/Pages/Documents/MyDocuments.razor with sorting/filtering and preview support

User Story 3 — Project Integration & Task Attachment (P2)
- [ ] T022 [US3] Add document attachment UI to `ContosoDashboard/Pages/ProjectDetails.razor` to upload or link documents to a project/task
- [ ] T023 [US3] Ensure `DocumentService` supports associating documents with `AssociatedProjectId` and linking to tasks in ContosoDashboard/Services/DocumentService.cs

User Story 4 — Sharing & Notifications (P2)
- [ ] T024 [US4] Implement share API `POST /api/documents/{id}/share` in ContosoDashboard/Controllers/DocumentsController.cs to create `DocumentShare` records
- [ ] T025 [US4] Integrate notifications by calling existing ContosoDashboard/Services/NotificationService.cs from DocumentService when shares are created
- [ ] T026 [US4] Add UI to `DocumentDetails.razor` to manage shares and show "Shared with Me" for recipients

User Story 5 — Admin Reporting & Audit (P3)
- [ ] T027 [US5] Implement activity logging in DocumentService to write `DocumentActivity` records for upload/download/delete/share actions
- [ ] T028 [US5] Add admin reporting endpoint `ContosoDashboard/Controllers/Admin/DocumentsReportController.cs` to return aggregated metrics (top file types, top uploaders)

Final Phase — Polish & Cross-Cutting Concerns
- [ ] T029 Add integration tests in tests/integration/DocumentManagementIntegrationTests.cs covering end-to-end flows (upload → search → download → delete)
- [ ] T030 Update `.github/workflows/ci.yml` to run document management tests and verify migrations (path: .github/workflows/ci.yml)
- [ ] T031 Update documentation: merge quickstart into README.md and add migration & config notes to ContosoDashboard/README.md

Dependencies

- Phase 1 tasks (T001–T006) must complete before Foundational tasks (T007–T014) that depend on service registrations and config.
- Foundational tasks (T007–T014) must complete before story-specific implementation (T015+).

Parallel execution examples

- Parallelize: T001/T002/T003/T004/T005 (storage & scanner interfaces and implementations) can be worked on in parallel by different engineers.
- Parallelize: T007/T008/T009 (models and DbContext updates) and T011 (service skeleton) may be implemented in parallel, with coordination on migrations.
- Parallelize across stories: UI tasks (T015, T021, T022, T026) can be developed in parallel with backend endpoints (T016, T019, T024) once foundational services are available.

Independent test criteria (per story)

- US1: Upload a valid file and verify metadata and file exist; retry on unsupported type and oversized file to confirm errors.
- US2: Search returns accessible documents within 2s; preview loads for PDF/images and download returns correct bytes.
- US3: Attach document from project page; attached document appears in project document list and task view.
- US4: Share a document and verify recipient gets an in-app notification and the document appears in "Shared with Me".
- US5: Run admin report and verify returned aggregates match recent activity logs.

Suggested MVP scope

- Minimal MVP: US1 + US2 (T015–T021) to provide basic upload, listing, preview, and download functionality.

Format validation

- All tasks follow the checklist format with sequential Task IDs and file paths. Ensure any new tasks maintain the same pattern.
