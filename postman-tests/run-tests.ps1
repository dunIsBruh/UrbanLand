param(
    [string]$BaseUrl = "http://localhost:5271",
    [string]$ReportDir = "newman-results"
)

$ErrorActionPreference = "Stop"

# Check Node.js availability
try {
    node -v | Out-Null
} catch {
    throw "Node.js is not installed or not in PATH"
}

# Create report directory
New-Item -ItemType Directory -Path $ReportDir -Force | Out-Null

$timestamp = Get-Date -Format "yyyyMMdd-HHmmss"
$reportFile = Join-Path $ReportDir "report-$timestamp.html"
$jsonReport = Join-Path $ReportDir "report-$timestamp.json"

$collectionFile = Join-Path $PSScriptRoot "UrbanLand_API_Tests.postman_collection.json"
$envFile = Join-Path $PSScriptRoot "UrbanLand_API_Environment.postman_environment.json"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  UrbanLand API - Automated Tests" -ForegroundColor Cyan
Write-Host "  Base URL : $BaseUrl" -ForegroundColor Cyan
Write-Host "  Report   : $reportFile" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Run Newman via npx (no global installs needed)
npx --yes `
    --package newman `
    --package newman-reporter-htmlextra `
    newman run $collectionFile `
    --environment $envFile `
    --env-var "baseUrl=$BaseUrl" `
    --reporters cli,htmlextra,json `
    --reporter-htmlextra-export $reportFile `
    --reporter-json-export $jsonReport `
    --delay-request 150 `
    --timeout-request 10000

$exitCode = $LASTEXITCODE

Write-Host ""
if ($exitCode -eq 0) {
    Write-Host "[PASS] All tests completed successfully!" -ForegroundColor Green
    Write-Host "[INFO] HTML report: $reportFile" -ForegroundColor Green
} else {
    Write-Host "[FAIL] Tests completed with errors (exit code: $exitCode)" -ForegroundColor Red
    Write-Host "[INFO] Check the report for details: $reportFile" -ForegroundColor Red
}

exit $exitCode