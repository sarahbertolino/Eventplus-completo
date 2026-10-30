using System;
using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO;

/// <summary>
/// Data Transfer Object (DTO) para criação e atualização de Comentários.
/// 
/// O QUE É UM DTO?
/// DTOs são objetos simples utilizados para transferir dados entre as requisições HTTP e a API.
/// Eles evitam expor as entidades do banco de dados diretamente e aplicam validações automáticas de entrada.
/// </summary>
public class ComentarioDTO
{
    /// <summary>
    /// Conteúdo do comentário digitado pelo usuário.
    /// </summary>
    [Required(ErrorMessage = "O texto do comentário é obrigatório.")]
    [StringLength(200, ErrorMessage = "O comentário deve ter no máximo 200 caracteres.")]
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Identificador único (Guid) do Evento ao qual este comentário pertence.
    /// </summary>
    [Required(ErrorMessage = "O identificador do evento é obrigatório.")]
    public Guid IdEvento { get; set; }

    /// <summary>
    /// Identificador único (Guid) do Usuário autor (Opcional - pode ser anônimo).
    /// </summary>
    public Guid? IdUsuario { get; set; }
}
