#!/usr/bin/env pwsh
param(
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $PSScriptRoot

Write-Host "==> Restaurando dependencias" -ForegroundColor Cyan
dotnet restore (Join-Path $root "LigaLibre.sln")
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host "==> Compilando solución ($Configuration)" -ForegroundColor Cyan
dotnet build (Join-Path $root "LigaLibre.sln") --configuration $Configuration --no-restore
exit $LASTEXITCODE
