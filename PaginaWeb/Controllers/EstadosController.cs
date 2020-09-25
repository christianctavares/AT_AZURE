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
using RestSharp;

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
            var client = new RestClient();

            var request = new RestRequest("https://localhost:5001/api/estados", DataFormat.Json);

            var response = await client.GetAsync<List<Estado>>(request);

            return View(response);

            //return View(await _service.GetAll());
        }



        public ActionResult Details(int id)
        {
            //var estado = this._service.GetEstadoById(id);
            var client = new RestClient();
            var requestEstado = new RestRequest("https://localhost:5001/api/estados/" + id, DataFormat.Json);

            var response = client.Get<Estado>(requestEstado);

            var estado = response.Data;
            return View(estado);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            // var estado = this._service.GetEstadoById(id);
            var client = new RestClient();
            var requestEstado = new RestRequest("https://localhost:5001/api/estados/" + id, DataFormat.Json);

            var response = client.Get<Estado>(requestEstado);

            var estado = response.Data;
            return View(estado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Estado estado)
        {
            try
            {
                //this._service.Update(id, estado);
                var client = new RestClient();
                var requestEstado = new RestRequest("https://localhost:5001/api/estados/" + id, DataFormat.Json);
                requestEstado.AddJsonBody(estado);
                var response = client.Put<Estado>(requestEstado);
                
                
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
                estado.FotoForm = null;
                var nomePais = form["nomePais"];

                var client = new RestClient();
                var requestEstado = new RestRequest("https://localhost:5001/api/estados?nomePais=" + nomePais, DataFormat.Json);
                requestEstado.AddJsonBody(estado);

                var response = await client.PostAsync<Estado>(requestEstado);
                //await _service.SaveAsync(estado, nomePais);

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
            //var estado = this._service.GetEstadoById(id);
            var client = new RestClient();
            var requestEstado = new RestRequest("https://localhost:5001/api/estados/" + id, DataFormat.Json);

            var response = client.Get<Estado>(requestEstado);

            var estado = response.Data;

            return View(estado);
        }

        // POST: AlunoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Estado estado)
        {
            try
            {
                var client = new RestClient();
                var requestEstado = new RestRequest("https://localhost:5001/api/estados/" + id, DataFormat.Json);
                requestEstado.AddJsonBody(estado);
                var response = client.Delete<Estado>(requestEstado);
                //this._service.Delete(id);
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
