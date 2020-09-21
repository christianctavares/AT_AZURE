using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain
{
    public class Pessoa
    {
        public Guid Id { get; set; }

        [NotMapped]
        public IFormFile FotoForm { get; set; }
        public string Foto { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public DateTime DataNiver { get; set; }


        [NotMapped]
        public List<Estado> Estado { get; set; }
        public string EstadoId { get; set; }

        [NotMapped]
        public List<Pais> Pais { get; set; }
        public string PaisId { get; set; }


        public IList<Amigo> Amigos { get; set; }
    }
}
