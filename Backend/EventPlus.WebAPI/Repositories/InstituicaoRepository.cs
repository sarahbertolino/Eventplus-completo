using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repositories;

/// <summary>
/// Repositório responsável pelo gerenciamento de Instituições.
/// </summary>
public class InstituicaoRepository : IInstituicaoRepository
{
    private readonly EventContext _context;

    public InstituicaoRepository(EventContext context)
    {
        _context = context;
    }

    public async Task CadastrarAsync(Instituicao instituicao)
    {
        await _context.Instituicao.AddAsync(instituicao);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Guid id, Instituicao instituicao)
    {
        var instBuscada = await _context.Instituicao.FindAsync(id);
        if (instBuscada != null)
        {
            instBuscada.CNPJ = instituicao.CNPJ;
            instBuscada.NomeFantasia = instituicao.NomeFantasia;
            instBuscada.Endereco = instituicao.Endereco;

            _context.Instituicao.Update(instBuscada);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeletarAsync(Guid id)
    {
        var instBuscada = await _context.Instituicao.FindAsync(id);
        if (instBuscada != null)
        {
            _context.Instituicao.Remove(instBuscada);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Instituicao>> ListarAsync()
    {
        return await _context.Instituicao.AsNoTracking().ToListAsync();
    }

    public async Task<Instituicao?> BuscarPorIdAsync(Guid id)
    {
        return await _context.Instituicao.FirstOrDefaultAsync(i => i.IdInstituicao == id);
    }
}
