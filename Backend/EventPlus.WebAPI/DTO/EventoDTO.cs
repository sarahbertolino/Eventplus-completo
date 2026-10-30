using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO;

/// <summary>
/// Data Transfer Object (DTO) para criação e atualização de Eventos.
/// </summary>
public class EventoDTO
{
    /// <summary>
    /// Nome do evento.
    /// </summary>
    [Required(ErrorMessage = "O nome do evento é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome pode ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Descrição completa do evento.
    /// </summary>
    [Required(ErrorMessage = "A descrição do evento é obrigatória.")]
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Data e horário em que o evento será realizado.
    /// </summary>
    [Required(ErrorMessage = "A data do evento é obrigatória.")]
    public DateTime DataEvento { get; set; }

    /// <summary>
    /// URL direta da imagem (caso já esteja hospedada na nuvem).
    /// </summary>
    [StringLength(2048, ErrorMessage = "A URL da imagem pode ter no máximo 2048 caracteres.")]
    public string? ImagemUrl { get; set; }

    /// <summary>
    /// Arquivo de imagem enviado via upload (FormData) para envio automático à nuvem.
    /// </summary>
    public IFormFile? ArquivoImagem { get; set; }

    /// <summary>
    /// Identificador único da categoria/tipo do evento.
    /// </summary>
    public Guid? IdTipoEvento { get; set; }

    /// <summary>
    /// Identificador único da instituição organizadora.
    /// </summary>
    public Guid? IdInstituicao { get; set; }
}
