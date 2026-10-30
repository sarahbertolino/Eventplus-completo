using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EventPlus.WebAPI.Models;

/// <summary>
/// Representa a entidade Evento no banco de dados.
/// Armazena as informações dos eventos cadastrados no sistema Event+.
/// </summary>
[Table("Evento")]
public partial class Evento
{
    /// <summary>
    /// Chave Primária (PK) do evento (Guid).
    /// </summary>
    [Key]
    public Guid IdEvento { get; set; }

    /// <summary>
    /// Nome do evento.
    /// </summary>
    [Required(ErrorMessage = "O nome do evento é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome do evento pode ter no máximo 100 caracteres.")]
    [Column("NomeEvento")]
    public string Nome { get; set; } = null!;

    /// <summary>
    /// Descrição detalhada sobre o evento.
    /// </summary>
    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [Column(TypeName = "text")]
    public string Descricao { get; set; } = null!;

    /// <summary>
    /// Data e horário em que o evento será realizado.
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime DataEvento { get; set; }

    /// <summary>
    /// URL da imagem do evento armazenada no Cloudinary.
    /// </summary>
    [StringLength(2048)]
    public string? ImagemUrl { get; set; }

    /// <summary>
    /// Chave Estrangeira (FK) para a categoria de TipoEvento (Anulável - Associação).
    /// </summary>
    public Guid? IdTipoEvento { get; set; }

    /// <summary>
    /// Chave Estrangeira (FK) para a Instituicao organizadora (Anulável - Associação).
    /// </summary>
    public Guid? IdInstituicao { get; set; }

    /// <summary>
    /// Propriedade de navegação para a Instituição promotora do evento.
    /// </summary>
    [ForeignKey("IdInstituicao")]
    [InverseProperty("Eventos")]
    public virtual Instituicao? IdInstituicaoNavigation { get; set; }

    /// <summary>
    /// Propriedade de navegação para a Categoria/Tipo do evento.
    /// </summary>
    [ForeignKey("IdTipoEvento")]
    [InverseProperty("Eventos")]
    public virtual TipoEvento? IdTipoEventoNavigation { get; set; }

    /// <summary>
    /// Coleção de comentários recebidos por este evento (Composição).
    /// </summary>
    [JsonIgnore]
    [InverseProperty("IdEventoNavigation")]
    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    /// <summary>
    /// Coleção de presenças confirmadas para este evento (Composição).
    /// </summary>
    [JsonIgnore]
    [InverseProperty("IdEventoNavigation")]
    public virtual ICollection<Presenca> Presencas { get; set; } = new List<Presenca>();
}
