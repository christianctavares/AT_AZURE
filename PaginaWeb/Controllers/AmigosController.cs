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
using RestSharp;

namespace PaginaWeb.Controllers
{
    public class AmigosController : Controller
    {
        private readonly AmigoService _service;

        public AmigosController(AmigoService service)
        {
            _service = service;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var client = new RestClient();

            var request = new RestRequest("https://localhost:5001/api/amigos", DataFormat.Json);

            var response = await client.GetAsync<List<Amigo>>(request);
            
            //return View(response);
            return View(await _service.GetAll());
        }

        public ActionResult Details(int id)
        {

            var client = new RestClient();
            var requestAmigo = new RestRequest("https://localhost:5001/api/amigos/" + id, DataFormat.Json);

            var response = client.Get<Amigo>(requestAmigo);

            var amigo = response.Data;
            //var amigo = this._service.GetAmigoById(id);

            return View(amigo);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            var client = new RestClient();
            var requestAmigo = new RestRequest("https://localhost:5001/api/amigos/" + id, DataFormat.Json);

            var response = client.Get<Amigo>(requestAmigo);

            var amigo = response.Data;
            //var amigo = this._service.GetAmigoById(id);
            return View(amigo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Amigo amigo)
        {
            try
            {
                var client = new RestClient();
                var requestAmigo = new RestRequest("https://localhost:5001/api/amigos/" + id, DataFormat.Json);
                requestAmigo.AddJsonBody(amigo);
                var response = client.Put<Amigo>(requestAmigo);
                //this._service.Update(id, amigo);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Amigo amigo, IFormCollection form)
        {
            try
            {
                if (ModelState.IsValid == false)
                    return View(amigo);

                var nomePessoa = form["nomePessoa"];

                var client = new RestClient();
                var requestAmigo = new RestRequest("https://localhost:5001/api/amigos?nomePessoa=" + nomePessoa, DataFormat.Json);
                requestAmigo.AddJsonBody(amigo);
                
                var response = client.Post<Amigo>(requestAmigo);
                //await _service.SaveAsync(amigo, nomePessoa);

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
            var client = new RestClient();
            var requestAmigo = new RestRequest("https://localhost:5001/api/amigos/" + id, DataFormat.Json);
            
            var response = client.Get<Amigo>(requestAmigo);
            //var amigo = this._service.GetAmigoById(id);

            return View(response.Data);
        }

        // POST: AlunoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Amigo amigo)
        {
            try
            {
                var client = new RestClient();
                var requestAmigo = new RestRequest("https://localhost:5001/api/amigos/" + id, DataFormat.Json);
                requestAmigo.AddJsonBody(amigo);
                var response = client.Delete<Amigo>(requestAmigo);
                //this._service.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
