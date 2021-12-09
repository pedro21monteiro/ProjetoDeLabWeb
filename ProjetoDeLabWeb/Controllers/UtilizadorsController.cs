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
    public class UtilizadorsController : Controller
    {
        private readonly ProjetoDeLabWebContext _context;

        public UtilizadorsController(ProjetoDeLabWebContext context)
        {
            _context = context;
        }

        // GET: Utilizadors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Utilizador.ToListAsync());
        }

        // GET: Utilizadors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Utilizador
                .FirstOrDefaultAsync(m => m.IdUtilizador == id);
            if (utilizador == null)
            {
                return NotFound();
            }

            return View(utilizador);
        }

        // GET: Utilizadors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Utilizadors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUtilizador,UserName,Password,Email,Bloqueado,Motivo,RegistoConfirmado,Categoria")] Utilizador utilizador)
        {
            if (ModelState.IsValid)
            {
                _context.Add(utilizador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(utilizador);
        }

        // GET: Utilizadors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Utilizador.FindAsync(id);
            if (utilizador == null)
            {
                return NotFound();
            }
            return View(utilizador);
        }

        // POST: Utilizadors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUtilizador,UserName,Password,Email,Bloqueado,Motivo,RegistoConfirmado,Categoria")] Utilizador utilizador)
        {
            if (id != utilizador.IdUtilizador)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(utilizador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UtilizadorExists(utilizador.IdUtilizador))
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
            return View(utilizador);
        }

        // GET: Utilizadors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Utilizador
                .FirstOrDefaultAsync(m => m.IdUtilizador == id);
            if (utilizador == null)
            {
                return NotFound();
            }

            return View(utilizador);
        }

        // POST: Utilizadors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var utilizador = await _context.Utilizador.FindAsync(id);
            _context.Utilizador.Remove(utilizador);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UtilizadorExists(int id)
        {
            return _context.Utilizador.Any(e => e.IdUtilizador == id);
        }


        //Minhas funções e views-----------------------------------------------------------------

        //Register----------------------------------------------------------------
        public IActionResult Register()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("IdUtilizador,UserName,Password,Email")] Utilizador utilizador)
        {
            if (ModelState.IsValid)
            {
            
                Utilizador u = _context.Utilizador.SingleOrDefault(u => u.UserName == utilizador.UserName);
                Utilizador e = _context.Utilizador.SingleOrDefault(e => e.Email == utilizador.Email);

                if (u == null && e == null)
                {
                    utilizador.Categoria = "Utilizador";
                    _context.Add(utilizador);
                    await _context.SaveChangesAsync();
                    HttpContext.Session.SetString("utilizador", utilizador.UserName);
                    HttpContext.Session.SetString("utilizadorId", utilizador.IdUtilizador.ToString());
                    return RedirectToAction("Index","Home");
                }
                if (u != null && e != null)
                {
                    ModelState.AddModelError("ErroUserNameeEmailExistente", "Ja existe um utilizador com esse Username e Email");
                    
                }
                if (u != null)
                {
                    ModelState.AddModelError("ErroUserNameExistente", "Ja existe um utilizador com esse Username");
                    
                }
                if (e != null)
                {
                    ModelState.AddModelError("ErroUserNameExistente", "Ja existe um utilizador com esse Email");
                    
                }
            }
            return View();
        }

        //Fim-Register----------------------------------------------------------------

        //LogIN----------------------------------------------------------------------
        public IActionResult LogIn()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult LogIn(string UserName, string Password)
        {
            if (ModelState.IsValid)
            {
                Utilizador u = _context.Utilizador.SingleOrDefault(u => u.UserName == UserName && u.Password == Password);

                if (u == null)
                {
                    ModelState.AddModelError("username", "Username or password are wrong");
                }
                else
                {
                    if (u.Bloqueado == true )
                    {
                        HttpContext.Session.SetString("Bloqueado", "true");
                        HttpContext.Session.SetString("Motivo", u.Motivo);
                        return RedirectToAction("ContaBloqueada", "Utilizadors");
                    }
                    else
                    {
                        HttpContext.Session.SetString("utilizador", UserName);
                        HttpContext.Session.SetString("utilizadorId", u.IdUtilizador.ToString());

                        if (u.Categoria == "Admin")
                        {
                            HttpContext.Session.SetString("categoria", "Admin");//mete na palavra chave categoria a palavra Admin
                        }

                        return RedirectToAction("Index", "Home");
                    }
                  
                }
            }
            return View();
        }
        //Fim-LogIN----------------------------------------------------------------------

        //Logout------------------------------------------------------------------------
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete(".ProjetoDeLabWeb.Session");
            return RedirectToAction("Index","Home");
        }
        //Fim-Logiut---------------------------------------------

        //GerirUtilizadores------------------------------------------------------------------------
        public async Task<IActionResult> GerirUtilizadores()
        {
            return View(await _context.Utilizador.ToListAsync());
        }
        //Fim-Gerir-Utilizadores---------------------------------------------


        //Bloquear---------------------------------------

        public IActionResult ClicouBloquear(int Id)
        {
            HttpContext.Session.SetString("IdUtilizadorBloquear", Id.ToString());

            return RedirectToAction("Bloquear", "Utilizadors");
        }

        public IActionResult Bloquear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Bloquear(string Motivo)
        {
            if (ModelState.IsValid)
            {
                Utilizador u = _context.Utilizador.SingleOrDefault(u => u.IdUtilizador == Int32.Parse(HttpContext.Session.GetString("IdUtilizadorBloquear")));

                if (u == null)
                {
                    ModelState.AddModelError("IdUtilizador", "IdUtilizador incorreto");
                }
                else
                {
                    u.Bloqueado = true;
                    u.Motivo = Motivo;
                    _context.Update(u);
                    await _context.SaveChangesAsync();
     
                    return RedirectToAction("GerirUtilizadores", "Utilizadors");
                }
            }
            return View();
        }


        public IActionResult ContaBloqueada()
        {
            return View();
        }

        //----------------------------------------------------------------------------------------------
        public async Task<IActionResult> DetailsPerfilUtilizador(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Utilizador
                .FirstOrDefaultAsync(m => m.IdUtilizador == id);
            if (utilizador == null)
            {
                return NotFound();
            }

            return View(utilizador);
        }
        //--------------------------------------------------------------------------------------------
        public async Task<IActionResult> EditarPassword(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Utilizador.FindAsync(id);
            if (utilizador == null)
            {
                return NotFound();
            }
            return View(utilizador);
        }

        // POST: Utilizadors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarPassword(int id, [Bind("IdUtilizador,UserName,Password,Email,Bloqueado,Motivo,RegistoConfirmado,Categoria")] Utilizador utilizador)
        {

            
            
                    Utilizador u = _context.Utilizador.SingleOrDefault(u => u.IdUtilizador == id);
                    u.Password = utilizador.Password;
                    _context.Update(u);
                    await _context.SaveChangesAsync();
                          
                return RedirectToAction("Perfil","Home");
            
            
        }

        public async Task<IActionResult> EditarEmail(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Utilizador.FindAsync(id);
            if (utilizador == null)
            {
                return NotFound();
            }
            return View(utilizador);
        }

        // POST: Utilizadors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarEmail(int id, [Bind("IdUtilizador,UserName,Password,Email,Bloqueado,Motivo,RegistoConfirmado,Categoria")] Utilizador utilizador)
        {

            Utilizador u = _context.Utilizador.SingleOrDefault(u => u.IdUtilizador == id);
            u.Email = utilizador.Email;
            _context.Update(u);
            await _context.SaveChangesAsync();

            return RedirectToAction("Perfil", "Home");


        }

        //Desbloquear--------------------------------------------
        public async Task<IActionResult> ClicouDesbloquear(int id)
        {
            if (ModelState.IsValid)
            {
                Utilizador u = _context.Utilizador.SingleOrDefault(u => u.IdUtilizador == id);

                u.Bloqueado = false;
                u.Motivo = "Vazio";
                _context.Update(u);
                await _context.SaveChangesAsync();

                return RedirectToAction("GerirUtilizadores", "Utilizadors");
            }
            else
            {
                ModelState.AddModelError("IdUtilizador", "IdUtilizador incorreto");
            }

            return RedirectToAction("GerirUtilizadores", "Utilizadors");

        }
        //--------------------------------------------------

        public async Task<IActionResult> ClicouDarAdmin(int id)
        {
            if (ModelState.IsValid)
            {
                Utilizador u = _context.Utilizador.SingleOrDefault(u => u.IdUtilizador == id);

                u.Categoria = "Admin";
                _context.Update(u);
                await _context.SaveChangesAsync();

                return RedirectToAction("GerirUtilizadores", "Utilizadors");
            }
            else
            {
                ModelState.AddModelError("IdUtilizador", "IdUtilizador incorreto");
            }

            return RedirectToAction("GerirUtilizadores", "Utilizadors");

        }

        //--------------------------------------------


        public async Task<IActionResult> ClicouRetirarAdmin(int id)
        {
            if (ModelState.IsValid)
            {
                Utilizador u = _context.Utilizador.SingleOrDefault(u => u.IdUtilizador == id);

                u.Categoria = "Utilizador";
                _context.Update(u);
                await _context.SaveChangesAsync();

                return RedirectToAction("GerirUtilizadores", "Utilizadors");
            }
            else
            {
                ModelState.AddModelError("IdUtilizador", "IdUtilizador incorreto");
            }

            return RedirectToAction("GerirUtilizadores", "Utilizadors");

        }
    }

}
