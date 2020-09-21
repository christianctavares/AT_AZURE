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
    public class PessoasController : ControllerBase
    {
        private readonly PessoaService _service;

        public PessoasController(PessoaService service)
        {
            _service = service;
        }

        // GET: api/Pessoas
        [HttpGet]
        public async Task<IEnumerable<Pessoa>> GetAsync()
        {
            return await _service.GetAll();
        }

        [HttpGet("{id}")]
        public Pessoa Get(Guid id)
        {
            return _service.GetPessoaById(id);
        }

        [HttpPost]
        public void Post([FromBody] Pessoa model)
        {
            try
            {
                _service.Save(model);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }

        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            _service.Delete(id);
        }

        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody] Pessoa model)
        {
            _service.Update(id, model);
        }
    }
}

