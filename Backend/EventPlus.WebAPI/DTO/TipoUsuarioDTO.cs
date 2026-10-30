using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO;

/// <summary>
/// Data Transfer Object (DTO) para cadastro e atualização do Perfil/Tipo de Usuário.
/// </summary>
public class TipoUsuarioDTO
{
    /// <summary>
    /// Título do tipo de usuário (ex: Administrador, Aluno, Professor).
    /// </summary>
    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(100, ErrorMessage = "O título pode ter no máximo 100 caracteres.")]
    public string Titulo { get; set; } = string.Empty;
}
