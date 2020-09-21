using Domain;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AmigoService
    {
        private AmigoRepository Repository { get; set; }

        public AmigoService(AmigoRepository repository)
        {
            this.Repository = repository;
        }

        public async Task<IEnumerable<Amigo>> GetAll()
        {
            return await Repository.GetAllAsync();
        }

        public Amigo GetAmigoById(int id)
        {
            return Repository.GetAmigoById(id);
        }


        public Amigo GetAmigoByEmail(string email)
        {
            return Repository.GetAmigoByEmail(email);
        }


        public async Task SaveAsync(Amigo amigo, string nomePessoa)
        {
            if (this.GetAmigoByEmail(amigo.Email) != null)
            {
                throw new Exception("Já existe um Amigo com este email, por favor cadastre outro email");
            }

            await this.Repository.Save(amigo, nomePessoa);
        }

        public void Delete(int id)
        {
            Repository.Delete(id);
        }

        public void Update(int id, Amigo amigo)
        {
            Repository.Update(id, amigo);
        }
    }
}