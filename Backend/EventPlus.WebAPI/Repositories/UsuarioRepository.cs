using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using EventPlus.WebAPI.Utils;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repositories;

/// <summary>
/// Repositório responsável pelo gerenciamento de Usuários no banco de dados.
/// </summary>
public class UsuarioRepository : IUsuarioRepository
{
    private readonly EventContext _context;

    public UsuarioRepository(EventContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Cadastra um novo usuário no banco de dados. A senha é criptografada com BCrypt antes de salvar.
    /// </summary>
    public async Task CadastrarAsync(Usuario usuario)
    {
        // Criptografamos a senha antes de salvar (nunca salvamos em texto puro)
        usuario.Senha = Criptografia.GerarHash(usuario.Senha);

        await _context.Usuario.AddAsync(usuario);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Atualiza os dados cadastrais do usuário (Nome e Tipo). E-mail e Senha têm fluxo separado.
    /// </summary>
    public async Task AtualizarAsync(Guid id, Usuario usuario)
    {
        var usuarioBuscado = await _context.Usuario.FindAsync(id);
        if (usuarioBuscado != null)
        {
            usuarioBuscado.Nome = usuario.Nome;
            usuarioBuscado.Email = usuario.Email;
            usuarioBuscado.IdTipoUsuario = usuario.IdTipoUsuario;

            // Se uma nova senha foi fornecida, atualizamos com o novo hash
            if (!string.IsNullOrEmpty(usuario.Senha))
            {
                usuarioBuscado.Senha = Criptografia.GerarHash(usuario.Senha);
            }

            _context.Usuario.Update(usuarioBuscado);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Remove um usuário pelo ID.
    /// </summary>
    public async Task DeletarAsync(Guid id)
    {
        var usuarioBuscado = await _context.Usuario.FindAsync(id);
        if (usuarioBuscado != null)
        {
            _context.Usuario.Remove(usuarioBuscado);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Lista todos os usuários com o perfil (TipoUsuario) carregado.
    /// </summary>
    public async Task<List<Usuario>> ListarAsync()
    {
        return await _context.Usuario
            .Include(u => u.IdTipoUsuarioNavigation)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Busca um usuário pelo seu ID.
    /// </summary>
    public async Task<Usuario?> BuscarPorIdAsync(Guid id)
    {
        return await _context.Usuario
            .Include(u => u.IdTipoUsuarioNavigation)
            .FirstOrDefaultAsync(u => u.IdUsuario == id);
    }

    /// <summary>
    /// Busca um usuário pelo e-mail e valida a senha com BCrypt. Usado no processo de Login.
    /// </summary>
    public async Task<Usuario?> BuscarPorEmailESenhaAsync(string email, string senha)
    {
        // 1. Buscamos o usuário pelo e-mail (campo único)
        var usuario = await _context.Usuario
            .Include(u => u.IdTipoUsuarioNavigation)
            .FirstOrDefaultAsync(u => u.Email == email);

        // 2. Se não encontrado, retorna null (usuário não existe)
        if (usuario == null)
        {
            return null;
        }

        // 3. Verificamos se a senha digitada corresponde ao hash salvo no banco
        bool senhaValida = Criptografia.CompararHash(senha, usuario.Senha);

        // 4. Se a senha não confere, retorna null (credenciais inválidas)
        if (!senhaValida)
        {
            return null;
        }

        return usuario;
    }
}
