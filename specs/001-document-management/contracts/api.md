# API Contracts: Document Management

Base path: `/api/documents`

## Endpoints

### POST /api/documents/upload
- Description: Upload one or more files with metadata.
- Request (multipart/form-data):
  - `files` (file[]) — uploaded files
  - `title` (string) — required per file (can accept per-file metadata via JSON part)
  - `description` (string, optional)
  - `category` (string, required)
  - `associatedProjectId` (int, optional)
  - `tags` (string[], optional)
- Response: `201 Created` with JSON body: `{ "documentIds": [int] }` or per-file results with status/errors

### GET /api/documents/{id}
- Description: Get document metadata (not file contents).
- Response: `200 OK` with JSON:
  ```json
  {
    "documentId": 123,
    "title": "...",
    "description": "...",
    "category": "Project Documents",
    "tags": ["design", "spec"],
    "associatedProjectId": 5,
    "uploadedByUserId": 2,
    "uploadDateUtc": "2026-06-09T12:00:00Z",
    "fileSizeBytes": 12345,
    "fileType": "application/pdf"
  }
  ```

### GET /api/documents/{id}/download
- Description: Download the file if the caller is authorized.
- Response: `200 OK` with file stream and appropriate `Content-Type` and `Content-Disposition` headers.

### GET /api/documents
- Description: List/search documents. Query params:
  - `q` (string) — full-text search over title/description/tags
  - `category` (string)
  - `projectId` (int)
  - `uploadedBy` (int)
  - `sort` (string) — e.g., `uploadDate_desc`
  - `page`, `pageSize`
- Response: paginated list with metadata objects.

### PUT /api/documents/{id}
- Description: Update metadata (title, description, category, tags). Only owners or authorized roles.
- Request: JSON with updatable fields.
- Response: `200 OK` with updated metadata.

### POST /api/documents/{id}/replace
- Description: Replace underlying file while preserving metadata; requires owner or authorized role.
- Request: multipart/form-data with new file.
- Response: `200 OK` or `400/413` on errors.

### DELETE /api/documents/{id}
- Description: Permanently delete document and file after confirmation. Authorization: owner or project manager/admin.
- Response: `204 No Content`.

### POST /api/documents/{id}/share
- Description: Share document with users or teams.
- Request JSON: `{ "userIds": [int], "teamIds": [int], "notify": true }`
- Response: `200 OK` with list of successful shares.

## DTOs
- `UploadResult`:
  - `documentId` (int)
  - `status` (string)
  - `errors` (string[])

- `DocumentMetadata` — as shown in GET `/api/documents/{id}` response.

## Authorization
- All endpoints require authentication. Additional role checks:
  - `DELETE`: owner OR ProjectManager OR Administrator
  - `PUT/replace`: owner OR ProjectManager OR Administrator
  - `GET /download`: only users with access to the document (owner, project members, recipients of share, admins)

## Errors
- `400 Bad Request` — validation failed (unsupported type, missing title, file too large)
- `401 Unauthorized` — not authenticated
- `403 Forbidden` — authenticated but not authorized
- `404 Not Found` — document id not found
- `413 Payload Too Large` — file size exceeds limit
