using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Models;

/// <summary>
/// Representa a entidade Usuario no banco de dados.
/// Armazena as informações dos usuários cadastrados (Alunos, Administradores, etc.).
/// </summary>
[Table("Usuario")]
[Index("Email", Name = "UQ__Usuario__Email", IsUnique = true)]
public partial class Usuario
{
    /// <summary>
    /// Chave Primária (PK) do usuário (Guid).
    /// </summary>
    [Key]
    public Guid IdUsuario { get; set; }

    /// <summary>
    /// Nome completo do usuário.
    /// </summary>
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome pode ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = null!;

    /// <summary>
    /// E-mail do usuário (Único no sistema).
    /// </summary>
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [StringLength(256, ErrorMessage = "O e-mail pode ter no máximo 256 caracteres.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail em formato válido.")]
    public string Email { get; set; } = null!;

    /// <summary>
    /// Senha criptografada do usuário.
    /// </summary>
    [Required(ErrorMessage = "A senha é obrigatória.")]
    [StringLength(60, ErrorMessage = "A senha deve ter no máximo 60 caracteres.")]
    public string Senha { get; set; } = null!;

    /// <summary>
    /// Chave Estrangeira (FK) do TipoUsuario ao qual este usuário pertence (Anulável - Associação).
    /// </summary>
    public Guid? IdTipoUsuario { get; set; }

    /// <summary>
    /// Propriedade de navegação para o perfil/tipo do usuário.
    /// </summary>
    [ForeignKey("IdTipoUsuario")]
    [InverseProperty("Usuarios")]
    public virtual TipoUsuario? IdTipoUsuarioNavigation { get; set; }

    /// <summary>
    /// Coleção de comentários escritos por este usuário.
    /// </summary>
    [JsonIgnore]
    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    /// <summary>
    /// Coleção de confirmações de presença feitas por este usuário.
    /// </summary>
    [JsonIgnore]
    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Presenca> Presencas { get; set; } = new List<Presenca>();
}
