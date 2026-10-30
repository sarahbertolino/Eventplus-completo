using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers;

/// <summary>
/// Controller responsável pelos endpoints de Instituições.
/// Rotas: GET, POST, PUT, DELETE.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class InstituicaoController : ControllerBase
{
    private readonly IInstituicaoRepository _instituicaoRepository;

    public InstituicaoController(IInstituicaoRepository instituicaoRepository)
    {
        _instituicaoRepository = instituicaoRepository;
    }

    /// <summary>
    /// Lista todas as instituições cadastradas.
    /// GET: /api/Instituicao
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var instituicoes = await _instituicaoRepository.ListarAsync();
        return Ok(instituicoes);
    }

    /// <summary>
    /// Busca uma instituição pelo ID.
    /// GET: /api/Instituicao/{id}
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> BuscarPorId(Guid id)
    {
        var instituicao = await _instituicaoRepository.BuscarPorIdAsync(id);

        if (instituicao == null)
        {
            return NotFound("Instituição não encontrada.");
        }

        return Ok(instituicao);
    }

    /// <summary>
    /// Cadastra uma nova instituição.
    /// POST: /api/Instituicao
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Cadastrar([FromBody] InstituicaoDTO dto)
    {
        var instituicao = new Instituicao
        {
            CNPJ = dto.CNPJ,
            NomeFantasia = dto.NomeFantasia,
            Endereco = dto.Endereco
        };

        await _instituicaoRepository.CadastrarAsync(instituicao);

        return CreatedAtAction(nameof(BuscarPorId), new { id = instituicao.IdInstituicao }, instituicao);
    }

    /// <summary>
    /// Atualiza os dados de uma instituição existente.
    /// PUT: /api/Instituicao/{id}
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] InstituicaoDTO dto)
    {
        var instituicao = new Instituicao
        {
            CNPJ = dto.CNPJ,
            NomeFantasia = dto.NomeFantasia,
            Endereco = dto.Endereco
        };

        await _instituicaoRepository.AtualizarAsync(id, instituicao);

        return NoContent();
    }

    /// <summary>
    /// Remove uma instituição pelo ID.
    /// DELETE: /api/Instituicao/{id}
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Deletar(Guid id)
    {
        await _instituicaoRepository.DeletarAsync(id);
        return NoContent();
    }
}
