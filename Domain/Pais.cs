using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain
{
    public class Pais
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        [NotMapped]
        public IFormFile FotoForm { get; set; }
        public string Foto { get; set; }
        public IList<Estado> Estados { get; set; }
    }
}
