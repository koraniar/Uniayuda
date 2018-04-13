using Data.Repositories.Interfaces;
using Entities.Entities;
using Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class ProfessionService :IProfessionService
    {
        private readonly IGenericRepository<Profession> _professionRepository;

        public ProfessionService(IGenericRepository<Profession> professionRepository)
        {
            _professionRepository = professionRepository;
        }

        public void CreateProfession(Profession profession)
        {
            _professionRepository.Add(profession);
        }

        public async Task<Profession> GetProfessionByIdAsync(Guid Id)
        {
            return await _professionRepository.GetByIdAsync(Id);
        }

        public async Task<IEnumerable<Profession>> GetAllProfessionsAsync()
        {
            return await _professionRepository.GetAllAsync();
        }

        public void UpdateProfession(Profession profession)
        {
            _professionRepository.Update(profession);
        }

        public void DeleteProfession(Profession profession)
        {
            _professionRepository.Delete(profession);
        }
    }
}
