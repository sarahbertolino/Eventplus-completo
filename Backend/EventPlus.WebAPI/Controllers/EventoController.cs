using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using EventPlus.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers;

/// <summary>
/// Controller responsável pelos endpoints de Eventos.
/// Este controller demonstra o uso do serviço de imagem em nuvem (IImageStorageService).
/// 
/// COMO FUNCIONA O UPLOAD DE IMAGEM:
/// O cliente pode enviar o arquivo de imagem diretamente via formulário multipart/form-data.
/// A API recebe o arquivo, envia para a nuvem através do IImageStorageService e salva 
/// apenas a URL retornada na coluna ImagemUrl do banco de dados.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class EventoController : ControllerBase
{
    private readonly IEventoRepository _eventoRepository;
    private readonly IImageStorageService _imageStorageService;

    /// <summary>
    /// Construtor com injeção de dependências do repositório de eventos e do serviço de imagem.
    /// </summary>
    public EventoController(IEventoRepository eventoRepository, IImageStorageService imageStorageService)
    {
        _eventoRepository = eventoRepository;
        _imageStorageService = imageStorageService;
    }

    /// <summary>
    /// Lista todos os eventos cadastrados com dados de Tipo e Instituição.
    /// GET: /api/Evento
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var eventos = await _eventoRepository.ListarAsync();
        return Ok(eventos);
    }

    /// <summary>
    /// Lista somente os eventos com data futura (próximos eventos).
    /// GET: /api/Evento/proximos
    /// </summary>
    [HttpGet("proximos")]
    public async Task<IActionResult> ListarProximos()
    {
        var eventos = await _eventoRepository.ListarProximosAsync();
        return Ok(eventos);
    }

    /// <summary>
    /// Busca um evento específico pelo ID.
    /// GET: /api/Evento/{id}
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> BuscarPorId(Guid id)
    {
        var evento = await _eventoRepository.BuscarPorIdAsync(id);

        if (evento == null)
        {
            return NotFound("Evento não encontrado.");
        }

        return Ok(evento);
    }

    /// <summary>
    /// Cadastra um novo evento. Aceita envio de imagem via multipart/form-data.
    /// POST: /api/Evento
    /// </summary>
    [HttpPost]
    [Authorize]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Cadastrar([FromForm] EventoDTO dto)
    {
        var evento = new Evento
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            DataEvento = dto.DataEvento,
            IdTipoEvento = dto.IdTipoEvento,
            IdInstituicao = dto.IdInstituicao
        };

        // 1. Verifica se um arquivo de imagem foi enviado no formulário
        if (dto.ArquivoImagem != null && dto.ArquivoImagem.Length > 0)
        {
            // 2. Envia a imagem para o serviço de nuvem e recebe a URL pública
            string urlImagem = await _imageStorageService.UploadImageAsync(dto.ArquivoImagem);

            // 3. Salva apenas a URL no campo ImagemUrl do evento
            evento.ImagemUrl = urlImagem;
        }
        else if (!string.IsNullOrEmpty(dto.ImagemUrl))
        {
            // Permite passar diretamente uma URL de imagem já existente na nuvem
            evento.ImagemUrl = dto.ImagemUrl;
        }

        await _eventoRepository.CadastrarAsync(evento);

        return CreatedAtAction(nameof(BuscarPorId), new { id = evento.IdEvento }, evento);
    }

    /// <summary>
    /// Atualiza os dados de um evento. Aceita nova imagem via multipart/form-data.
    /// PUT: /api/Evento/{id}
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Atualizar(Guid id, [FromForm] EventoDTO dto)
    {
        var evento = new Evento
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            DataEvento = dto.DataEvento,
            IdTipoEvento = dto.IdTipoEvento,
            IdInstituicao = dto.IdInstituicao
        };

        // Se uma nova imagem foi enviada, fazemos o upload e atualizamos a URL
        if (dto.ArquivoImagem != null && dto.ArquivoImagem.Length > 0)
        {
            string urlImagem = await _imageStorageService.UploadImageAsync(dto.ArquivoImagem);
            evento.ImagemUrl = urlImagem;
        }
        else if (!string.IsNullOrEmpty(dto.ImagemUrl))
        {
            evento.ImagemUrl = dto.ImagemUrl;
        }

        await _eventoRepository.AtualizarAsync(id, evento);

        return NoContent();
    }

    /// <summary>
    /// Remove um evento pelo ID (os comentários e presenças vinculados são excluídos em cascata).
    /// DELETE: /api/Evento/{id}
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Deletar(Guid id)
    {
        await _eventoRepository.DeletarAsync(id);
        return NoContent();
    }
}
