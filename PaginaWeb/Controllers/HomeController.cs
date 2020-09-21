using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaginaWeb.Models;
using PaginaWeb.ViewModel;
using Service;

namespace PaginaWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PessoaService _pessoaService;
        private readonly PaisService _paisService;
        private readonly EstadoService _estadoService;
        public HomeController(ILogger<HomeController> logger, PessoaService pessoaService, PaisService paisService, EstadoService estadoService)
        {
            _logger = logger;
            _pessoaService = pessoaService;
            _paisService = paisService;
            _estadoService = estadoService;
        }

        public IActionResult Index()
        {
            var paginaInicial = new PaginaInicialViewModel();

            var pessoas = _pessoaService.GetAll();
            paginaInicial.QuantidadePessoa = pessoas.Result.Count();

            var estados = _estadoService.GetAll();
            paginaInicial.QuantidadeEstado = estados.Result.Count();

            var paises = _paisService.GetAll();
            paginaInicial.QuantidadePais = paises.Result.Count();



            return View(paginaInicial);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
