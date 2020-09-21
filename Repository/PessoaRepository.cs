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
    public class PessoaRepository
    {
        private readonly AtContext _context;

        public PessoaRepository(AtContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pessoa>> GetAllAsync()
        {
            var pessoas = await _context.Pessoas.Include(x => x.Amigos).ToListAsync();
            
            return pessoas;
        }

        public Pessoa GetPessoaById(Guid id)
        {
            var pessoa = _context.Pessoas.Include(x => x.Amigos).FirstOrDefault(x => x.Id == id);
            _context.Pessoas.Include(x => x.Pais).ThenInclude(x => x.Estados);
            return pessoa;
        }

        public void Save(Pessoa pessoa)
        {
            
            this._context.Pessoas.Add(pessoa);
            this._context.SaveChanges();
        }

        public Pessoa GetPessoaByEmail(string email)
        {
            return _context.Pessoas.Include(x => x.Amigos).FirstOrDefault(x => x.Email == email);
        }

        public void Delete(Guid id)
        {
            var pessoa = _context.Pessoas.Include(x => x.Amigos).FirstOrDefault(x => x.Id == id);

            if (pessoa.Amigos != null)
            {
                foreach (var item in pessoa.Amigos)
                {
                    _context.Amigos.Remove(item);
                }
            }

            _context.Pessoas.Remove(pessoa);

            _context.SaveChanges();
        }

        public void Update(Guid id, Pessoa pessoa)
        {
            var pessoaOld = _context.Pessoas.FirstOrDefault(x => x.Id == id);

            pessoaOld.Foto = pessoa.Foto;
            pessoaOld.FotoForm = pessoa.FotoForm;
            pessoaOld.Nome = pessoa.Nome;
            pessoaOld.Email = pessoa.Email;
            pessoaOld.Telefone = pessoa.Telefone;
            pessoaOld.DataNiver = pessoa.DataNiver;

            _context.Pessoas.Update(pessoaOld);
            _context.SaveChanges();
        }

    }
}