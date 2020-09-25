using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace AT_WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmigosController : ControllerBase
    {
        // GET: api/Amigos
        private readonly AmigoService _service;

        public AmigosController(AmigoService service)
        {
            _service = service;
        }

        // GET: api/Amigos
        [HttpGet]
        public async Task<IEnumerable<Amigo>> GetAsync()
        {

            return await _service.GetAll();
        }

        [HttpGet("{id}")]
        public Amigo Get(int id)
        {
            return _service.GetAmigoById(id);
        }

        [HttpPost]
        public async Task PostAsync([FromBody] Amigo model, string nomePessoa)
        {
            try
            {
                await _service.SaveAsync(model, nomePessoa);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _service.Delete(id);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Amigo model)
        {
            _service.Update(id, model);
        }
    }
}