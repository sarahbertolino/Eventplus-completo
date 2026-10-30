using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO;

/// <summary>
/// Data Transfer Object (DTO) para cadastro e atualização de Instituições.
/// </summary>
public class InstituicaoDTO
{
    /// <summary>
    /// CNPJ da instituição (14 números).
    /// </summary>
    [Required(ErrorMessage = "O CNPJ é obrigatório.")]
    [StringLength(14, MinimumLength = 14, ErrorMessage = "O CNPJ deve ter 14 caracteres.")]
    public string CNPJ { get; set; } = string.Empty;

    /// <summary>
    /// Nome Fantasia da instituição.
    /// </summary>
    [Required(ErrorMessage = "O Nome Fantasia é obrigatório.")]
    [StringLength(100, ErrorMessage = "O Nome Fantasia pode ter no máximo 100 caracteres.")]
    public string NomeFantasia { get; set; } = string.Empty;

    /// <summary>
    /// Endereço físico completo da instituição.
    /// </summary>
    [Required(ErrorMessage = "O endereço é obrigatório.")]
    [StringLength(100, ErrorMessage = "O endereço pode ter no máximo 100 caracteres.")]
    public string Endereco { get; set; } = string.Empty;
}
