# Script para executar testes com cobertura e gerar relatório
Write-Host "Executando testes com cobertura de codigo..." -ForegroundColor Cyan

# Limpar resultados anteriores
if (Test-Path "TestResults") {
    Remove-Item -Recurse -Force "TestResults"
}

# Executar testes com cobertura
dotnet test MBA.Educacao.Online.sln --collect:"XPlat Code Coverage" --results-directory ./TestResults

# Verificar se ReportGenerator está instalado
Write-Host "`n Verificando instalacao do ReportGenerator..." -ForegroundColor Cyan
$reportGeneratorInstalled = dotnet tool list -g | Select-String "reportgenerator"

if (-not $reportGeneratorInstalled) {
    Write-Host "Instalando ReportGenerator como ferramenta global..." -ForegroundColor Yellow
    dotnet tool install -g dotnet-reportgenerator-globaltool
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Erro ao instalar ReportGenerator. Abortando..." -ForegroundColor Red
        exit 1
    }
}

# Encontrar o executável do ReportGenerator
$userProfile = $env:USERPROFILE
$dotnetToolsPath = Join-Path $userProfile ".dotnet\tools"
$reportGeneratorExe = Join-Path $dotnetToolsPath "reportgenerator.exe"

if (-not (Test-Path $reportGeneratorExe)) {
    Write-Host "Erro: Nao foi possivel encontrar o executavel do ReportGenerator em $dotnetToolsPath" -ForegroundColor Red
    Write-Host "Tentando usar o comando direto..." -ForegroundColor Yellow
    $reportGeneratorExe = "reportgenerator"
}

# Gerar relatório HTML
Write-Host "`n Gerando relatorio de cobertura..." -ForegroundColor Cyan
& $reportGeneratorExe -reports:"TestResults/**/coverage.cobertura.xml" -targetdir:"TestResults/CoverageReport" -reporttypes:Html

if ($LASTEXITCODE -ne 0) {
    Write-Host "Erro ao gerar relatorio de cobertura." -ForegroundColor Red
    exit 1
}

# Verificar se o relatório foi gerado antes de abrir
$reportPath = Join-Path $PSScriptRoot "TestResults\CoverageReport\index.html"
if (Test-Path $reportPath) {
    Write-Host "`n Relatorio gerado com sucesso!" -ForegroundColor Green
    Write-Host "Abrindo relatorio no navegador..." -ForegroundColor Cyan
    Start-Process $reportPath
    Write-Host "`n Concluido! O relatorio foi aberto no navegador." -ForegroundColor Green
} else {
    Write-Host "`n Erro: Relatorio nao foi gerado. Verifique os logs acima." -ForegroundColor Red
    exit 1
}
