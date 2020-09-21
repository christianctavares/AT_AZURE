using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain;
using Repository.Context;
using Service;

namespace AT_WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadosController : ControllerBase
    {
        private readonly EstadoService _service;

        public EstadosController(EstadoService service)
        {
            _service = service;
        }

        // GET: api/Estados
        [HttpGet]
        public async Task<IEnumerable<Estado>> GetAsync()
        {
            return await _service.GetAll();
        }

        [HttpGet("{id}")]
        public Estado Get(int id)
        {
            return _service.GetEstadoById(id);
        }

        [HttpPost]
        public async Task PostAsync([FromBody] Estado model, string nomePais)
        {
            try
            {
                await _service.SaveAsync(model, nomePais);
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
        public void Put(int id, [FromBody] Estado model)
        {
            _service.Update(id, model);
        }
    }
}
