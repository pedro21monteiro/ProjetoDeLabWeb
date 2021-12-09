using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoDeLabWeb.Models
{
    public class PreferemRestaurante
    {
        [Key]
        public int IdPreferemRestaurante{ get; set; }

        public int UtilizadorId { get; set; }

        public int RestauranteId { get; set; }
    }
}
