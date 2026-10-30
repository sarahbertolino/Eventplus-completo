using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EventPlus.WebAPI.Models;

/// <summary>
/// Representa a entidade Comentario no banco de dados.
/// Guarda os comentários e feedback deixados pelos usuários sobre um evento.
/// 
/// RELACIONAMENTOS (REGRAS DE NEGÓCIO DO DER / UML):
/// 1. Evento (Composição): O comentário PERTENCE a um evento. Se o Evento for apagado, 
///    seus comentários são excluídos em cascata (FK IdEvento é OBRIGATÓRIA).
/// 2. Usuario (Associação): O comentário foi escrito por um usuário, mas se o Usuário for 
///    removido, o comentário pode continuar existindo de forma anônima (FK IdUsuario é ANULÁVEL).
/// </summary>
[Table("Comentario")]
public partial class Comentario
{
    /// <summary>
    /// Chave Primária (PK) do comentário. Identificador único global (Guid).
    /// </summary>
    [Key]
    public Guid IdComentario { get; set; }

    /// <summary>
    /// Conteúdo do comentário digitado pelo usuário.
    /// </summary>
    [Required(ErrorMessage = "A descrição do comentário é obrigatória.")]
    [StringLength(200, ErrorMessage = "O comentário pode ter no máximo 200 caracteres.")]
    public string Descricao { get; set; } = null!;

    /// <summary>
    /// Data e hora em que o comentário foi registrado.
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime DataComentario { get; set; }

    /// <summary>
    /// Status de exibição do comentário (true = exibido, false = oculto pela moderação).
    /// </summary>
    public bool Exibe { get; set; }

    /// <summary>
    /// Chave Estrangeira (FK) do Evento associado. (Obrigatória - Composição).
    /// </summary>
    public Guid IdEvento { get; set; }

    /// <summary>
    /// Chave Estrangeira (FK) do Usuário autor. (Anulável - Associação).
    /// </summary>
    public Guid? IdUsuario { get; set; }

    /// <summary>
    /// Propriedade de navegação para o Evento ao qual este comentário pertence.
    /// </summary>
    [ForeignKey("IdEvento")]
    [InverseProperty("Comentarios")]
    public virtual Evento? IdEventoNavigation { get; set; }

    /// <summary>
    /// Propriedade de navegação para o Usuário que escreveu o comentário.
    /// </summary>
    [ForeignKey("IdUsuario")]
    [InverseProperty("Comentarios")]
    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
