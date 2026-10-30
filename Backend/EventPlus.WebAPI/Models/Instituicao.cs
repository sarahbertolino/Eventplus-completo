using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Models;

/// <summary>
/// Representa a Instituição promotora de eventos (ex: Escola, Empresa, Faculdade).
/// </summary>
[Table("Instituicao")]
[Index("CNPJ", Name = "UQ__Institui__AA57D6B4", IsUnique = true)]
public partial class Instituicao
{
    /// <summary>
    /// Chave Primária (PK) da instituição (Guid).
    /// </summary>
    [Key]
    public Guid IdInstituicao { get; set; }

    /// <summary>
    /// CNPJ da Instituição (deve conter 14 caracteres e ser único).
    /// </summary>
    [Required(ErrorMessage = "O CNPJ é obrigatório.")]
    [StringLength(14, MinimumLength = 14, ErrorMessage = "O CNPJ deve ter exatamente 14 caracteres numéricos.")]
    public string CNPJ { get; set; } = null!;

    /// <summary>
    /// Nome Fantasia da Instituição.
    /// </summary>
    [Required(ErrorMessage = "O Nome Fantasia é obrigatório.")]
    [StringLength(100, ErrorMessage = "O Nome Fantasia pode ter no máximo 100 caracteres.")]
    public string NomeFantasia { get; set; } = null!;

    /// <summary>
    /// Endereço físico completo da Instituição.
    /// </summary>
    [Required(ErrorMessage = "O endereço é obrigatório.")]
    [StringLength(100, ErrorMessage = "O endereço pode ter no máximo 100 caracteres.")]
    public string Endereco { get; set; } = null!;

    /// <summary>
    /// Coleção de eventos promovidos por esta instituição.
    /// </summary>
    [JsonIgnore]
    [InverseProperty("IdInstituicaoNavigation")]
    public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();
}
