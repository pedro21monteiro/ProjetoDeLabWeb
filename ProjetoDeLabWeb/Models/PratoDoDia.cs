using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoDeLabWeb.Models
{
    public class PratoDoDia
    {
        [Key]
        public int IdPratoDoDia { get; set; }

        public string Descricao { get; set; }

        public string Foto { get; set; }

        public string DiaDaSemana { get; set; }

        public DateTime DataPrato { get; set; }

        public string Tipo { get; set; }

        public bool Folga { get; set; }

        // esta inativo
        public int NdePreferemPratoDoDia { get; set; } = 0;

        public Restaurante restaurante { get; set; }

        public int RestauranteId { get; set; }
    }
}
