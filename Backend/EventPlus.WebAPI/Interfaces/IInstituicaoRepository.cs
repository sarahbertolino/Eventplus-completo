using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces;

/// <summary>
/// Interface do Repositório para a entidade <see cref="Instituicao"/>.
/// </summary>
public interface IInstituicaoRepository
{
    Task CadastrarAsync(Instituicao instituicao);
    Task AtualizarAsync(Guid id, Instituicao instituicao);
    Task DeletarAsync(Guid id);
    Task<List<Instituicao>> ListarAsync();
    Task<Instituicao?> BuscarPorIdAsync(Guid id);
}
