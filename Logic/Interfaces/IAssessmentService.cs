using Entities.DatabaseEntities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IAssessmentService
    {
        void Create(Assessment assessment);
        Task<Assessment> GetByIdAsync(Guid Id);
        Task<IEnumerable<Assessment>> GetAllByUserIdAsync(string clientId);
        Task<IEnumerable<Assessment>> GetAllByPostIdAsync(Guid postId);
        Task<Assessment> GetByUserIdAndPostIdAsync(string userId, Guid postId);
        Task<IEnumerable<Assessment>> GetAllAsync();
        void Update(Assessment assessment);
        void Delete(Assessment assessment);
    }
}
