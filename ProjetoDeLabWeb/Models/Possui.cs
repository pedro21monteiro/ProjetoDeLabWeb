using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ProjetoDeLabWeb.Models
{
    [Table("Possui")]
    public partial class Possui
    {
        [Key]
        [Column(TypeName = "date")]
        public DateTime DataPossui { get; set; }
        [Column(TypeName = "money")]
        public decimal? Preco { get; set; }
        public int Idutilizador { get; set; }
        public int IdPratoDoDia { get; set; }

        [ForeignKey(nameof(IdPratoDoDia))]
        [InverseProperty(nameof(PratoDoDium.Possuis))]
        public virtual PratoDoDium IdPratoDoDiaNavigation { get; set; }
        [ForeignKey(nameof(Idutilizador))]
        [InverseProperty(nameof(Restaurante.Possuis))]
        public virtual Restaurante IdutilizadorNavigation { get; set; }
    }
}
