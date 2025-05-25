using System;
using System.Threading.Tasks;
using Baynatna.Repositories;
using Baynatna.Repositories.Interfaces;
using Baynatna.Models;
using Baynatna.ViewModels;
using System.Linq;
using System.Collections.Generic;

namespace Baynatna.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly ITagRepository _tagRepository;
        public PostService(IPostRepository postRepository, ITagRepository tagRepository)
        {
            _postRepository = postRepository;
            _tagRepository = tagRepository;
        }

        public async Task<ServiceResult> CreatePostAsync(int userId, CreatePostViewModel model)
        {
            // Check if user has posted in the last 12 hours
            var posts = await _postRepository.GetAllAsync();
            var lastPost = posts.Where(p => p.UserId == userId)
                                .OrderByDescending(p => p.CreatedAt)
                                .FirstOrDefault();
            if (lastPost != null && (DateTime.UtcNow - lastPost.CreatedAt).TotalHours < 12)
            {
                return new ServiceResult { Success = false, ErrorMessage = "You can only post once every 12 hours." };
            }
            var post = new Post
            {
                UserId = userId,
                OriginalBody = model.OriginalBody,
                TranslatedBody = model.TranslatedBody,
                IsTranslatedByModerator = false,
                VoiceMessageUrl = model.VoiceMessageUrl,
                IsQuarantined = false,
                CreatedAt = DateTime.UtcNow
            };
            await _postRepository.AddAsync(post);
            await _postRepository.SaveChangesAsync();
            // Add tag
            if (model.TagId > 0)
            {
                post.PostTags = new List<PostTag> { new PostTag { PostId = post.Id, TagId = model.TagId } };
                _postRepository.Update(post);
                await _postRepository.SaveChangesAsync();
            }
            return new ServiceResult { Success = true, Data = post };
        }
    }
} 