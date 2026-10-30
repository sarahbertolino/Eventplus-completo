using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces;

/// <summary>
/// Interface do Repositório para a entidade <see cref="Comentario"/>.
/// Define o contrato de métodos assíncronos para manipulação de comentários.
/// </summary>
public interface IComentarioRepository
{
    /// <summary>
    /// Cadastra um novo comentário no banco de dados.
    /// </summary>
    /// <param name="comentario">Objeto do comentário a ser salvo.</param>
    Task CadastrarAsync(Comentario comentario);

    /// <summary>
    /// Remove um comentário do banco de dados pelo seu ID.
    /// </summary>
    /// <param name="id">Identificador do comentário (Guid).</param>
    Task DeletarAsync(Guid id);

    /// <summary>
    /// Lista todos os comentários cadastrados no sistema.
    /// </summary>
    /// <returns>Lista de comentários.</returns>
    Task<List<Comentario>> ListarAsync();

    /// <summary>
    /// Lista apenas os comentários pertencentes a um evento específico.
    /// </summary>
    /// <param name="idEvento">Identificador único do evento.</param>
    /// <returns>Lista de comentários vinculados ao evento.</returns>
    Task<List<Comentario>> ListarPorEventoAsync(Guid idEvento);

    /// <summary>
    /// Busca um comentário pelo seu ID.
    /// </summary>
    /// <param name="id">Identificador do comentário (Guid).</param>
    /// <returns>O comentário encontrado ou null.</returns>
    Task<Comentario?> BuscarPorIdAsync(Guid id);
}
