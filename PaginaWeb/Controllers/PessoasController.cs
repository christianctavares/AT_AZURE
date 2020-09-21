using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain;
using Repository.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Service;
using Microsoft.Azure.Storage.Blob;
using PaginaWeb.ViewModel;

namespace PaginaWeb.Controllers
{
    public class PessoasController : Controller
    {
        private readonly PessoaService _service;
        private readonly PaisService _paisService;
        private readonly EstadoService _estadoService;
        public PessoasController(PessoaService service, PaisService paisService, EstadoService estadoService)
        {
            _service = service;
            _paisService = paisService;
            _estadoService = estadoService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAll());
        }



        public ActionResult Details(Guid id)
        {
            var pessoa = this._service.GetPessoaById(id);
            var paisNome = _paisService.GetPaisById(Int16.Parse(pessoa.PaisId));
            var estadoNome = _estadoService.GetEstadoById(Int16.Parse(pessoa.PaisId));
            pessoa.PaisId = paisNome.Nome;
            pessoa.EstadoId = estadoNome.Nome;
            return View(pessoa);
        }

        public ActionResult DetailsAmigos(Guid id)
        {
            var pessoa = this._service.GetPessoaById(id);
            var list = new List<Amigo>();

            foreach (var item in pessoa.Amigos)
            {
                list.Add(item);
            }

            return View(list);
        }

        public ActionResult Create()
        {
            //var viewModel = new CriarPessoaViewModel();
            var paises = _paisService.GetAll().Result;
            var listaDePaises = new List<Pais>();
            foreach (var pais in paises)
            {
                listaDePaises.Add(pais);
            }

            var estados = _estadoService.GetAll().Result;
            var listaDeEstados = new List<Estado>();
            foreach (var estado in estados)
            {
                listaDeEstados.Add(estado);
            }

            var pessoa = new Pessoa();
            pessoa.Pais = listaDePaises;
            pessoa.Estado = listaDeEstados;
            

            return View(pessoa);
        }

        public ActionResult Edit(Guid id)
        {
            var pessoa = this._service.GetPessoaById(id);
            return View(pessoa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, Pessoa pessoa)
        {
            try
            {
                this._service.Update(id, pessoa);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pessoa pessoa)
        {
            try
            {
                if (ModelState.IsValid == false)
                    return View(pessoa);

                var urlFoto = UploadFotoPessoa(pessoa.FotoForm);

                pessoa.Foto = urlFoto;


                _service.Save(pessoa);

                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("APP_ERROR", ex.Message);
                return View(pessoa);
            }
        }

        // GET: AlunoController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var pessoa = this._service.GetPessoaById(id);

            return View(pessoa);
        }

        // POST: AlunoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, Pessoa pessoa)
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

        private string UploadFotoPessoa(IFormFile foto)
        {

            var reader = foto.OpenReadStream();
            var cloundStorageAccount = CloudStorageAccount.Parse(@"UseDevelopmentStorage=true");
            var blobClient = cloundStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("fotos-pessoas");
            container.CreateIfNotExists();
            var blob = container.GetBlockBlobReference(Guid.NewGuid().ToString());
            blob.UploadFromStream(reader);
            var destinoDaImagemNaNuvem = blob.Uri.ToString();
            return destinoDaImagemNaNuvem;

        }

    }
}
