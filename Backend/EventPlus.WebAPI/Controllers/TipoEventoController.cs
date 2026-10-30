using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers;

/// <summary>
/// Controller responsável pelos endpoints de Tipo de Evento (Categorias).
/// Rotas disponíveis: GET, POST, PUT, DELETE.
/// 
/// [Route("api/[controller]")] → Define o prefixo da URL: /api/TipoEvento
/// [ApiController]            → Habilita recursos automáticos: validação de ModelState, binding de parâmetros, etc.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class TipoEventoController : ControllerBase
{
    private readonly ITipoEventoRepository _tipoEventoRepository;

    /// <summary>
    /// Construtor com injeção de dependência da interface do repositório.
    /// </summary>
    public TipoEventoController(ITipoEventoRepository tipoEventoRepository)
    {
        _tipoEventoRepository = tipoEventoRepository;
    }

    /// <summary>
    /// Lista todas as categorias de eventos cadastradas.
    /// GET: /api/TipoEvento
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var tipos = await _tipoEventoRepository.ListarAsync();
        return Ok(tipos);
    }

    /// <summary>
    /// Busca uma categoria de evento específica pelo ID.
    /// GET: /api/TipoEvento/{id}
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> BuscarPorId(Guid id)
    {
        var tipo = await _tipoEventoRepository.BuscarPorIdAsync(id);

        if (tipo == null)
        {
            return NotFound("Tipo de evento não encontrado.");
        }

        return Ok(tipo);
    }

    /// <summary>
    /// Cadastra uma nova categoria de evento.
    /// POST: /api/TipoEvento
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Cadastrar([FromBody] TipoEventoDTO dto)
    {
        // Mapeamos o DTO recebido para a entidade do modelo
        var tipoEvento = new TipoEvento
        {
            Titulo = dto.Titulo
        };

        await _tipoEventoRepository.CadastrarAsync(tipoEvento);

        // Retorna 201 Created com a URL do novo recurso no header Location
        return CreatedAtAction(nameof(BuscarPorId), new { id = tipoEvento.IdTipoEvento }, tipoEvento);
    }

    /// <summary>
    /// Atualiza os dados de uma categoria de evento existente.
    /// PUT: /api/TipoEvento/{id}
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] TipoEventoDTO dto)
    {
        var tipoEvento = new TipoEvento
        {
            Titulo = dto.Titulo
        };

        await _tipoEventoRepository.AtualizarAsync(id, tipoEvento);

        // 204 No Content: operação concluída, sem corpo de resposta
        return NoContent();
    }

    /// <summary>
    /// Remove uma categoria de evento pelo ID.
    /// DELETE: /api/TipoEvento/{id}
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Deletar(Guid id)
    {
        await _tipoEventoRepository.DeletarAsync(id);
        return NoContent();
    }
}
