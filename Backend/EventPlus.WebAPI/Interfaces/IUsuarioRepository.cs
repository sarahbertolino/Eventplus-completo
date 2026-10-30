using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces;

/// <summary>
/// Interface do Repositório para a entidade <see cref="Usuario"/>.
/// </summary>
public interface IUsuarioRepository
{
    Task CadastrarAsync(Usuario usuario);
    Task AtualizarAsync(Guid id, Usuario usuario);
    Task DeletarAsync(Guid id);
    Task<List<Usuario>> ListarAsync();
    Task<Usuario?> BuscarPorIdAsync(Guid id);
    Task<Usuario?> BuscarPorEmailESenhaAsync(string email, string senha);
}
