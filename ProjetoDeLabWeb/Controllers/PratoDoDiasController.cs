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
    public class PratoDoDiasController : Controller
    {
        private readonly ProjetoDeLabWebContext _context;
        private readonly IHostEnvironment _environment;

        public PratoDoDiasController(ProjetoDeLabWebContext context, IHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: PratoDoDias
        public async Task<IActionResult> Index()
        {
            var projetoDeLabWebContext = _context.PratoDoDia.Include(p => p.restaurante);
            return View(await projetoDeLabWebContext.ToListAsync());
        }

        // GET: PratoDoDias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pratoDoDia = await _context.PratoDoDia
                .Include(p => p.restaurante)
                .FirstOrDefaultAsync(m => m.IdPratoDoDia == id);

            HttpContext.Session.SetString("DiaDaSemanaPratoDoDia_Details", pratoDoDia.DiaDaSemana);
            if (pratoDoDia == null)
            {
                return NotFound();
            }

            return View(pratoDoDia);
        }

        // GET: PratoDoDias/Create
        public IActionResult Create()
        {
            ViewData["RestauranteId"] = new SelectList(_context.Restaurante, "IdRestaurante", "IdRestaurante");
            return View();
        }

        // POST: PratoDoDias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPratoDoDia,Descricao,Foto,DiaDaSemana,DataPrato,Tipo,RestauranteId")] PratoDoDia pratoDoDia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pratoDoDia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RestauranteId"] = new SelectList(_context.Restaurante, "IdRestaurante", "IdRestaurante", pratoDoDia.RestauranteId);
            return View(pratoDoDia);
        }

        // GET: PratoDoDias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pratoDoDia = await _context.PratoDoDia.FindAsync(id);
            if (pratoDoDia == null)
            {
                return NotFound();
            }
            ViewData["RestauranteId"] = new SelectList(_context.Restaurante, "IdRestaurante", "IdRestaurante", pratoDoDia.RestauranteId);
            return View(pratoDoDia);
        }

        // POST: PratoDoDias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPratoDoDia,Descricao,Foto,DiaDaSemana,DataPrato,Tipo,RestauranteId")] PratoDoDia pratoDoDia)
        {
            if (id != pratoDoDia.IdPratoDoDia)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pratoDoDia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PratoDoDiaExists(pratoDoDia.IdPratoDoDia))
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
            ViewData["RestauranteId"] = new SelectList(_context.Restaurante, "IdRestaurante", "IdRestaurante", pratoDoDia.RestauranteId);
            return View(pratoDoDia);
        }

        // GET: PratoDoDias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pratoDoDia = await _context.PratoDoDia
                .Include(p => p.restaurante)
                .FirstOrDefaultAsync(m => m.IdPratoDoDia == id);
            if (pratoDoDia == null)
            {
                return NotFound();
            }

            return View(pratoDoDia);
        }

        // POST: PratoDoDias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pratoDoDia = await _context.PratoDoDia.FindAsync(id);
            _context.PratoDoDia.Remove(pratoDoDia);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PratoDoDiaExists(int id)
        {
            return _context.PratoDoDia.Any(e => e.IdPratoDoDia == id);
        }

        //gerir o horario do restaurante

        public async Task<IActionResult> GerirHorarioRestaurante()
        {
            var projetoDeLabWebContext = _context.PratoDoDia.Include(p => p.restaurante);
            return View(await projetoDeLabWebContext.ToListAsync());
        }

        public async Task<IActionResult> ClicouGerirHorarioRestaurante(int Id)
        {
           
            //verificar se clicou Gerir horario pela primeira vez, caso seja verdade vai criar 7 Pratos do dia
            //para aquele restaurante, um para cada dia
            //----
            if (ModelState.IsValid)
            {
                Restaurante r = _context.Restaurante.SingleOrDefault(r => r.IdRestaurante == Id);
                HttpContext.Session.SetString("GerirHorarioRestauranteId", Id.ToString());
                PratoDoDia p = _context.PratoDoDia.SingleOrDefault(p => p.RestauranteId == Id && p.DiaDaSemana == "SegundaFeira");
                if (p != null)
                {
                    HttpContext.Session.SetString("IdPratoDoDia_SegundaFeira", (p.IdPratoDoDia).ToString());
                    HttpContext.Session.SetString("IdPratoDoDia_TercaFeira", (p.IdPratoDoDia + 1).ToString());
                    HttpContext.Session.SetString("IdPratoDoDia_QuartaFeira", (p.IdPratoDoDia + 2).ToString());
                    HttpContext.Session.SetString("IdPratoDoDia_QuintaFeira", (p.IdPratoDoDia + 3).ToString());
                    HttpContext.Session.SetString("IdPratoDoDia_SextaFeira", (p.IdPratoDoDia + 4).ToString());
                    HttpContext.Session.SetString("IdPratoDoDia_Sabado", (p.IdPratoDoDia + 5).ToString());
                    HttpContext.Session.SetString("IdPratoDoDia_Domingo", (p.IdPratoDoDia + 6).ToString());
                }
                

                if (r.HorarioCriado == false)
                {
                    PratoDoDia SegundaFeira = new PratoDoDia();
                    PratoDoDia TercaFeira = new PratoDoDia();
                    PratoDoDia QuartaFeira = new PratoDoDia();
                    PratoDoDia QuintaFeira = new PratoDoDia();
                    PratoDoDia SextaFeira = new PratoDoDia();
                    PratoDoDia Sabado = new PratoDoDia();
                    PratoDoDia Domingo = new PratoDoDia();

                    //---
                    SegundaFeira.RestauranteId = r.IdRestaurante;
                    SegundaFeira.DiaDaSemana = "SegundaFeira";

                    TercaFeira.RestauranteId = r.IdRestaurante;
                    TercaFeira.DiaDaSemana = "TercaFeira";

                    QuartaFeira.RestauranteId = r.IdRestaurante;
                    QuartaFeira.DiaDaSemana = "QuartaFeira";

                    QuintaFeira.RestauranteId = r.IdRestaurante;
                    QuintaFeira.DiaDaSemana = "QuintaFeira";

                    SextaFeira.RestauranteId = r.IdRestaurante;
                    SextaFeira.DiaDaSemana = "SextaFeira";

                    Sabado.RestauranteId = r.IdRestaurante;
                    Sabado.DiaDaSemana = "Sabado";

                    Domingo.RestauranteId = r.IdRestaurante;
                    Domingo.DiaDaSemana = "Domingo";

                    _context.Add(SegundaFeira);
                    _context.Add(TercaFeira);
                    _context.Add(QuartaFeira);
                    _context.Add(QuintaFeira);
                    _context.Add(SextaFeira);
                    _context.Add(Sabado);
                    _context.Add(Domingo);

                    
                    r.HorarioCriado = true;
                    _context.Update(r);
                    _context.SaveChanges();

                    HttpContext.Session.SetString("IdPratoDoDia_SegundaFeira", SegundaFeira.IdPratoDoDia.ToString());
                    HttpContext.Session.SetString("IdPratoDoDia_TercaFeira", TercaFeira.IdPratoDoDia.ToString());
                    HttpContext.Session.SetString("IdPratoDoDia_QuartaFeira", QuartaFeira.IdPratoDoDia.ToString());
                    HttpContext.Session.SetString("IdPratoDoDia_QuintaFeira", QuintaFeira.IdPratoDoDia.ToString());
                    HttpContext.Session.SetString("IdPratoDoDia_SextaFeira", SextaFeira.IdPratoDoDia.ToString());
                    HttpContext.Session.SetString("IdPratoDoDia_Sabado", Sabado.IdPratoDoDia.ToString());
                    HttpContext.Session.SetString("IdPratoDoDia_Domingo", Domingo.IdPratoDoDia.ToString());

                }

                if (r.SegundaFeira_PratoDoDia == false)
                {
                    HttpContext.Session.SetString("SegundaFeiraHorario", "false");
                }
                else
                {
                    HttpContext.Session.SetString("SegundaFeiraHorario", "true");
                }
                if (r.TercaFeira_PratoDoDia == false)
                {
                    HttpContext.Session.SetString("TercaFeiraHorario", "false");
                }
                else
                {
                    HttpContext.Session.SetString("TercaFeiraHorario", "true");
                }
                if (r.QuartaFeira_PratoDoDia == false)
                {
                    HttpContext.Session.SetString("QuartaFeiraHorario", "false");
                }
                else
                {
                    HttpContext.Session.SetString("QuartaFeiraHorario", "true");
                }
                if (r.QuintaFeira_PratoDoDia == false)
                {
                    HttpContext.Session.SetString("QuintaFeiraHorario", "false");
                }
                else
                {
                    HttpContext.Session.SetString("QuintaFeiraHorario", "true");
                }
                if (r.SegundaFeira_PratoDoDia == false)
                {
                    HttpContext.Session.SetString("SextaFeiraHorario", "false");
                }
                else
                {
                    HttpContext.Session.SetString("SextaFeiraHorario", "true");
                }
                if (r.Sabado_PratoDoDia == false)
                {
                    HttpContext.Session.SetString("SabadoHorario", "false");
                }
                else
                {
                    HttpContext.Session.SetString("SabadoHorario", "true");
                }
                if (r.Domingo_PratoDoDia == false)
                {
                    HttpContext.Session.SetString("DomingoHorario", "false");
                }
                else
                {
                    HttpContext.Session.SetString("DomingoHorario", "true");
                }

                return RedirectToAction("GerirHorarioRestaurante", "PratoDoDias");
            }

            return RedirectToAction("GerirHorarioRestaurante", "PratoDoDias");
        }

        //Editar o prato do dia

        // GET: PratoDoDias/Edit/5
        public async Task<IActionResult> EditPratoDoDia(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pratoDoDia = await _context.PratoDoDia.FindAsync(id);
            if (pratoDoDia == null)
            {
                return NotFound();
            }
            ViewData["RestauranteId"] = new SelectList(_context.Restaurante, "IdRestaurante", "IdRestaurante", pratoDoDia.RestauranteId);
            return View(pratoDoDia);
        }

        // POST: PratoDoDias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPratoDoDia(int id, IFormFile Foto,[Bind("IdPratoDoDia,Descricao,Foto,DiaDaSemana,DataPrato,Tipo,Folga,RestauranteId")] PratoDoDia pratoDoDia)
        {

            if (ModelState.IsValid)
            {
                PratoDoDia p = _context.PratoDoDia.SingleOrDefault(p => p.IdPratoDoDia == id);
                
                if (Foto != null)
                {
                    string destination = Path.Combine(_environment.ContentRootPath, "wwwroot/Images/", Path.GetFileName(Foto.FileName));
                    FileStream fs = new FileStream(destination, FileMode.Create);

                    Foto.CopyTo(fs);
                    fs.Close();

                    p.Foto = "Images/" + Path.GetFileName(Foto.FileName);
                }
                if (pratoDoDia.Folga == true)
                {
                    p.Descricao = "";
                    p.Foto = null;
                    p.Tipo = "";
                    p.Folga = pratoDoDia.Folga;
                }
                else
                {
                    p.Descricao = pratoDoDia.Descricao;
                    p.Tipo = pratoDoDia.Tipo;
                    p.Folga = pratoDoDia.Folga;
                }


                _context.Update(p);
                 await _context.SaveChangesAsync();
                

                return await ClicouGerirHorarioRestaurante(p.RestauranteId);
            }
            ViewData["RestauranteId"] = new SelectList(_context.Restaurante, "IdRestaurante", "IdRestaurante", pratoDoDia.RestauranteId);
            return View(pratoDoDia);
        }


        //----------------------------------------------------------------------
        public async Task<IActionResult> PaginaInicialPratoDoDia()
        {
            var projetoDeLabWebContext = _context.PratoDoDia.Include(p => p.restaurante);
            return View(await projetoDeLabWebContext.ToListAsync());
        }
        //----------------------------------------------------------------------

        public async Task<IActionResult> ClicouVerHorarioRestaurante(int Id)
        {

            //verificar se clicou Gerir horario pela primeira vez, caso seja verdade vai criar 7 Pratos do dia
            //para aquele restaurante, um para cada dia
            //----
            if (ModelState.IsValid)
            {
                Restaurante r = _context.Restaurante.SingleOrDefault(r => r.IdRestaurante == Id);
                HttpContext.Session.SetString("GerirHorarioRestauranteId", Id.ToString());
                PratoDoDia p = _context.PratoDoDia.SingleOrDefault(p => p.RestauranteId == Id && p.DiaDaSemana == "SegundaFeira");
                if (p != null)
                {
                    HttpContext.Session.SetString("IdPratoDoDia_SegundaFeira", (p.IdPratoDoDia).ToString());
                    HttpContext.Session.SetString("IdPratoDoDia_TercaFeira", (p.IdPratoDoDia + 1).ToString());
                    HttpContext.Session.SetString("IdPratoDoDia_QuartaFeira", (p.IdPratoDoDia + 2).ToString());
                    HttpContext.Session.SetString("IdPratoDoDia_QuintaFeira", (p.IdPratoDoDia + 3).ToString());
                    HttpContext.Session.SetString("IdPratoDoDia_SextaFeira", (p.IdPratoDoDia + 4).ToString());
                    HttpContext.Session.SetString("IdPratoDoDia_Sabado", (p.IdPratoDoDia + 5).ToString());
                    HttpContext.Session.SetString("IdPratoDoDia_Domingo", (p.IdPratoDoDia + 6).ToString());
                }


                if (r.HorarioCriado == false)
                {
                    PratoDoDia SegundaFeira = new PratoDoDia();
                    PratoDoDia TercaFeira = new PratoDoDia();
                    PratoDoDia QuartaFeira = new PratoDoDia();
                    PratoDoDia QuintaFeira = new PratoDoDia();
                    PratoDoDia SextaFeira = new PratoDoDia();
                    PratoDoDia Sabado = new PratoDoDia();
                    PratoDoDia Domingo = new PratoDoDia();

                    //---
                    SegundaFeira.RestauranteId = r.IdRestaurante;
                    SegundaFeira.DiaDaSemana = "SegundaFeira";

                    TercaFeira.RestauranteId = r.IdRestaurante;
                    TercaFeira.DiaDaSemana = "TercaFeira";

                    QuartaFeira.RestauranteId = r.IdRestaurante;
                    QuartaFeira.DiaDaSemana = "QuartaFeira";

                    QuintaFeira.RestauranteId = r.IdRestaurante;
                    QuintaFeira.DiaDaSemana = "QuintaFeira";

                    SextaFeira.RestauranteId = r.IdRestaurante;
                    SextaFeira.DiaDaSemana = "SextaFeira";

                    Sabado.RestauranteId = r.IdRestaurante;
                    Sabado.DiaDaSemana = "Sabado";

                    Domingo.RestauranteId = r.IdRestaurante;
                    Domingo.DiaDaSemana = "Domingo";

                    _context.Add(SegundaFeira);
                    _context.Add(TercaFeira);
                    _context.Add(QuartaFeira);
                    _context.Add(QuintaFeira);
                    _context.Add(SextaFeira);
                    _context.Add(Sabado);
                    _context.Add(Domingo);


                    r.HorarioCriado = true;
                    _context.Update(r);
                    _context.SaveChanges();

                    HttpContext.Session.SetString("IdPratoDoDia_SegundaFeira", SegundaFeira.IdPratoDoDia.ToString());
                    HttpContext.Session.SetString("IdPratoDoDia_TercaFeira", TercaFeira.IdPratoDoDia.ToString());
                    HttpContext.Session.SetString("IdPratoDoDia_QuartaFeira", QuartaFeira.IdPratoDoDia.ToString());
                    HttpContext.Session.SetString("IdPratoDoDia_QuintaFeira", QuintaFeira.IdPratoDoDia.ToString());
                    HttpContext.Session.SetString("IdPratoDoDia_SextaFeira", SextaFeira.IdPratoDoDia.ToString());
                    HttpContext.Session.SetString("IdPratoDoDia_Sabado", Sabado.IdPratoDoDia.ToString());
                    HttpContext.Session.SetString("IdPratoDoDia_Domingo", Domingo.IdPratoDoDia.ToString());

                }

                if (r.SegundaFeira_PratoDoDia == false)
                {
                    HttpContext.Session.SetString("SegundaFeiraHorario", "false");
                }
                else
                {
                    HttpContext.Session.SetString("SegundaFeiraHorario", "true");
                }
                if (r.TercaFeira_PratoDoDia == false)
                {
                    HttpContext.Session.SetString("TercaFeiraHorario", "false");
                }
                else
                {
                    HttpContext.Session.SetString("TercaFeiraHorario", "true");
                }
                if (r.QuartaFeira_PratoDoDia == false)
                {
                    HttpContext.Session.SetString("QuartaFeiraHorario", "false");
                }
                else
                {
                    HttpContext.Session.SetString("QuartaFeiraHorario", "true");
                }
                if (r.QuintaFeira_PratoDoDia == false)
                {
                    HttpContext.Session.SetString("QuintaFeiraHorario", "false");
                }
                else
                {
                    HttpContext.Session.SetString("QuintaFeiraHorario", "true");
                }
                if (r.SegundaFeira_PratoDoDia == false)
                {
                    HttpContext.Session.SetString("SextaFeiraHorario", "false");
                }
                else
                {
                    HttpContext.Session.SetString("SextaFeiraHorario", "true");
                }
                if (r.Sabado_PratoDoDia == false)
                {
                    HttpContext.Session.SetString("SabadoHorario", "false");
                }
                else
                {
                    HttpContext.Session.SetString("SabadoHorario", "true");
                }
                if (r.Domingo_PratoDoDia == false)
                {
                    HttpContext.Session.SetString("DomingoHorario", "false");
                }
                else
                {
                    HttpContext.Session.SetString("DomingoHorario", "true");
                }

                return RedirectToAction("VerHorarioRestaurante", "PratoDoDias");
            }

            return RedirectToAction("VerHorarioRestaurante", "PratoDoDias");
        }

        public async Task<IActionResult> VerHorarioRestaurante()
        {
            var projetoDeLabWebContext = _context.PratoDoDia.Include(p => p.restaurante);
            return View(await projetoDeLabWebContext.ToListAsync());
        }






    }
}
