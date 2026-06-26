Write-Host "Starting build and publish process..." -ForegroundColor Cyan

# 1. Clean publish directory
$publishDir = "publish"
if (Test-Path $publishDir) {
    Write-Host "Cleaning existing publish directory..." -ForegroundColor Yellow
    Remove-Item -Path $publishDir -Recurse -Force
}
New-Item -ItemType Directory -Path $publishDir | Out-Null

# 2. Build frontend (Angular)
Write-Host "Building Angular Frontend..." -ForegroundColor Yellow
cd frontend
if (-not (Test-Path "node_modules")) {
    Write-Host "node_modules not found, running npm install..." -ForegroundColor Green
    npm install
}
npm run build
cd ..

# 3. Copy frontend build files to backend wwwroot
Write-Host "Copying Angular build files to WebAPI wwwroot..." -ForegroundColor Yellow
$frontendBuildPath = "frontend/dist/frontend/browser"
$backendWwwRoot = "backend/src/Portfolio.WebAPI/wwwroot"

if (-not (Test-Path $frontendBuildPath)) {
    Write-Host "Error: Angular build folder not found at $frontendBuildPath!" -ForegroundColor Red
    exit 1
}

# Ensure backend wwwroot exists
if (-not (Test-Path $backendWwwRoot)) {
    New-Item -ItemType Directory -Path $backendWwwRoot | Out-Null
}

# Copy all files from frontend build to backend wwwroot
Copy-Item -Path "$frontendBuildPath/*" -Destination $backendWwwRoot -Recurse -Force

# 4. Publish backend (ASP.NET Core Web API)
Write-Host "Publishing ASP.NET Core Backend..." -ForegroundColor Yellow
dotnet publish backend/src/Portfolio.WebAPI/Portfolio.WebAPI.csproj -c Release -o $publishDir

Write-Host ""
Write-Host "==========================================================" -ForegroundColor Green
Write-Host "Build and Publish successful!" -ForegroundColor Green
Write-Host "All files are generated in the '$publishDir' directory." -ForegroundColor Green
Write-Host "You can now zip the files inside '$publishDir' and upload them to MonsterASP.net!" -ForegroundColor Cyan
Write-Host "==========================================================" -ForegroundColor Green
