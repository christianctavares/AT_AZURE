using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain;
using Repository.Context;
using Service;
using Microsoft.AspNetCore.Http;

namespace PaginaWeb.Controllers
{
    public class AmigosController : Controller
    {
        private readonly AmigoService _service;

        public AmigosController(AmigoService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAll());
        }

        public ActionResult Details(int id)
        {
            var amigo = this._service.GetAmigoById(id);

            return View(amigo);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            var amigo = this._service.GetAmigoById(id);
            return View(amigo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Amigo amigo)
        {
            try
            {
                this._service.Update(id, amigo);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Amigo amigo, IFormCollection form)
        {
            try
            {
                if (ModelState.IsValid == false)
                    return View(amigo);

                var nomePessoa = form["nomePessoa"];
                await _service.SaveAsync(amigo, nomePessoa);

                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("APP_ERROR", ex.Message);
                return View(amigo);
            }
        }

        // GET: AlunoController/Delete/5
        public ActionResult Delete(int id)
        {
            var amigo = this._service.GetAmigoById(id);

            return View(amigo);
        }

        // POST: AlunoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Amigo amigo)
        {
            try
            {
                this._service.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
