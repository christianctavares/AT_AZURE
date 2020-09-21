using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Domain
{
    public class Estado
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        [NotMapped]
        public IFormFile FotoForm { get; set; }
        public string Foto { get; set; }

        [JsonIgnore]
        public Pais Pais { get; set; }
    }
}
