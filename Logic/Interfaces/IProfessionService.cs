using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IProfessionService
    {
        void CreateProfession(Profession profession);
        Task<Profession> GetProfessionByIdAsync(Guid Id);
        Task<IEnumerable<Profession>> GetAllProfessionsAsync();
        void UpdateProfession(Profession profession);
        void DeleteProfession(Profession profession);
    }
}
