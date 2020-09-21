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
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace PaginaWeb.Controllers
{
    public class EstadosController : Controller
    {
        private readonly EstadoService _service;

        public EstadosController(EstadoService service)
        {
            _service = service;
        }

        // GET: Livros
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAll());
        }



        public ActionResult Details(int id)
        {
            var estado = this._service.GetEstadoById(id);

            return View(estado);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            var estado = this._service.GetEstadoById(id);
            return View(estado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Estado estado)
        {
            try
            {
                this._service.Update(id, estado);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Estado estado, IFormCollection form)
        {
            try
            {
                if (ModelState.IsValid == false)
                    return View(estado);

                var urlFoto = UploadFotoEstado(estado.FotoForm);
                estado.Foto = urlFoto;

                var nomePais = form["nomePais"];
                await _service.SaveAsync(estado, nomePais);

                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("APP_ERROR", ex.Message);
                return View(estado);
            }
        }

        // GET: AlunoController/Delete/5
        public ActionResult Delete(int id)
        {
            var estado = this._service.GetEstadoById(id);

            return View(estado);
        }

        // POST: AlunoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Estado estado)
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

        private string UploadFotoEstado(IFormFile foto)
        {

            var reader = foto.OpenReadStream();
            var cloundStorageAccount = CloudStorageAccount.Parse(@"UseDevelopmentStorage=true");
            var blobClient = cloundStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("fotos-estados");
            container.CreateIfNotExists();
            var blob = container.GetBlockBlobReference(Guid.NewGuid().ToString());
            blob.UploadFromStream(reader);
            var destinoDaImagemNaNuvem = blob.Uri.ToString();
            return destinoDaImagemNaNuvem;

        }
    }
}
