using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers;

/// <summary>
/// Controller responsável pelos endpoints de Presença nos eventos.
/// Permite que usuários se inscrevam e confirmem presença em eventos.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class PresencaController : ControllerBase
{
    private readonly IPresencaRepository _presencaRepository;

    public PresencaController(IPresencaRepository presencaRepository)
    {
        _presencaRepository = presencaRepository;
    }

    /// <summary>
    /// Lista todas as presenças cadastradas.
    /// GET: /api/Presenca
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Listar()
    {
        var presencas = await _presencaRepository.ListarAsync();
        return Ok(presencas);
    }

    /// <summary>
    /// Lista as presenças de um usuário específico.
    /// GET: /api/Presenca/usuario/{idUsuario}
    /// </summary>
    [HttpGet("usuario/{idUsuario:guid}")]
    [Authorize]
    public async Task<IActionResult> ListarMinhasPresencas(Guid idUsuario)
    {
        var presencas = await _presencaRepository.ListarMinhasPresencasAsync(idUsuario);
        return Ok(presencas);
    }

    /// <summary>
    /// Busca uma presença pelo ID.
    /// GET: /api/Presenca/{id}
    /// </summary>
    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> BuscarPorId(Guid id)
    {
        var presenca = await _presencaRepository.BuscarPorIdAsync(id);

        if (presenca == null)
        {
            return NotFound("Presença não encontrada.");
        }

        return Ok(presenca);
    }

    /// <summary>
    /// Inscreve um usuário em um evento (cria o registro de presença).
    /// POST: /api/Presenca
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Inscrever([FromBody] PresencaDTO dto)
    {
        var presenca = new Presenca
        {
            IdEvento = dto.IdEvento,
            IdUsuario = dto.IdUsuario,
            Situacao = dto.Situacao
        };

        await _presencaRepository.InscreverAsync(presenca);

        return CreatedAtAction(nameof(BuscarPorId), new { id = presenca.IdPresenca }, presenca);
    }

    /// <summary>
    /// Atualiza a situação (confirmado/ausente) de uma presença.
    /// PATCH: /api/Presenca/{id}/situacao
    /// </summary>
    [HttpPatch("{id:guid}/situacao")]
    [Authorize]
    public async Task<IActionResult> AtualizarSituacao(Guid id, [FromBody] bool situacao)
    {
        await _presencaRepository.AtualizarSituacaoAsync(id, situacao);
        return NoContent();
    }

    /// <summary>
    /// Remove um registro de presença (cancela inscrição no evento).
    /// DELETE: /api/Presenca/{id}
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Deletar(Guid id)
    {
        await _presencaRepository.DeletarAsync(id);
        return NoContent();
    }
}
