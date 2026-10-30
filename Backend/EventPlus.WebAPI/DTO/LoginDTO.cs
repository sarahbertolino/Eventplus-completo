using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO;

/// <summary>
/// Data Transfer Object (DTO) utilizado para efetuar o login e autenticação de usuários na API.
/// </summary>
public class LoginDTO
{
    /// <summary>
    /// E-mail do usuário.
    /// </summary>
    [Required(ErrorMessage = "O e-mail é obrigatório para autenticação.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Senha de acesso do usuário.
    /// </summary>
    [Required(ErrorMessage = "A senha é obrigatória para autenticação.")]
    public string Senha { get; set; } = string.Empty;
}
