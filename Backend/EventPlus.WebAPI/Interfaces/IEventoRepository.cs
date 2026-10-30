using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces;

/// <summary>
/// Interface do Repositório para a entidade <see cref="Evento"/>.
/// </summary>
public interface IEventoRepository
{
    /// <summary>
    /// Cadastra um novo evento no banco de dados.
    /// </summary>
    Task CadastrarAsync(Evento evento);

    /// <summary>
    /// Atualiza as informações de um evento existente.
    /// </summary>
    Task AtualizarAsync(Guid id, Evento evento);

    /// <summary>
    /// Remove um evento pelo seu ID.
    /// </summary>
    Task DeletarAsync(Guid id);

    /// <summary>
    /// Lista todos os eventos com suas respectivas navegações (TipoEvento e Instituicao).
    /// </summary>
    Task<List<Evento>> ListarAsync();

    /// <summary>
    /// Lista apenas os eventos futuros a partir da data atual.
    /// </summary>
    Task<List<Evento>> ListarProximosAsync();

    /// <summary>
    /// Busca um evento específico pelo seu ID.
    /// </summary>
    Task<Evento?> BuscarPorIdAsync(Guid id);
}
