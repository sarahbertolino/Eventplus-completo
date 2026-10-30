using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repositories;

/// <summary>
/// Repositório responsável pelos Perfis/Tipos de Usuários.
/// </summary>
public class TipoUsuarioRepository : ITipoUsuarioRepository
{
    private readonly EventContext _context;

    public TipoUsuarioRepository(EventContext context)
    {
        _context = context;
    }

    public async Task CadastrarAsync(TipoUsuario tipoUsuario)
    {
        await _context.TipoUsuario.AddAsync(tipoUsuario);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Guid id, TipoUsuario tipoUsuario)
    {
        var tipoBuscado = await _context.TipoUsuario.FindAsync(id);
        if (tipoBuscado != null)
        {
            tipoBuscado.Titulo = tipoUsuario.Titulo;
            _context.TipoUsuario.Update(tipoBuscado);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeletarAsync(Guid id)
    {
        var tipoBuscado = await _context.TipoUsuario.FindAsync(id);
        if (tipoBuscado != null)
        {
            _context.TipoUsuario.Remove(tipoBuscado);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<TipoUsuario>> ListarAsync()
    {
        return await _context.TipoUsuario.AsNoTracking().ToListAsync();
    }

    public async Task<TipoUsuario?> BuscarPorIdAsync(Guid id)
    {
        return await _context.TipoUsuario.FirstOrDefaultAsync(t => t.IdTipoUsuario == id);
    }
}
