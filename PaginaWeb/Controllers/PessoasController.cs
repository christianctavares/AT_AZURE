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
using RestSharp;

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

        public IActionResult Index()
        {
            var client = new RestClient();

            var request = new RestRequest("https://localhost:5001/api/pessoas", DataFormat.Json);

            var response = client.Get<List<Pessoa>>(request);

            return View(response.Data);

        }



        public ActionResult Details(Guid id)
        {

            var client = new RestClient();
            var requestPessoa = new RestRequest("https://localhost:5001/api/pessoas/" + id, DataFormat.Json);

            var response = client.Get<Pessoa>(requestPessoa);

            var pessoa = response.Data;



            var requestPais = new RestRequest("https://localhost:5001/api/paises/" + pessoa.PaisId.ToString(), DataFormat.Json);

            var responsePais = client.Get<Pais>(requestPais);

            var paisNome = responsePais.Data;


            var requestEstado= new RestRequest("https://localhost:5001/api/estados/" + pessoa.EstadoId.ToString(), DataFormat.Json);

            var responseEstado = client.Get<Estado>(requestEstado);

            var estadoNome = responseEstado.Data;

           
            if(paisNome == null)
            {
                paisNome = new Pais();
                paisNome.Nome = "Pais Deletado";
            }
            if (estadoNome == null)
            {
                estadoNome = new Estado();
                estadoNome.Nome = "Estado Deletado";
            }
            pessoa.PaisId = paisNome.Nome;
            pessoa.EstadoId = estadoNome.Nome;
            return View(pessoa);

        }

        public ActionResult DetailsAmigos(Guid id)
        {
            var client = new RestClient();
            var request = new RestRequest("https://localhost:5001/api/pessoas/" + id, DataFormat.Json);

            var response = client.Get<Pessoa>(request);

            var pessoa = response.Data;
            
            var list = new List<Amigo>();

            foreach (var item in pessoa.Amigos)
            {
                list.Add(item);
            }

            return View(list);
        }

        public ActionResult Create()
        {
            
            var client = new RestClient();

            var requestPais = new RestRequest("https://localhost:5001/api/paises", DataFormat.Json);
            var paises = client.Get<List<Pais>>(requestPais);
            var listaDePaises = new List<Pais>();
            foreach (var pais in paises.Data)
            {
                listaDePaises.Add(pais);
            }

            var requestEstado = new RestRequest("https://localhost:5001/api/estados", DataFormat.Json);
            var estados = client.Get<List<Estado>>(requestEstado);
            var listaDeEstados = new List<Estado>();
            foreach (var estado in estados.Data)
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
            var client = new RestClient();
            var requestPessoa = new RestRequest("https://localhost:5001/api/pessoas/" + id, DataFormat.Json);

            var response = client.Get<Pessoa>(requestPessoa);

            var pessoa = response.Data;
            return View(pessoa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, Pessoa pessoa)
        {
            try
            {
                var client = new RestClient();
                var requestPessoa = new RestRequest("https://localhost:5001/api/pessoas/" + id, DataFormat.Json);
                requestPessoa.AddJsonBody(pessoa);
                var response = client.Put<Pessoa>(requestPessoa);
                
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

                var client = new RestClient();
                var requestPessoa = new RestRequest("https://localhost:5001/api/pessoas", DataFormat.Json);
                pessoa.FotoForm = null;
                requestPessoa.AddJsonBody(pessoa);
                var response = client.Post<Pessoa>(requestPessoa);

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
            var client = new RestClient();
            var requestPessoa = new RestRequest("https://localhost:5001/api/pessoas/" + id, DataFormat.Json);

            var response = client.Get<Pessoa>(requestPessoa);

            var pessoa = response.Data;

            return View(pessoa);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, Pessoa pessoa)
        {
            try
            {
                var client = new RestClient();
                var requestPessoa = new RestRequest("https://localhost:5001/api/pessoas/" + id, DataFormat.Json);

                var response = client.Delete<Pessoa>(requestPessoa);

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
