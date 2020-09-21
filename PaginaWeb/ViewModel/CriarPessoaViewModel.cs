using Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaginaWeb.ViewModel
{
    public class CriarPessoaViewModel
    {

        [NotMapped]
        public IFormFile FotoForm { get; set; }
        public string Foto { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public DateTime DataNiver { get; set; }

        public List<EstadoViewModel> Estados { get; set; }
        public int EstadoId { get; set; }
        public List<PaisViewModel> Paises { get; set; }
        public int PaisId { get; set; }

    }

    public class EstadoViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }

    public class PaisViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }

    public class DadosPessoaViewModel
    {

        public Guid Id { get; set; }

        [NotMapped]
        public IFormFile FotoForm { get; set; }
        public string Foto { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public DateTime DataNiver { get; set; }
    }
}
