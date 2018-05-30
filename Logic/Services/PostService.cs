using Data.Repositories.Interfaces;
using Entities.DatabaseEntities;
using Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class PostService : IPostService
    {
        private readonly IGenericRepository<Post> _postRepository;

        public PostService(IGenericRepository<Post> postRepository)
        {
            _postRepository = postRepository;
        }

        public void Create(Post post)
        {
            _postRepository.Add(post);
        }

        public async Task<Post> GetByIdAsync(Guid Id)
        {
            return await _postRepository.GetByIdAsync(Id);
        }

        public async Task<IEnumerable<Post>> GetAllByUserIdAsync(string userId)
        {
            return await _postRepository.GetManyAsync(k => k.UserId.Equals(userId));
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await _postRepository.GetAllAsync();
        }

        public void Update(Post post)
        {
            _postRepository.Update(post);
        }

        public void Delete(Post post)
        {
            _postRepository.Delete(post);
        }
    }
}
