using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjetoDeLabWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoDeLabWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ProjetoDeLabWeb.Models.Administrador> Administrador { get; set; }

        public DbSet<ProjetoDeLabWeb.Models.Cliente> Cliente { get; set; }

        public DbSet<ProjetoDeLabWeb.Models.Possui> Possui { get; set; }

        public DbSet<ProjetoDeLabWeb.Models.PratoDoDium> PratoDoDium { get; set; }

        public DbSet<ProjetoDeLabWeb.Models.Restaurante> Restaurante { get; set; }

        public DbSet<ProjetoDeLabWeb.Models.Utilizador> Utilizador { get; set; }

        public DbSet<ProjetoDeLabWeb.Models.Preferem> Preferem { get; set; }

        public DbSet<ProjetoDeLabWeb.Models.PreferemRestaurante> PreferemRestaurante { get; set; }
    
    }
}
