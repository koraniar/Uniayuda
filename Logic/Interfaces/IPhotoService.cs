using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IPhotoService
    {
        void CreatePhoto(Photo photo);
        Task<Photo> GetPhotoByIdAsync(Guid Id);
        Task<IEnumerable<Photo>> GetAllPhotosAsync();
        void UpdatePhoto(Photo photo);
        void DeletePhoto(Photo photo);
    }
}
