# Bakery Store - convert C# and Razor source files to UTF-8 with BOM.
# Preserves all existing code content and original formatting.
$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$project = Join-Path $root 'BakeryStore.csproj'
if (-not (Test-Path -LiteralPath $project)) {
    Write-Host 'KHONG TIM THAY BakeryStore.csproj!' -ForegroundColor Red
    Write-Host 'Hay chep 2 file SUA_LOI_TIENG_VIET.cmd va FixUTF8.ps1 vao CUNG THU MUC voi BakeryStore.csproj.' -ForegroundColor Yellow
    exit 1
}

$reader = New-Object System.Text.UTF8Encoding($false, $true)
$writer = New-Object System.Text.UTF8Encoding($true)
$changed = 0
$skipped = 0
$already = 0
$files = @(Get-ChildItem -LiteralPath $root -Recurse -File | Where-Object { $_.Extension -in @('.cshtml', '.cs') -and $_.FullName -notmatch '[\\/](bin|obj|packages)[\\/]' })
foreach ($f in $files) {
    $path = $f.FullName
    $bytes = [System.IO.File]::ReadAllBytes($path)
    if ($bytes.Length -ge 3 -and $bytes[0] -eq 239 -and $bytes[1] -eq 187 -and $bytes[2] -eq 191) {
        $already++
        continue
    }
    try {
        $content = $reader.GetString($bytes)
    }
    catch {
        Write-Host ('BO QUA (khong phai UTF-8 hop le): ' + $path) -ForegroundColor Yellow
        $skipped++
        continue
    }
    $backupPath = $path + '.before_utf8_fix'
    if (-not (Test-Path -LiteralPath $backupPath)) {
        [System.IO.File]::Copy($path, $backupPath)
    }
    [System.IO.File]::WriteAllText($path, $content, $writer)
    $changed++
}
Write-Host ('DA XONG! Da sua {0} file; co san BOM: {1}; bo qua: {2}.' -f $changed, $already, $skipped) -ForegroundColor Green
Write-Host 'Khong thay doi Web.config hay database. File goc duoc sao luu duoi duoi .before_utf8_fix.'
Write-Host 'Tiep theo: dong IIS Express, Build lai, F5, Ctrl+F5 tren Chrome.'
