using Data.Repositories.Interfaces;
using Entities.Entities;
using Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IGenericRepository<Photo> _photoRepository;

        public PhotoService(IGenericRepository<Photo> photoRepository)
        {
            _photoRepository = photoRepository;
        }

        public void CreatePhoto(Photo photo)
        {
            _photoRepository.Add(photo);
        }

        public async Task<Photo> GetPhotoByIdAsync(Guid Id)
        {
            return await _photoRepository.GetByIdAsync(Id);
        }

        public async Task<IEnumerable<Photo>> GetAllPhotosAsync()
        {
            return await _photoRepository.GetAllAsync();
        }

        public void UpdatePhoto(Photo photo)
        {
            _photoRepository.Update(photo);
        }

        public void DeletePhoto(Photo photo)
        {
            _photoRepository.Delete(photo);
        }
    }
}
