using Data.Repositories.Interfaces;
using Entities.DatabaseEntities;
using Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class AssessmentService : IAssessmentService
    {
        private readonly IGenericRepository<Assessment> _assessmentRepository;

        public AssessmentService(IGenericRepository<Assessment> assessmentRepository)
        {
            _assessmentRepository = assessmentRepository;
        }

        public void Create(Assessment assessment)
        {
            _assessmentRepository.Add(assessment);
        }

        public async Task<Assessment> GetByIdAsync(Guid Id)
        {
            return await _assessmentRepository.GetByIdAsync(Id);
        }

        public async Task<IEnumerable<Assessment>> GetAllByUserIdAsync(string userId)
        {
            return await _assessmentRepository.GetManyAsync(k => k.UserId.Equals(userId));
        }

        public async Task<IEnumerable<Assessment>> GetAllByPostIdAsync(Guid postId)
        {
            return await _assessmentRepository.GetManyAsync(k => k.PostId.Equals(postId));
        }

        public async Task<Assessment> GetByUserIdAndPostIdAsync(string userId, Guid postId)
        {
            return await _assessmentRepository.GetFirstOrDefaultAsync(x => x.UserId.Equals(userId) && x.PostId.Equals(postId));
        }

        public async Task<IEnumerable<Assessment>> GetAllAsync()
        {
            return await _assessmentRepository.GetAllAsync();
        }

        public void Update(Assessment assessment)
        {
            _assessmentRepository.Update(assessment);
        }

        public void Delete(Assessment assessment)
        {
            _assessmentRepository.Delete(assessment);
        }
    }
}
