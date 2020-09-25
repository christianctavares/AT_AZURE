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
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.AspNetCore.Http;
using RestSharp;

namespace PaginaWeb.Controllers
{
    public class PaisesController : Controller
    {
        private readonly PaisService _service;

        public PaisesController(PaisService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var client = new RestClient();

            var request = new RestRequest("https://localhost:5001/api/paises", DataFormat.Json);

            var response = await client.GetAsync<List<Pais>>(request);

            return View(response);
        }



        public ActionResult Details(int id)
        {
            //var pais = this._service.GetPaisById(id);
            var client = new RestClient();
            var requestPais = new RestRequest("https://localhost:5001/api/paises/" + id, DataFormat.Json);

            var response = client.Get<Pais>(requestPais);

            var pais = response.Data;
            return View(pais);
        }

        public ActionResult DetailsEstados(int id)
        {

            //var pais = this._service.GetPaisById(id);
            var client = new RestClient();
            var request = new RestRequest("https://localhost:5001/api/paises/" + id, DataFormat.Json);

            var response = client.Get<Pais>(request);

            var pais = response.Data;

            var list = new List<Estado>();
            foreach (var item in pais.Estados)
            {
                list.Add(item);
            }

            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            //var pais = this._service.GetPaisById(id);
            var client = new RestClient();
            var requestPais = new RestRequest("https://localhost:5001/api/paises/" + id, DataFormat.Json);

            var response = client.Get<Pais>(requestPais);

            var pais = response.Data;

            return View(pais);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Pais pais)
        {
            try
            {
                //this._service.Update(id, pais);
                var client = new RestClient();
                var requestPais = new RestRequest("https://localhost:5001/api/paises/" + id, DataFormat.Json);
                requestPais.AddJsonBody(pais);
                var response = client.Put<Pais>(requestPais);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pais pais)
        {
            try
            {
                if (ModelState.IsValid == false)
                    return View(pais);

                var urlFoto = UploadFotoPais(pais.FotoForm);
                pais.Foto = urlFoto;

                var client = new RestClient();
                var requestPais = new RestRequest("https://localhost:5001/api/paises", DataFormat.Json);
                pais.FotoForm = null;
                requestPais.AddJsonBody(pais);
                var response = client.Post<Pais>(requestPais);
                //_service.Save(pais);

                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("APP_ERROR", ex.Message);
                return View(pais);
            }
        }

        // GET: AlunoController/Delete/5
        public ActionResult Delete(int id)
        {
            // var pais = this._service.GetPaisById(id);
            var client = new RestClient();
            var requestPais = new RestRequest("https://localhost:5001/api/paises/" + id, DataFormat.Json);

            var response = client.Get<Pais>(requestPais);

            var pais = response.Data;

            return View(pais);
        }

        // POST: AlunoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Pais pais)
        {
            try
            {
                var client = new RestClient();
                var requestPais = new RestRequest("https://localhost:5001/api/paises/" + id, DataFormat.Json);

                var response = client.Delete<Pais>(requestPais);
                //this._service.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private string UploadFotoPais(IFormFile foto)
        {

            var reader = foto.OpenReadStream();
            var cloundStorageAccount = CloudStorageAccount.Parse(@"UseDevelopmentStorage=true");
            var blobClient = cloundStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("fotos-paises");
            container.CreateIfNotExists();
            var blob = container.GetBlockBlobReference(Guid.NewGuid().ToString());
            blob.UploadFromStream(reader);
            var destinoDaImagemNaNuvem = blob.Uri.ToString();
            return destinoDaImagemNaNuvem;

        }

    }
}
