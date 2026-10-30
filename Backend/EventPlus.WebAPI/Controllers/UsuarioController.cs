using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers;

/// <summary>
/// Controller responsável pelo gerenciamento de Usuários.
/// Expõe endpoints para cadastro, listagem, atualização e remoção de usuários.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioController(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    /// <summary>
    /// Lista todos os usuários com seus perfis de acesso.
    /// GET: /api/Usuario
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Listar()
    {
        var usuarios = await _usuarioRepository.ListarAsync();
        return Ok(usuarios);
    }

    /// <summary>
    /// Busca um usuário pelo ID.
    /// GET: /api/Usuario/{id}
    /// </summary>
    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> BuscarPorId(Guid id)
    {
        var usuario = await _usuarioRepository.BuscarPorIdAsync(id);

        if (usuario == null)
        {
            return NotFound("Usuário não encontrado.");
        }

        return Ok(usuario);
    }

    /// <summary>
    /// Cadastra um novo usuário no sistema. A senha é criptografada automaticamente com BCrypt.
    /// POST: /api/Usuario
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Cadastrar([FromBody] UsuarioDTO dto)
    {
        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Senha = dto.Senha, // A criptografia ocorre dentro do repositório
            IdTipoUsuario = dto.IdTipoUsuario
        };

        await _usuarioRepository.CadastrarAsync(usuario);

        return CreatedAtAction(nameof(BuscarPorId), new { id = usuario.IdUsuario }, new
        {
            usuario.IdUsuario,
            usuario.Nome,
            usuario.Email,
            usuario.IdTipoUsuario
        });
    }

    /// <summary>
    /// Atualiza os dados de um usuário existente.
    /// PUT: /api/Usuario/{id}
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] UsuarioDTO dto)
    {
        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Senha = dto.Senha,
            IdTipoUsuario = dto.IdTipoUsuario
        };

        await _usuarioRepository.AtualizarAsync(id, usuario);

        return NoContent();
    }

    /// <summary>
    /// Remove um usuário pelo ID.
    /// DELETE: /api/Usuario/{id}
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Deletar(Guid id)
    {
        await _usuarioRepository.DeletarAsync(id);
        return NoContent();
    }
}
