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
    public class AmigoRepository
    {
        private readonly AtContext _context;

        public AmigoRepository(AtContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Amigo>> GetAllAsync()
        {
            var amigos = await _context.Amigos.Include(x => x.Pessoa).ToListAsync();
            return amigos;
        }

        public Amigo GetAmigoById(int id)
        {
            return _context.Amigos.FirstOrDefault(x => x.Id == id);
        }

        public async Task Save(Amigo amigo, string emailPessoa)
        {
            var pessoa = await _context.Pessoas.Include(x => x.Amigos).FirstOrDefaultAsync(x => x.Email == emailPessoa);
            if (pessoa == null)
            {
                throw new Exception("Nenhum amigo cadastrado com esse email.");
            }
            amigo.Pessoa = pessoa;
            this._context.Amigos.Add(amigo);
            this._context.SaveChanges();

        }

        public Amigo GetAmigoByEmail(string email)
        {
            return _context.Amigos.FirstOrDefault(x => x.Email == email);
        }

        public void Delete(int id)
        {
            var amigo = _context.Amigos.FirstOrDefault(x => x.Id == id);

            _context.Amigos.Remove(amigo);

            _context.SaveChanges();
        }

        public void Update(int id, Amigo amigo)
        {
            var amigoOld = _context.Amigos.FirstOrDefault(x => x.Id == id);

            amigoOld.Nome = amigo.Nome;
            amigoOld.Email = amigo.Email;
            amigoOld.Telefone = amigo.Telefone;
            amigoOld.DataNiver = amigo.DataNiver;

            _context.Amigos.Update(amigoOld);
            _context.SaveChanges();
        }
    }
}