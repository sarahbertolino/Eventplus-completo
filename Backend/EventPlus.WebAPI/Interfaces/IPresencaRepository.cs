using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces;

/// <summary>
/// Interface do Repositório para a entidade <see cref="Presenca"/>.
/// </summary>
public interface IPresencaRepository
{
    Task InscreverAsync(Presenca presenca);
    Task AtualizarSituacaoAsync(Guid id, bool situacao);
    Task DeletarAsync(Guid id);
    Task<List<Presenca>> ListarAsync();
    Task<List<Presenca>> ListarMinhasPresencasAsync(Guid idUsuario);
    Task<Presenca?> BuscarPorIdAsync(Guid id);
}
