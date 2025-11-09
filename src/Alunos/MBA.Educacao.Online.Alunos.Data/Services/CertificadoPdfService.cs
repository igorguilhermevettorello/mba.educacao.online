using MBA.Educacao.Online.Alunos.Domain.Interfaces.Services;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace MBA.Educacao.Online.Alunos.Data.Services
{
    public class CertificadoPdfService : ICertificadoPdfService
    {
        public Task<byte[]> GerarCertificadoPdfAsync(
            string nomeAluno,
            string nomeCurso,
            DateTime dataConclusao,
            string codigoCertificado)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(50);
                    page.PageColor(Colors.White);

                    page.Content().Column(column =>
                    {
                        column.Spacing(20);

                        // Título principal
                        column.Item().AlignCenter().Text("CERTIFICADO DE CONCLUSÃO")
                            .FontSize(32)
                            .Bold()
                            .FontColor(Colors.Blue.Darken2);

                        // Espaçamento
                        column.Item().PaddingVertical(10);

                        // Texto "Certificamos que"
                        column.Item().AlignCenter().Text("Certificamos que")
                            .FontSize(16)
                            .FontColor(Colors.Grey.Darken1);

                        // Nome do aluno
                        column.Item().AlignCenter().Text(nomeAluno)
                            .FontSize(28)
                            .Bold()
                            .FontColor(Colors.Blue.Darken3);

                        // Espaçamento
                        column.Item().PaddingVertical(5);

                        // Texto "Concluiu com sucesso o curso"
                        column.Item().AlignCenter().Text("Concluiu com sucesso o curso")
                            .FontSize(16)
                            .FontColor(Colors.Grey.Darken1);

                        // Nome do curso
                        column.Item().AlignCenter().Text(nomeCurso)
                            .FontSize(22)
                            .Bold()
                            .FontColor(Colors.Blue.Darken2);

                        // Espaçamento
                        column.Item().PaddingVertical(10);

                        // Data de conclusão
                        column.Item().AlignCenter().Text($"Concluído em {dataConclusao:dd/MM/yyyy}")
                            .FontSize(14)
                            .FontColor(Colors.Grey.Darken1);

                        // Espaçamento maior
                        column.Item().PaddingVertical(20);

                        // Código do certificado
                        column.Item().AlignCenter().Text($"Código de Autenticação: {codigoCertificado}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Medium);

                        // Borda decorativa no rodapé
                        column.Item().PaddingTop(30).AlignCenter().Row(row =>
                        {
                            row.RelativeItem().LineHorizontal(2).LineColor(Colors.Blue.Darken2);
                            row.ConstantItem(150).AlignCenter().Text("Assinatura")
                                .FontSize(12)
                                .FontColor(Colors.Grey.Darken1);
                            row.RelativeItem().LineHorizontal(2).LineColor(Colors.Blue.Darken2);
                        });
                    });

                    // Rodapé
                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("MBA Educação Online - ").FontSize(10).FontColor(Colors.Grey.Medium);
                            text.Span("www.mbaeducacao.online").FontSize(10).FontColor(Colors.Blue.Medium);
                        });
                });
            }).GeneratePdf();

            return Task.FromResult(pdfBytes);
        }
    }
}

