using Domain;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class PaisService
    {
        private PaisRepository Repository { get; set; }

        public PaisService(PaisRepository repository)
        {
            this.Repository = repository;
        }

        public async Task<IEnumerable<Pais>> GetAll()
        {
            return await Repository.GetAllAsync();
        }

        public Pais GetPaisById(int id)
        {
            return Repository.GetPaisById(id);
        }


        public Pais GetPaisByName(string nome)
        {
            return Repository.GetPaisByName(nome);
        }


        public void Save(Pais pais)
        {
            if (this.GetPaisByName(pais.Nome) != null)
            {
                throw new Exception("Já existe um Pais com este nome, por favor cadastre outro nome");
            }

            this.Repository.Save(pais);
        }

        public void Delete(int id)
        {
            Repository.Delete(id);
        }

        public void Update(int id, Pais pais)
        {
            Repository.Update(id, pais);
        }
    }
}