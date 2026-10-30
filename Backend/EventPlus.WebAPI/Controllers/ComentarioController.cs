using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using EventPlus.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers;

/// <summary>
/// Controller responsável pelos endpoints de Comentários.
/// Demonstra a integração com o IContentModerationService antes de publicar um comentário.
/// 
/// FLUXO DE MODERAÇÃO DE CONTEÚDO:
/// 1. Usuário envia um comentário via POST.
/// 2. A API chama o serviço de moderação para analisar o texto.
/// 3. Se o texto for impróprio, o campo "Exibe" é salvo como false (oculto).
/// 4. Comentários ocultos não aparecem na listagem pública.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ComentarioController : ControllerBase
{
    private readonly IComentarioRepository _comentarioRepository;
    private readonly IContentModerationService _moderacaoService;

    /// <summary>
    /// Construtor com injeção do repositório e do serviço de moderação de conteúdo.
    /// </summary>
    public ComentarioController(IComentarioRepository comentarioRepository, IContentModerationService moderacaoService)
    {
        _comentarioRepository = comentarioRepository;
        _moderacaoService = moderacaoService;
    }

    /// <summary>
    /// Lista todos os comentários com dados do evento e do usuário.
    /// GET: /api/Comentario
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Listar()
    {
        var comentarios = await _comentarioRepository.ListarAsync();
        return Ok(comentarios);
    }

    /// <summary>
    /// Lista somente os comentários aprovados (Exibe = true) de um evento específico.
    /// GET: /api/Comentario/evento/{idEvento}
    /// </summary>
    [HttpGet("evento/{idEvento:guid}")]
    public async Task<IActionResult> ListarPorEvento(Guid idEvento)
    {
        var comentarios = await _comentarioRepository.ListarPorEventoAsync(idEvento);
        return Ok(comentarios);
    }

    /// <summary>
    /// Busca um comentário pelo ID.
    /// GET: /api/Comentario/{id}
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> BuscarPorId(Guid id)
    {
        var comentario = await _comentarioRepository.BuscarPorIdAsync(id);

        if (comentario == null)
        {
            return NotFound("Comentário não encontrado.");
        }

        return Ok(comentario);
    }

    /// <summary>
    /// Cadastra um novo comentário. Passa pelo serviço de moderação de conteúdo antes de ser salvo.
    /// POST: /api/Comentario
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Cadastrar([FromBody] ComentarioDTO dto)
    {
        // 1. Enviamos o texto para o serviço de moderação de conteúdo
        var resultadoModeracao = await _moderacaoService.ModerarTextoAsync(dto.Descricao);

        // 2. Definimos se o comentário deve ser exibido com base na análise
        //    Se o conteúdo for impróprio, o comentário é salvo oculto (Exibe = false)
        bool exibir = resultadoModeracao.EhApropriado;

        var comentario = new Comentario
        {
            Descricao = dto.Descricao,
            IdEvento = dto.IdEvento,
            IdUsuario = dto.IdUsuario,
            DataComentario = DateTime.Now,
            Exibe = exibir
        };

        await _comentarioRepository.CadastrarAsync(comentario);

        // Informamos ao cliente se o comentário foi aprovado ou ficará em moderação
        if (!exibir)
        {
            return Ok(new
            {
                Mensagem = "Comentário enviado, mas aguarda revisão de moderação.",
                Motivo = resultadoModeracao.Motivo
            });
        }

        return CreatedAtAction(nameof(BuscarPorId), new { id = comentario.IdComentario }, comentario);
    }

    /// <summary>
    /// Remove um comentário pelo ID.
    /// DELETE: /api/Comentario/{id}
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Deletar(Guid id)
    {
        await _comentarioRepository.DeletarAsync(id);
        return NoContent();
    }
}
