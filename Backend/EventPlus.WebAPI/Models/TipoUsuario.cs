using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EventPlus.WebAPI.Models;

/// <summary>
/// Representa o perfil de acesso do Usuário no sistema (ex: Administrador, Aluno, Professor).
/// </summary>
[Table("TipoUsuario")]
public partial class TipoUsuario
{
    /// <summary>
    /// Chave Primária (PK) do TipoUsuario (Guid).
    /// </summary>
    [Key]
    public Guid IdTipoUsuario { get; set; }

    /// <summary>
    /// Título do tipo de usuário (ex: "Administrador", "Aluno").
    /// </summary>
    [Required(ErrorMessage = "O título do tipo de usuário é obrigatório.")]
    [StringLength(100, ErrorMessage = "O título pode ter no máximo 100 caracteres.")]
    [Column("TituloTipoUsuario")]
    public string Titulo { get; set; } = null!;

    /// <summary>
    /// Coleção de usuários vinculados a este perfil.
    /// </summary>
    [JsonIgnore]
    [InverseProperty("IdTipoUsuarioNavigation")]
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
