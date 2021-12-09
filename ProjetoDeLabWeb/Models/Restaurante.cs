using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoDeLabWeb.Models
{
    public class Restaurante 
    {
        [Key]
        public int IdRestaurante { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        [RegularExpression(@"^.+\.([pP][nN][gG])$", ErrorMessage = "Only Png Images")]
        public string Foto { get; set; }
        public string Morada { get; set; }
        public string Gps { get; set; }
        public string HorarioFunc { get; set; }
        public string DiaDescanco { get; set; }
        public string TipoServico { get; set; }
        public bool RestauranteAceite { get; set; }
        public bool HorarioCriado { get; set; }


        public bool SegundaFeira_PratoDoDia { get; set; }
        public bool TercaFeira_PratoDoDia { get; set; } 
        public bool QuartaFeira_PratoDoDia { get; set; } 
        public bool QuintaFeira_PratoDoDia { get; set; } 
        public bool SextaFeira_PratoDoDia { get; set; } 
        public bool Sabado_PratoDoDia { get; set; } 
        public bool Domingo_PratoDoDia { get; set; }

        public int NdePreferem { get; set; } = 0;




        public Utilizador UtilizadorDono { get; set; }

        public int UtilizadorId { get; set; }


        ////Só existe no máximo 7 pratos do dia para cada restaurante, um para cada dia
        
       public ICollection<PratoDoDia> PratoDoDia { get; set; }

    }


}
