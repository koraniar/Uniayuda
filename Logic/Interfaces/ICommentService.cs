using Entities.DatabaseEntities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface ICommentService
    {
        void Create(Comment comment);
        Task<Comment> GetByIdAsync(Guid Id);
        Task<IEnumerable<Comment>> GetAllByUserIdAsync(string clientId);
        Task<IEnumerable<Comment>> GetAllByPostIdAsync(Guid postId);
        Task<IEnumerable<Comment>> GetLastCommentsByPostIdAsync(Guid postId, int quantity);
        Task<Comment> GetByUserIdAndPostIdAsync(string userId, Guid postId);
        Task<IEnumerable<Comment>> GetAllAsync();
        void Update(Comment comment);
        void Delete(Comment comment);
    }
}
