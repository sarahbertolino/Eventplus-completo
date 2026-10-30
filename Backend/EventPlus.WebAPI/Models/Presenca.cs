using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventPlus.WebAPI.Models;

/// <summary>
/// Representa a confirmação de presença de um usuário em um evento.
/// 
/// RELACIONAMENTOS (DER / UML):
/// 1. Evento (Composição): A presença pertence a um evento específico (IdEvento é OBRIGATÓRIA).
/// 2. Usuario (Associação): A presença se refere a um usuário (IdUsuario é ANULÁVEL).
/// </summary>
[Table("Presenca")]
public partial class Presenca
{
    /// <summary>
    /// Chave Primária (PK) do registro de presença.
    /// </summary>
    [Key]
    public Guid IdPresenca { get; set; }

    /// <summary>
    /// Indica a situação da presença (true = Confirmada, false = Não confirmada/Ausente).
    /// </summary>
    public bool Situacao { get; set; }

    /// <summary>
    /// Chave Estrangeira (FK) do Evento. (Obrigatória - Composição).
    /// </summary>
    public Guid IdEvento { get; set; }

    /// <summary>
    /// Chave Estrangeira (FK) do Usuário. (Anulável - Associação).
    /// </summary>
    public Guid? IdUsuario { get; set; }

    /// <summary>
    /// Propriedade de navegação para o Evento.
    /// </summary>
    [ForeignKey("IdEvento")]
    [InverseProperty("Presencas")]
    public virtual Evento? IdEventoNavigation { get; set; }

    /// <summary>
    /// Propriedade de navegação para o Usuário.
    /// </summary>
    [ForeignKey("IdUsuario")]
    [InverseProperty("Presencas")]
    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
