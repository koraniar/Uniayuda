using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface ICountryService
    {
        void CreateCountry(Country country);
        Task<Country> GetCountryByIdAsync(Guid Id);
        Task<IEnumerable<Country>> GetAllCountriesAsync();
        void UpdateCountry(Country country);
        void DeleteCountry(Country country);
    }
}
