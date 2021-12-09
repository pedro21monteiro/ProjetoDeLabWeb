using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ProjetoDeLabWeb.Models
{
   // [Keyless]
    [Table("Preferem")]
    public partial class Preferem
    {
        public int IdUtilizador { get; set; }
        public int IdPratoDoDia { get; set; }

        [ForeignKey(nameof(IdPratoDoDia))]
        public virtual PratoDoDium IdPratoDoDiaNavigation { get; set; }
        [ForeignKey(nameof(IdUtilizador))]
        public virtual Cliente IdUtilizadorNavigation { get; set; }
    }
}
