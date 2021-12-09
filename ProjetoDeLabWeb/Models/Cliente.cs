using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ProjetoDeLabWeb.Models
{
    [Table("Cliente")]
    public partial class Cliente
    {
        [Key]
        public int IdUtilizador { get; set; }
        [Required]
        [StringLength(13)]
        public string Nome { get; set; }

        [ForeignKey(nameof(IdUtilizador))]
        [InverseProperty(nameof(Utilizador.Cliente))]
        public virtual Utilizador IdUtilizadorNavigation { get; set; }
    }
}
