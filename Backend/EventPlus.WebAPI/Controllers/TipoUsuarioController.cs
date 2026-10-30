using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers;

/// <summary>
/// Controller responsável pelos endpoints de Tipo de Usuário (Perfis de Acesso).
/// Rotas disponíveis: GET, POST, PUT, DELETE.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class TipoUsuarioController : ControllerBase
{
    private readonly ITipoUsuarioRepository _tipoUsuarioRepository;

    public TipoUsuarioController(ITipoUsuarioRepository tipoUsuarioRepository)
    {
        _tipoUsuarioRepository = tipoUsuarioRepository;
    }

    /// <summary>
    /// Lista todos os perfis de usuário.
    /// GET: /api/TipoUsuario
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var tipos = await _tipoUsuarioRepository.ListarAsync();
        return Ok(tipos);
    }

    /// <summary>
    /// Busca um perfil de usuário pelo ID.
    /// GET: /api/TipoUsuario/{id}
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> BuscarPorId(Guid id)
    {
        var tipo = await _tipoUsuarioRepository.BuscarPorIdAsync(id);

        if (tipo == null)
        {
            return NotFound("Tipo de usuário não encontrado.");
        }

        return Ok(tipo);
    }

    /// <summary>
    /// Cadastra um novo perfil de usuário.
    /// POST: /api/TipoUsuario
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Cadastrar([FromBody] TipoUsuarioDTO dto)
    {
        var tipoUsuario = new TipoUsuario
        {
            Titulo = dto.Titulo
        };

        await _tipoUsuarioRepository.CadastrarAsync(tipoUsuario);

        return CreatedAtAction(nameof(BuscarPorId), new { id = tipoUsuario.IdTipoUsuario }, tipoUsuario);
    }

    /// <summary>
    /// Atualiza um perfil de usuário existente.
    /// PUT: /api/TipoUsuario/{id}
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] TipoUsuarioDTO dto)
    {
        var tipoUsuario = new TipoUsuario
        {
            Titulo = dto.Titulo
        };

        await _tipoUsuarioRepository.AtualizarAsync(id, tipoUsuario);

        return NoContent();
    }

    /// <summary>
    /// Remove um perfil de usuário pelo ID.
    /// DELETE: /api/TipoUsuario/{id}
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Deletar(Guid id)
    {
        await _tipoUsuarioRepository.DeletarAsync(id);
        return NoContent();
    }
}
