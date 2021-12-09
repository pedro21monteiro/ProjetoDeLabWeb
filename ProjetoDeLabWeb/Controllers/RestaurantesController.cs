using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ProjetoDeLabWeb.Data;
using ProjetoDeLabWeb.Models;

namespace ProjetoDeLabWeb.Controllers
{
    public class RestaurantesController : Controller
    {
        private readonly ProjetoDeLabWebContext _context;
        private readonly IHostEnvironment _environment;

        public RestaurantesController(ProjetoDeLabWebContext context, IHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Restaurantes
        public async Task<IActionResult> Index()
        {
            var projetoDeLabWebContext = _context.Restaurante.Include(r => r.UtilizadorDono);
            return View(await projetoDeLabWebContext.ToListAsync());
        }

        // GET: Restaurantes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurante = await _context.Restaurante
                .Include(r => r.UtilizadorDono)
                .FirstOrDefaultAsync(m => m.IdRestaurante == id);
            if (restaurante == null)
            {
                return NotFound();
            }

            return View(restaurante);
        }

        // GET: Restaurantes/Create
        public IActionResult Create()
        {
            ViewData["UtilizadorId"] = new SelectList(_context.Utilizador, "IdUtilizador", "Email");
            return View();
        }

        // POST: Restaurantes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile Foto,[Bind("IdRestaurante,Nome,Telefone,Foto,Morada,Gps,HorarioFunc,DiaDescanco,TipoServico")] Restaurante restaurante)
        {
            if (ModelState.IsValid)
            {
                if(Foto != null)
                {
                    string destination = Path.Combine(_environment.ContentRootPath, "wwwroot/Images/", Path.GetFileName(Foto.FileName));
                    FileStream fs = new FileStream(destination, FileMode.Create);

                    Foto.CopyTo(fs);
                    fs.Close();

                    restaurante.Foto = "Images/" + Path.GetFileName(Foto.FileName);
                }


                Utilizador u = _context.Utilizador.SingleOrDefault(u => u.UserName == HttpContext.Session.GetString("utilizador"));
                restaurante.UtilizadorId = u.IdUtilizador;
                //isto vai trocar para verdadeiro depois de o admin aceitar
               
      
                restaurante.RestauranteAceite = false;
                _context.Add(restaurante);
                await _context.SaveChangesAsync();
                return RedirectToAction("MeusRestaurantes","Restaurantes");
            }
            ViewData["UtilizadorId"] = new SelectList(_context.Utilizador, "IdUtilizador", "Email", restaurante.UtilizadorId);
            return View(restaurante);
        }

        // GET: Restaurantes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurante = await _context.Restaurante.FindAsync(id);
            if (restaurante == null)
            {
                return NotFound();
            }
            ViewData["UtilizadorId"] = new SelectList(_context.Utilizador, "IdUtilizador", "Email", restaurante.UtilizadorId);
            return View(restaurante);
        }

        // POST: Restaurantes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile Foto, [Bind("IdRestaurante,Telefone,Foto,Morada,Gps,HorarioFunc,DiaDescanco,TipoServico,RestauranteAceite,UtilizadorId")] Restaurante restaurante)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (Foto != null)
                    {
                        string destination = Path.Combine(_environment.ContentRootPath, "wwwroot/Images/", Path.GetFileName(Foto.FileName));
                        FileStream fs = new FileStream(destination, FileMode.Create);

                        Foto.CopyTo(fs);
                        fs.Close();

                        restaurante.Foto = "Images/" + Path.GetFileName(Foto.FileName);
                    }
                    _context.Update(restaurante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestauranteExists(restaurante.IdRestaurante))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UtilizadorId"] = new SelectList(_context.Utilizador, "IdUtilizador", "Email", restaurante.UtilizadorId);
            return View(restaurante);
        }

        // GET: Restaurantes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurante = await _context.Restaurante
                .Include(r => r.UtilizadorDono)
                .FirstOrDefaultAsync(m => m.IdRestaurante == id);
            if (restaurante == null)
            {
                return NotFound();
            }

            return View(restaurante);
        }

        // POST: Restaurantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restaurante = await _context.Restaurante.FindAsync(id);
            _context.Restaurante.Remove(restaurante);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestauranteExists(int id)
        {
            return _context.Restaurante.Any(e => e.IdRestaurante == id);
        }


        //---------------------------------------------------------------
        public async Task<IActionResult> MeusRestaurantes()
        {
            var projetoDeLabWebContext = _context.Restaurante.Include(r => r.UtilizadorDono);
            return View(await projetoDeLabWebContext.ToListAsync());
        }

        public async Task<IActionResult> Pedidos()
        {
            var projetoDeLabWebContext = _context.Restaurante.Include(r => r.UtilizadorDono);
            return View(await projetoDeLabWebContext.ToListAsync());
        }

        //Aceitar restaurantes
        //Aceitar restaurante
        public async Task<IActionResult> ClicouAceitarRestaurante(int Id)
        {
            if (ModelState.IsValid)
            {
                Restaurante r = _context.Restaurante.SingleOrDefault(r => r.IdRestaurante == Id);

                if (r == null)
                {
                    ModelState.AddModelError("IdRestaurante", "IdRestaurante incorreto");
                }
                else
                {
                    r.RestauranteAceite = true;
                   
                    _context.Update(r);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Pedidos", "Restaurantes");
                }
            }
            return RedirectToAction("Pedidos", "Restaurantes");

        }


        //View Inicial da lista de restaurantes---------------------------------------

        public async Task<IActionResult> PaginaInicialRestaurantes()
        {
            if (HttpContext.Session.GetString("utilizadorId") != null)
            {
              
                foreach (var item in _context.Restaurante)
                {
                    PreferemRestaurante p = _context.PreferemRestaurante.SingleOrDefault(p=>p.RestauranteId == item.IdRestaurante && p.UtilizadorId == Int32.Parse(HttpContext.Session.GetString("utilizadorId")));

                    if (p != null)
                    {
                        HttpContext.Session.SetString("LigacaoFavorito-" + item.IdRestaurante + "-" + HttpContext.Session.GetString("utilizadorId"), "true");
                    }
                 }
            }
            
            var projetoDeLabWebContext = _context.Restaurante.Include(r => r.UtilizadorDono);
            return View(await projetoDeLabWebContext.ToListAsync());
        }

        //---------------------------------------------------------------------------

        public async Task<IActionResult> ListaDeRestaurantesPreferidos()
        {

            foreach (var item in _context.Restaurante)
            {
                PreferemRestaurante p = _context.PreferemRestaurante.SingleOrDefault(p => p.RestauranteId == item.IdRestaurante && p.UtilizadorId == Int32.Parse(HttpContext.Session.GetString("utilizadorId")));

                if (p != null)
                {
                    HttpContext.Session.SetString("ListaDeRestaurantesFavoritos-" + item.IdRestaurante + "-" + HttpContext.Session.GetString("utilizadorId"), "true");
                }
            }
            var projetoDeLabWebContext = _context.Restaurante.Include(r => r.UtilizadorDono);
            return View(await projetoDeLabWebContext.ToListAsync());
        }

    }
}
