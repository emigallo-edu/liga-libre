#!/usr/bin/env pwsh
param(
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $PSScriptRoot

Write-Host "==> Ejecutando pruebas ($Configuration)" -ForegroundColor Cyan
dotnet test (Join-Path $root "LigaLibre.sln") `
    --configuration $Configuration `
    --no-build `
    --logger "trx" `
    --results-directory (Join-Path $root "TestResults")
exit $LASTEXITCODE
