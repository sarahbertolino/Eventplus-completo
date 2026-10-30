using System;
using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO;

/// <summary>
/// Data Transfer Object (DTO) para registrar ou alterar a presença em um evento.
/// </summary>
public class PresencaDTO
{
    /// <summary>
    /// Situação da presença (true = Confirmada, false = Não confirmada).
    /// </summary>
    public bool Situacao { get; set; }

    /// <summary>
    /// Identificador único do Evento.
    /// </summary>
    [Required(ErrorMessage = "O identificador do evento é obrigatório.")]
    public Guid IdEvento { get; set; }

    /// <summary>
    /// Identificador único do Usuário.
    /// </summary>
    public Guid? IdUsuario { get; set; }
}
