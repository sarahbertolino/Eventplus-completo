using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces;

/// <summary>
/// Interface do Repositório para a entidade <see cref="TipoUsuario"/>.
/// </summary>
public interface ITipoUsuarioRepository
{
    Task CadastrarAsync(TipoUsuario tipoUsuario);
    Task AtualizarAsync(Guid id, TipoUsuario tipoUsuario);
    Task DeletarAsync(Guid id);
    Task<List<TipoUsuario>> ListarAsync();
    Task<TipoUsuario?> BuscarPorIdAsync(Guid id);
}
