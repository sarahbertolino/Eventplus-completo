using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EventPlus.WebAPI.Models;

/// <summary>
/// Representa a Categoria / Tipo de um Evento (ex: Palestra, Workshop, Hackathon, Webinar).
/// </summary>
[Table("TipoEvento")]
public partial class TipoEvento
{
    /// <summary>
    /// Chave Primária (PK) do Tipo de Evento (Guid).
    /// </summary>
    [Key]
    public Guid IdTipoEvento { get; set; }

    /// <summary>
    /// Título ou nome da categoria do evento.
    /// </summary>
    [Required(ErrorMessage = "O título da categoria do evento é obrigatório.")]
    [StringLength(100, ErrorMessage = "O título pode ter no máximo 100 caracteres.")]
    [Column("TituloTipoEvento")]
    public string Titulo { get; set; } = null!;

    /// <summary>
    /// Coleção de eventos associados a este tipo/categoria.
    /// </summary>
    [JsonIgnore]
    [InverseProperty("IdTipoEventoNavigation")]
    public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();
}
