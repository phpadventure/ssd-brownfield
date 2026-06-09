# Quickstart: Document Management (Local Validation)

## Prerequisites
- .NET 8 SDK installed
- SQL Server LocalDB (or configured LocalDB equivalent)
- From repo root: run in shell, `cd ContosoDashboard`

## Run the application locally

1. Ensure `appsettings.Development.json` has a configurable `FileStorage:BasePath` (default: `AppData/uploads`).
2. Start the app:

```bash
cd ContosoDashboard
dotnet run
```

3. Open browser: `http://localhost:5000` and login using the mock login page.

## Validate the feature manually

1. Navigate to the Document Upload page (created under `/documents` or via UI link).
2. Click Upload → choose a small PDF (under 5 MB) → fill required metadata (Title + Category) → Upload.
3. Verify:
   - Upload completes and success shown
   - Document appears under "My Documents"
   - Preview for PDF opens in browser
   - Download returns the same file

## Quick API test (curl)

Upload a single file using curl (adjust port and auth cookie as appropriate):

```bash
curl -v -X POST "http://localhost:5000/api/documents/upload" \
  -F "files=@/path/to/file.pdf" \
  -F "title=Test Doc" \
  -F "category=Personal Files"
```

Expect a `201 Created` with JSON containing `documentIds`.

## Test Notes
- If database migrations are required, run EF migration commands or let the application auto-create/seed LocalDB on first run.
- To test malware scanning behavior in training, use `IFileScanner` provider configuration: set provider to `NoOpFileScanner` (default) or `ClamAvFileScanner` if available.

## Troubleshooting
- If files are not serving, confirm `FileStorage:BasePath` exists and the app process has read/write permissions.
- If uploads fail due to size, ensure `MaxRequestBodySize` is configured in Kestrel and `InputFile` max stream size matches limit.
