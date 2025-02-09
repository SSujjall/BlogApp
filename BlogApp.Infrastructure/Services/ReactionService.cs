using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogApp.Application.Interface.IRepositories;
using BlogApp.Application.Interface.IServices;
using BlogApp.Domain.Entities.Abstracts;

namespace BlogApp.Infrastructure.Services
{
    public class ReactionService<T> : IReactionService<T> where T : Reaction
    {
        private readonly IReactionRepository<T> _reactionRepository;

        public ReactionService(IReactionRepository<T> reactionRepository)
        {
            _reactionRepository = reactionRepository;
        }

        public async Task<IEnumerable<T>> GetAllReactionsAsync()
        {
            return await _reactionRepository.GetAll(null);
        }

        public async Task<T?> GetReactionByIdAsync(int id)
        {
            return await _reactionRepository.GetById(id);
        }

        public async Task AddReactionAsync(T reaction)
        {
            await _reactionRepository.Add(reaction);
        }

        public async Task RemoveReactionAsync(int id)
        {
            var reaction = await _reactionRepository.GetById(id);
            await _reactionRepository.Delete(reaction);
        }
    }
}
