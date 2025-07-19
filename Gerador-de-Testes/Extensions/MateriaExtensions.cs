using Gerador_de_Testes.Dominio.ModuloDisciplina;
using Gerador_de_Testes.Dominio.ModuloMateria;
using Gerador_de_Testes.WebApp.Models;
using System.Net;

namespace Gerador_de_Testes.WebApp.Extensions;

public static class MateriaExtensions
{
    public static Materia ParaEntidade(this FormularioMateriaViewModel formVm, List<Disciplina> disciplinas)
    {
        var disciplina = disciplinas.FirstOrDefault(d => d.Id.Equals(formVm.DisciplinaId));
        return new Materia(formVm.Nome, formVm.Serie, disciplina!);
    }

    public static DetalhesMateriaViewModel ParaDetalhes(this Materia materia)
    {
        return new DetalhesMateriaViewModel(materia.Id, materia.Nome, materia.Serie, materia.Disciplina.Nome);
    }
}
