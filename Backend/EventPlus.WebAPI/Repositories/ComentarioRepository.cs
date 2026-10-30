using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repositories;

/// <summary>
/// Repositório responsável pelo acesso a dados da entidade <see cref="Comentario"/>.
/// 
/// O QUE É O REPOSITORY PATTERN?
/// O padrão Repository centraliza todas as operações de banco de dados (CRUD) em um único local,
/// evitando duplicar consultas SQL ou chamadas do Entity Framework em diferentes controllers.
/// </summary>
public class ComentarioRepository : IComentarioRepository
{
    private readonly EventContext _context;

    /// <summary>
    /// Construtor com Injeção de Dependência do <see cref="EventContext"/>.
    /// </summary>
    /// <param name="context">Instância do contexto do banco de dados.</param>
    public ComentarioRepository(EventContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Cadastra um novo comentário no banco de dados.
    /// </summary>
    public async Task CadastrarAsync(Comentario comentario)
    {
        comentario.DataComentario = DateTime.Now;
        await _context.Comentario.AddAsync(comentario);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Remove um comentário pelo seu ID.
    /// </summary>
    public async Task DeletarAsync(Guid id)
    {
        var comentarioBuscado = await _context.Comentario.FindAsync(id);
        if (comentarioBuscado != null)
        {
            _context.Comentario.Remove(comentarioBuscado);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Lista todos os comentários do sistema com os dados das navegações de Evento e Usuario.
    /// </summary>
    public async Task<List<Comentario>> ListarAsync()
    {
        return await _context.Comentario
            .Include(c => c.IdEventoNavigation)
            .Include(c => c.IdUsuarioNavigation)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Lista apenas os comentários vinculados a um evento específico.
    /// </summary>
    public async Task<List<Comentario>> ListarPorEventoAsync(Guid idEvento)
    {
        return await _context.Comentario
            .Where(c => c.IdEvento == idEvento && c.Exibe == true)
            .Include(c => c.IdUsuarioNavigation)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Busca um comentário por seu ID.
    /// </summary>
    public async Task<Comentario?> BuscarPorIdAsync(Guid id)
    {
        return await _context.Comentario
            .Include(c => c.IdEventoNavigation)
            .Include(c => c.IdUsuarioNavigation)
            .FirstOrDefaultAsync(c => c.IdComentario == id);
    }
}
