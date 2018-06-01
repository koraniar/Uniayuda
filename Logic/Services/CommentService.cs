using Data.Repositories.Interfaces;
using Entities.DatabaseEntities;
using Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class CommentService : ICommentService
    {
        private readonly IGenericRepository<Comment> _commentRepository;

        public CommentService(IGenericRepository<Comment> commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public void Create(Comment comment)
        {
            _commentRepository.Add(comment);
        }

        public async Task<Comment> GetByIdAsync(Guid Id)
        {
            return await _commentRepository.GetByIdAsync(Id);
        }

        public async Task<IEnumerable<Comment>> GetAllByUserIdAsync(string userId)
        {
            return await _commentRepository.GetManyAsync(k => k.UserId.Equals(userId));
        }

        public async Task<IEnumerable<Comment>> GetAllByPostIdAsync(Guid postId)
        {
            return (await _commentRepository.GetManyAsync(k => k.PostId.Equals(postId))).OrderBy(x => x.CreatedDate);
        }

        public async Task<IEnumerable<Comment>> GetLastCommentsByPostIdAsync(Guid postId, int quantity)
        {
            return (await _commentRepository.GetManyAsync(k => k.PostId.Equals(postId))).OrderByDescending(x => x.CreatedDate).Take(quantity);
        }

        public async Task<Comment> GetByUserIdAndPostIdAsync(string userId, Guid postId)
        {
            return await _commentRepository.GetFirstOrDefaultAsync(x => x.UserId.Equals(userId) && x.PostId.Equals(postId));
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _commentRepository.GetAllAsync();
        }

        public void Update(Comment comment)
        {
            _commentRepository.Update(comment);
        }

        public void Delete(Comment comment)
        {
            _commentRepository.Delete(comment);
        }
    }
}
