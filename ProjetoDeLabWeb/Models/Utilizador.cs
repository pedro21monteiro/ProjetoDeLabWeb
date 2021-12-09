using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoDeLabWeb.Models
{
    public partial class Utilizador
    {
        [Key]
        public int IdUtilizador { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }
        //----------não obrigatorios de inicio
        public bool Bloqueado { get; set; } 

        public string Motivo { get; set; }
        public bool RegistoConfirmado { get; set; }

        public string Categoria { get; set; } //Admin Utilizador ou Restaurante

    }
}
