# Data Model: Document Management

## Entities

### Document
- **Table**: Documents
- **Primary Key**: `DocumentId` (int)
- **Fields**:
  - `DocumentId` (int, PK)
  - `Title` (string, required)
  - `Description` (string, nullable)
  - `Category` (string, required)
  - `Tags` (string, nullable) — comma-separated or separate normalized tag table (TBD)
  - `AssociatedProjectId` (int, nullable) — FK to `Projects` if applicable
  - `UploadedByUserId` (int, required) — FK to `Users`
  - `UploadDateUtc` (datetime, required)
  - `FileSizeBytes` (long, required)
  - `FileType` (string, length 255)
  - `StoragePath` (string, required) — relative path or blob name

### DocumentShare
- **Table**: DocumentShares
- **Primary Key**: `ShareId` (int)
- **Fields**:
  - `ShareId` (int, PK)
  - `DocumentId` (int, FK -> Documents)
  - `SharedWithUserId` (int, nullable) — either user or team
  - `SharedWithTeamId` (int, nullable)
  - `SharedByUserId` (int, required)
  - `SharedAtUtc` (datetime)

### DocumentActivity
- **Table**: DocumentActivities
- **Primary Key**: `ActivityId` (int)
- **Fields**:
  - `ActivityId` (int, PK)
  - `DocumentId` (int, FK)
  - `Action` (string) — upload/download/delete/share
  - `ActorUserId` (int)
  - `TimestampUtc` (datetime)
  - `Details` (string, optional)

## Indexes & Performance
- Index `UploadedByUserId` for user listing queries.
- Index `AssociatedProjectId` for project document queries.
- Full-text or indexed columns on `Title`, `Tags`, and `Description` for search performance.

## Storage Path Pattern
- Use `{userId}/{projectId_or_personal}/{guid}.{ext}` as the storage key. Store files outside `wwwroot` and serve via authorized controller endpoints.

## Migration Notes
- `DocumentId` is integer to match repository convention. When adding the new tables, include migrations that backfill any required metadata defaults.
