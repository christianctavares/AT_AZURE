using Domain;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class EstadoService
    {
        private EstadoRepository Repository { get; set; }

        public EstadoService(EstadoRepository repository)
        {
            this.Repository = repository;
        }

        public async Task<IEnumerable<Estado>> GetAll()
        {
            return await Repository.GetAllAsync();
        }

        public Estado GetEstadoById(int id)
        {
            return Repository.GetEstadoById(id);
        }


        public Estado GetEstadoByName(string nome)
        {
            return Repository.GetEstadoByName(nome);
        }


        public async Task SaveAsync(Estado estado, string nomePais)
        {
            if (this.GetEstadoByName(estado.Nome) != null)
            {
                throw new Exception("Já existe um Estado com este nome, por favor cadastre outro nome");
            }

            await this.Repository.Save(estado, nomePais);
        }

        public void Delete(int id)
        {
            Repository.Delete(id);
        }

        public void Update(int id, Estado estado)
        {
            Repository.Update(id, estado);
        }
    }
}
