# Script para executar testes com cobertura e gerar relatório
Write-Host "🧪 Executando testes com cobertura de código..." -ForegroundColor Cyan

# Limpar resultados anteriores
if (Test-Path "TestResults") {
    Remove-Item -Recurse -Force "TestResults"
}

# Executar testes com cobertura
dotnet test MBA.Educacao.Online.sln --collect:"XPlat Code Coverage" --results-directory ./TestResults

# Gerar relatório HTML
Write-Host "`n📊 Gerando relatório de cobertura..." -ForegroundColor Cyan
reportgenerator -reports:"TestResults/**/coverage.cobertura.xml" -targetdir:"TestResults/CoverageReport" -reporttypes:Html

# Abrir relatório no navegador
Write-Host "`n✅ Relatório gerado com sucesso!" -ForegroundColor Green
Write-Host "📂 Abrindo relatório no navegador..." -ForegroundColor Cyan
Start-Process "TestResults/CoverageReport/index.html"

Write-Host "`n✨ Concluído! O relatório foi aberto no navegador." -ForegroundColor Green

