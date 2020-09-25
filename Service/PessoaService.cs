using Domain;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class PessoaService
    {
    private PessoaRepository Repository { get; set; }

    public PessoaService(PessoaRepository repository)
    {
        this.Repository = repository;
    }

    public async Task<IEnumerable<Pessoa>> GetAll()
    {
        return await Repository.GetAllAsync();
    }

    public Pessoa GetPessoaById(Guid id)
    {
        return Repository.GetPessoaById(id);
    }

    public Pessoa GetPessoaByEmail(string nome)
    {
        return Repository.GetPessoaByEmail(nome);
    }


    public void Save(Pessoa pessoa)
    {
        if (this.GetPessoaByEmail(pessoa.Email) != null)
        {
            throw new Exception("Já existe uma Pessoa com este email, por favor cadastre outro email");
        }

        this.Repository.Save(pessoa);
    }

    public void Delete(Guid id)
    {
        Repository.Delete(id);
    }

    public void Update(Guid id, Pessoa pessoa)
    {
        Repository.Update(id, pessoa);
    }
}
}