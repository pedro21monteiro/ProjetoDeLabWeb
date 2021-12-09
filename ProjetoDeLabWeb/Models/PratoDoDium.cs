using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ProjetoDeLabWeb.Models
{
    public partial class PratoDoDium
    {
        public PratoDoDium()
        {
            Possuis = new HashSet<Possui>();
        }

        [Key]
        public int IdPratoDoDia { get; set; }
        [Required]
        [StringLength(100)]
        public string Descricao { get; set; }
        [StringLength(100)]
        public string Foto { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DataPrato { get; set; }
        [Required]
        [StringLength(30)]
        public string Tipo { get; set; }

        [InverseProperty(nameof(Possui.IdPratoDoDiaNavigation))]
        public virtual ICollection<Possui> Possuis { get; set; }
    }
}
