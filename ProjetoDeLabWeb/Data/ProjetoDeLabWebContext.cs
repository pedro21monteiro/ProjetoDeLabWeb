using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjetoDeLabWeb.Models;

namespace ProjetoDeLabWeb.Data
{
    public class ProjetoDeLabWebContext : DbContext
    {
        public ProjetoDeLabWebContext (DbContextOptions<ProjetoDeLabWebContext> options)
            : base(options)
        {
        }

        public DbSet<ProjetoDeLabWeb.Models.Utilizador> Utilizador { get; set; }

        public DbSet<ProjetoDeLabWeb.Models.Restaurante> Restaurante { get; set; }

        public DbSet<ProjetoDeLabWeb.Models.PratoDoDia> PratoDoDia { get; set; }

        public DbSet<ProjetoDeLabWeb.Models.PreferemRestaurante> PreferemRestaurante { get; set; }


    }
}
