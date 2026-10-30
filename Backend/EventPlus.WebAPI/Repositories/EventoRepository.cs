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
/// Repositório responsável pelo gerenciamento de dados de Eventos.
/// </summary>
public class EventoRepository : IEventoRepository
{
    private readonly EventContext _context;

    public EventoRepository(EventContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Cadastra um novo evento no banco de dados.
    /// </summary>
    public async Task CadastrarAsync(Evento evento)
    {
        await _context.Evento.AddAsync(evento);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Atualiza os dados de um evento já cadastrado.
    /// </summary>
    public async Task AtualizarAsync(Guid id, Evento evento)
    {
        var eventoBuscado = await _context.Evento.FindAsync(id);
        if (eventoBuscado != null)
        {
            eventoBuscado.Nome = evento.Nome;
            eventoBuscado.Descricao = evento.Descricao;
            eventoBuscado.DataEvento = evento.DataEvento;
            eventoBuscado.IdTipoEvento = evento.IdTipoEvento;
            eventoBuscado.IdInstituicao = evento.IdInstituicao;

            if (!string.IsNullOrEmpty(evento.ImagemUrl))
            {
                eventoBuscado.ImagemUrl = evento.ImagemUrl;
            }

            _context.Evento.Update(eventoBuscado);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Remove um evento pelo seu ID.
    /// </summary>
    public async Task DeletarAsync(Guid id)
    {
        var eventoBuscado = await _context.Evento.FindAsync(id);
        if (eventoBuscado != null)
        {
            _context.Evento.Remove(eventoBuscado);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Lista todos os eventos ordenados por data.
    /// </summary>
    public async Task<List<Evento>> ListarAsync()
    {
        return await _context.Evento
            .Include(e => e.IdTipoEventoNavigation)
            .Include(e => e.IdInstituicaoNavigation)
            .OrderBy(e => e.DataEvento)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Lista apenas eventos futuros (DataEvento >= DateTime.Now).
    /// </summary>
    public async Task<List<Evento>> ListarProximosAsync()
    {
        return await _context.Evento
            .Where(e => e.DataEvento >= DateTime.Now)
            .Include(e => e.IdTipoEventoNavigation)
            .Include(e => e.IdInstituicaoNavigation)
            .OrderBy(e => e.DataEvento)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Busca um evento por seu ID.
    /// </summary>
    public async Task<Evento?> BuscarPorIdAsync(Guid id)
    {
        return await _context.Evento
            .Include(e => e.IdTipoEventoNavigation)
            .Include(e => e.IdInstituicaoNavigation)
            .FirstOrDefaultAsync(e => e.IdEvento == id);
    }
}
