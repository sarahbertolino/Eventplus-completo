using System;
using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO;

/// <summary>
/// Data Transfer Object (DTO) para cadastro e atualização de Usuários.
/// </summary>
public class UsuarioDTO
{
    /// <summary>
    /// Nome completo do usuário.
    /// </summary>
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome pode ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// E-mail de acesso do usuário.
    /// </summary>
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Senha de acesso do usuário (será criptografada com BCrypt antes de salvar).
    /// </summary>
    [Required(ErrorMessage = "A senha é obrigatória.")]
    [StringLength(60, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 60 caracteres.")]
    public string Senha { get; set; } = string.Empty;

    /// <summary>
    /// Identificador do perfil/tipo do usuário.
    /// </summary>
    public Guid? IdTipoUsuario { get; set; }
}
