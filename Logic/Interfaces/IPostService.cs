using Entities.DatabaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IPostService
    {
        void Create(Post post);
        Task<Post> GetByIdAsync(Guid Id);
        Task<IEnumerable<Post>> GetAllByUserIdAsync(string clientId);
        Task<IEnumerable<Post>> GetAllAsync();
        void Update(Post post);
        void Delete(Post post);
    }
}
