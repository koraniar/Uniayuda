using Data.Repositories.Interfaces;
using Entities.Entities;
using Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class CountryService : ICountryService
    {
        private readonly IGenericRepository<Country> _countryRepository;
        private readonly IEmailRepository _emailRepository;

        public CountryService(IGenericRepository<Country> countryRepository, IEmailRepository emailRepository)
        {
            _countryRepository = countryRepository;
            _emailRepository = emailRepository;
        }

        public void CreateCountry(Country country)
        {
            _countryRepository.Add(country);
        }

        public async Task<Country> GetCountryByIdAsync(Guid Id)
        {
            return await _countryRepository.GetByIdAsync(Id);
        }

        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            return await _countryRepository.GetAllAsync();
        }

        public void UpdateCountry(Country country)
        {
            _countryRepository.Update(country);
        }

        public void DeleteCountry(Country country)
        {
            _countryRepository.Delete(country);
        }

        //public async Task sendMSG()
        //{
        //    await _emailRepository.SendMessageSmtp(new EmailMessage()
        //    {
        //        To = "eganfs@gmail.com",
        //        HtmlBody = "Testing some Mailgun! <a href='https://www.google.com'>Google</a>",
        //        Subject = "Confirm email on Uniayuda.com",
        //        ToName = "koraniar"
        //    });
        //    await _emailRepository.SendAsync(new EmailMessage()
        //    {
        //        From = "korak@norak.com",
        //        To = "eganfs@gmail.com",
        //        HtmlBody = "hola mailgun",
        //        Subject = "rest api"
        //    });
        //}
    }
}
