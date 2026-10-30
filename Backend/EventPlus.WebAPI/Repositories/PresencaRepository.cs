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
/// Repositório responsável pela confirmação e listagem de Presenças nos eventos.
/// </summary>
public class PresencaRepository : IPresencaRepository
{
    private readonly EventContext _context;

    public PresencaRepository(EventContext context)
    {
        _context = context;
    }

    public async Task InscreverAsync(Presenca presenca)
    {
        await _context.Presenca.AddAsync(presenca);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarSituacaoAsync(Guid id, bool situacao)
    {
        var presencaBuscada = await _context.Presenca.FindAsync(id);
        if (presencaBuscada != null)
        {
            presencaBuscada.Situacao = situacao;
            _context.Presenca.Update(presencaBuscada);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeletarAsync(Guid id)
    {
        var presencaBuscada = await _context.Presenca.FindAsync(id);
        if (presencaBuscada != null)
        {
            _context.Presenca.Remove(presencaBuscada);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Presenca>> ListarAsync()
    {
        return await _context.Presenca
            .Include(p => p.IdEventoNavigation)
            .Include(p => p.IdUsuarioNavigation)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Presenca>> ListarMinhasPresencasAsync(Guid idUsuario)
    {
        return await _context.Presenca
            .Where(p => p.IdUsuario == idUsuario)
            .Include(p => p.IdEventoNavigation)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Presenca?> BuscarPorIdAsync(Guid id)
    {
        return await _context.Presenca
            .Include(p => p.IdEventoNavigation)
            .Include(p => p.IdUsuarioNavigation)
            .FirstOrDefaultAsync(p => p.IdPresenca == id);
    }
}
