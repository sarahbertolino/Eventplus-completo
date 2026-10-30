using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO;

/// <summary>
/// Data Transfer Object (DTO) para cadastro e atualização de Categoria/Tipo de Evento.
/// </summary>
public class TipoEventoDTO
{
    /// <summary>
    /// Título ou nome da categoria (ex: Palestra, Workshop, Hackathon).
    /// </summary>
    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(100, ErrorMessage = "O título pode ter no máximo 100 caracteres.")]
    public string Titulo { get; set; } = string.Empty;
}
