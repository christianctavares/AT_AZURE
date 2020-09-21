using Domain;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class EstadoRepository
    {
        private readonly AtContext _context;

        public EstadoRepository(AtContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Estado>> GetAllAsync()
        {
            var estados = await _context.Estados.ToListAsync();
            return estados;
        }

        public Estado GetEstadoById(int id)
        {
            return _context.Estados.FirstOrDefault(x => x.Id == id);
        }

        public async Task Save(Estado estado, string nomePais)
        {
            var pais = await _context.Paises.Include(x => x.Estados).FirstOrDefaultAsync(x => x.Nome == nomePais);
            if (pais == null)
            {
                throw new Exception("Nenhum pais cadastrado com esse nome.");
            }
            estado.Pais = pais;
            this._context.Estados.Add(estado);
            this._context.SaveChanges();

        }

        public Estado GetEstadoByName(string nome)
        {
            return _context.Estados.FirstOrDefault(x => x.Nome == nome);
        }

        public void Delete(int id)
        {
            var estado = _context.Estados.FirstOrDefault(x => x.Id == id);

            _context.Estados.Remove(estado);

            _context.SaveChanges();
        }

        public void Update(int id, Estado estado)
        {
            var estadoOld = _context.Estados.FirstOrDefault(x => x.Id == id);

            estadoOld.Nome = estado.Nome;
            estadoOld.Foto = estado.Foto;


            _context.Estados.Update(estadoOld);
            _context.SaveChanges();
        }
    }
}