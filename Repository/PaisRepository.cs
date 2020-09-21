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
    public class PaisRepository
    {
        private readonly AtContext _context;

        public PaisRepository(AtContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pais>> GetAllAsync()
        {
            var paises = await _context.Paises.Include(x => x.Estados).ToListAsync();
            return paises;
        }

        public Pais GetPaisById(int id)
        {
            return _context.Paises.Include(x => x.Estados).FirstOrDefault(x => x.Id == id);
        }

        public void Save(Pais pais)
        {
            this._context.Paises.Add(pais);
            this._context.SaveChanges();
        }

        public Pais GetPaisByName(string nome)
        {
            return _context.Paises.Include(x => x.Estados).FirstOrDefault(x => x.Nome == nome);
        }

        public void Delete(int id)
        {
            var pais = _context.Paises.Include(x => x.Estados).FirstOrDefault(x => x.Id == id);

            if (pais.Estados != null)
            {
                foreach (var item in pais.Estados)
                {
                    _context.Estados.Remove(item);
                }
            }

            _context.Paises.Remove(pais);

            _context.SaveChanges();
        }

        public void Update(int id, Pais pais)
        {
            var paisOld = _context.Paises.FirstOrDefault(x => x.Id == id);

            paisOld.Nome = pais.Nome;
            paisOld.Foto = pais.Foto;


            _context.Paises.Update(paisOld);
            _context.SaveChanges();
        }

    }
}