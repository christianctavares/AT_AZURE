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
    public class PaisesController : ControllerBase
    {
        private readonly PaisService _service;

        public PaisesController(PaisService service)
        {
            _service = service;
        }

        // GET: api/Paises
        [HttpGet]
        public async Task<IEnumerable<Pais>> GetAsync()
        {
            return await _service.GetAll();
        }

        [HttpGet("{id}")]
        public Pais Get(int id)
        {
            return _service.GetPaisById(id);
        }

        [HttpPost]
        public void Post([FromBody] Pais model)
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
        public void Delete(int id)
        {
            _service.Delete(id);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Pais model)
        {
            _service.Update(id, model);
        }
    }
}

