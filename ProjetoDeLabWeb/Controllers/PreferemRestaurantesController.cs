using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetoDeLabWeb.Data;
using ProjetoDeLabWeb.Models;

namespace ProjetoDeLabWeb.Controllers
{
    public class PreferemRestaurantesController : Controller
    {
        private readonly ProjetoDeLabWebContext _context;

        public PreferemRestaurantesController(ProjetoDeLabWebContext context)
        {
            _context = context;
        }

        // GET: PreferemRestaurantes
        public async Task<IActionResult> Index()
        {
            return View(await _context.PreferemRestaurante.ToListAsync());
        }

        // GET: PreferemRestaurantes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var preferemRestaurante = await _context.PreferemRestaurante
                .FirstOrDefaultAsync(m => m.IdPreferemRestaurante == id);
            if (preferemRestaurante == null)
            {
                return NotFound();
            }

            return View(preferemRestaurante);
        }

        // GET: PreferemRestaurantes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PreferemRestaurantes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPreferemRestaurante,UtilizadorId,RestauranteId")] PreferemRestaurante preferemRestaurante)
        {
            if (ModelState.IsValid)
            {
                _context.Add(preferemRestaurante);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(preferemRestaurante);
        }

        // GET: PreferemRestaurantes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var preferemRestaurante = await _context.PreferemRestaurante.FindAsync(id);
            if (preferemRestaurante == null)
            {
                return NotFound();
            }
            return View(preferemRestaurante);
        }

        // POST: PreferemRestaurantes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPreferemRestaurante,UtilizadorId,RestauranteId")] PreferemRestaurante preferemRestaurante)
        {
            if (id != preferemRestaurante.IdPreferemRestaurante)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(preferemRestaurante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PreferemRestauranteExists(preferemRestaurante.IdPreferemRestaurante))
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
            return View(preferemRestaurante);
        }

        // GET: PreferemRestaurantes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var preferemRestaurante = await _context.PreferemRestaurante
                .FirstOrDefaultAsync(m => m.IdPreferemRestaurante == id);
            if (preferemRestaurante == null)
            {
                return NotFound();
            }

            return View(preferemRestaurante);
        }

        // POST: PreferemRestaurantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var preferemRestaurante = await _context.PreferemRestaurante.FindAsync(id);
            _context.PreferemRestaurante.Remove(preferemRestaurante);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PreferemRestauranteExists(int id)
        {
            return _context.PreferemRestaurante.Any(e => e.IdPreferemRestaurante == id);
        }

        public IActionResult ErroPrecisaEstarLogado()
        {
            return View();
        }
        //este id de entrada é o RestauranteId do PreferemRestaurantes
        public async Task<IActionResult> MeterRestauranteFavorito(int id)
        {

            PreferemRestaurante p = _context.PreferemRestaurante.FirstOrDefault(p => p.UtilizadorId == Int32.Parse(HttpContext.Session.GetString("utilizadorId")) && p.RestauranteId == id);
            if (p == null)
            {
                PreferemRestaurante CriarPreferemRestaurante = new PreferemRestaurante();

                CriarPreferemRestaurante.RestauranteId = id;
                CriarPreferemRestaurante.UtilizadorId = Int32.Parse(HttpContext.Session.GetString("utilizadorId"));

                _context.Add(CriarPreferemRestaurante);
                await _context.SaveChangesAsync();

                return RedirectToAction("PaginaInicialRestaurantes", "Restaurantes");
            }
            else
            {
                ModelState.AddModelError("JaEstaPreferido", "Já tem este restaurante como preferido");
            }
            return RedirectToAction("PaginaInicialRestaurantes", "Restaurantes");
        }

        public async Task<IActionResult> RemoverRestauranteFavorito(int id)
        {

            PreferemRestaurante p = _context.PreferemRestaurante.FirstOrDefault(p => p.UtilizadorId == Int32.Parse(HttpContext.Session.GetString("utilizadorId")) && p.RestauranteId == id);
            if (p != null)
            {            
                _context.Remove(p);
                await _context.SaveChangesAsync();
  
                HttpContext.Session.SetString("LigacaoFavorito-" + p.RestauranteId + "-" + HttpContext.Session.GetString("utilizadorId"), "false");
                HttpContext.Session.SetString("ListaDeRestaurantesFavoritos-" + p.RestauranteId + "-" + HttpContext.Session.GetString("utilizadorId"), "false");

                return RedirectToAction("PaginaInicialRestaurantes", "Restaurantes");
            }
            else
            {
                ModelState.AddModelError("JaEstaRemovido", "Já tem este restaurante como Removido dos preferidos");
            }
            return RedirectToAction("PaginaInicialRestaurantes", "Restaurantes");
        }

        public async Task<IActionResult> RemoverRestauranteFavoritoDalistaDeRestaurantesFavoritos(int id)
        {

            PreferemRestaurante p = _context.PreferemRestaurante.FirstOrDefault(p => p.UtilizadorId == Int32.Parse(HttpContext.Session.GetString("utilizadorId")) && p.RestauranteId == id);
            if (p != null)
            {
                _context.Remove(p);
                await _context.SaveChangesAsync();

                HttpContext.Session.SetString("LigacaoFavorito-" + p.RestauranteId + "-" + HttpContext.Session.GetString("utilizadorId"), "false");

                HttpContext.Session.SetString("ListaDeRestaurantesFavoritos-" + p.RestauranteId + "-" + HttpContext.Session.GetString("utilizadorId"), "false");

                return RedirectToAction("ListaDeRestaurantesPreferidos", "Restaurantes");
            }
            else
            {
                ModelState.AddModelError("JaEstaRemovido", "Já tem este restaurante como Removido dos preferidos");
            }
            return RedirectToAction("ListaDeRestaurantesPreferidos", "Restaurantes");
        }

    }
}
