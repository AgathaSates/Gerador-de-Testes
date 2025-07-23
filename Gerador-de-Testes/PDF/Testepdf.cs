namespace Gerador_de_Testes.WebApp.PDF;

using System.Collections.Generic;
using Gerador_de_Testes.Dominio.ModuloMateria;
using QuestPDF.Drawing;
using QuestPDF.Elements;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

public class TestePdfDocument : IDocument
{
    public string Titulo { get; }
    public string Disciplina { get; }
    public int QuantidadeQuestoes { get; }
    public bool ProvaRecuperacao { get; }
    public string Materia { get; }
    public List<QuestaoDto> Questoes { get; }
    public bool MostrarGabarito { get; }

    public TestePdfDocument(
        string titulo,
        string disciplina,
        int quantidadeQuestoes,
        bool provaRecuperacao,
        string materia,
        List<QuestaoDto> questoes,
        bool mostrarGabarito = false
    )
    {
        Titulo = titulo;
        Disciplina = disciplina;
        QuantidadeQuestoes = quantidadeQuestoes;
        ProvaRecuperacao = provaRecuperacao;
        Materia = materia;
        Questoes = questoes;
        MostrarGabarito = mostrarGabarito;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(30);
            page.Size(PageSizes.A4);
            page.DefaultTextStyle(x => x.FontSize(12));

            page.Header().Element(e =>
                e.PaddingBottom(10)
                 .AlignCenter()
                 .Text($"Teste - {Titulo}")
                 .SemiBold()
                 .FontSize(20)
            );

            page.Content().Element(ComposeContent);
        });
    }

    void ComposeContent(IContainer container)
    {
        string materiaTexto = ProvaRecuperacao
            ? "Prova de Recuperação"
            : Materia;

        container.Column(col =>
        {
            col.Item().Text($"Disciplina: {Disciplina}").Bold();
            col.Item().Text($"Matéria: {materiaTexto}").Bold();
            col.Item().PaddingVertical(10);

            int numero = 1;
            foreach (var questao in Questoes)
            {
                col.Item().Element(c =>
                {
                    c.Column(q =>
                    {
                        q.Item().Text($"{numero++}. {questao.Enunciado}").Bold();

                        foreach (var alt in questao.Alternativas)
                        {
                            var textoAlternativa = $"{alt.Letra}) {alt.Texto}";
                            if (MostrarGabarito && alt.Correta)
                                textoAlternativa += " ✅";

                            q.Item().Element(e => e.PaddingLeft(15).Text(textoAlternativa));
                        }
                    });
                });

                col.Item().PaddingBottom(10);
            }
        });
    }

    public class QuestaoDto
    {
        public string Enunciado { get; set; }
        public List<AlternativaDto> Alternativas { get; set; }
    }

    public class AlternativaDto
    {
        public string Letra { get; set; }
        public string Texto { get; set; }
        public bool Correta { get; set; }
    }
}

