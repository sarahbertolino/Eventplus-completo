using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces;

/// <summary>
/// Interface do Repositório para a entidade <see cref="TipoEvento"/>.
/// </summary>
public interface ITipoEventoRepository
{
    Task CadastrarAsync(TipoEvento tipoEvento);
    Task AtualizarAsync(Guid id, TipoEvento tipoEvento);
    Task DeletarAsync(Guid id);
    Task<List<TipoEvento>> ListarAsync();
    Task<TipoEvento?> BuscarPorIdAsync(Guid id);
}
