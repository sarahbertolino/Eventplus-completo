using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repositories;

/// <summary>
/// Repositório responsável pelo gerenciamento de Tipos/Categorias de Eventos.
/// </summary>
public class TipoEventoRepository : ITipoEventoRepository
{
    private readonly EventContext _context;

    public TipoEventoRepository(EventContext context)
    {
        _context = context;
    }

    public async Task CadastrarAsync(TipoEvento tipoEvento)
    {
        await _context.TipoEvento.AddAsync(tipoEvento);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Guid id, TipoEvento tipoEvento)
    {
        var tipoBuscado = await _context.TipoEvento.FindAsync(id);
        if (tipoBuscado != null)
        {
            tipoBuscado.Titulo = tipoEvento.Titulo;
            _context.TipoEvento.Update(tipoBuscado);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeletarAsync(Guid id)
    {
        var tipoBuscado = await _context.TipoEvento.FindAsync(id);
        if (tipoBuscado != null)
        {
            _context.TipoEvento.Remove(tipoBuscado);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<TipoEvento>> ListarAsync()
    {
        return await _context.TipoEvento.AsNoTracking().ToListAsync();
    }

    public async Task<TipoEvento?> BuscarPorIdAsync(Guid id)
    {
        return await _context.TipoEvento.FirstOrDefaultAsync(t => t.IdTipoEvento == id);
    }
}
