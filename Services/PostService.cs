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
                OriginalTitle = model.OriginalTitle,
                TranslatedTitle = model.TranslatedTitle,
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

        public async Task<PostListResult> GetPostsAsync(PostListQuery query)
        {
            var posts = await _postRepository.GetAllAsync();
            // Filter by tag
            if (query.TagId.HasValue)
                posts = posts.Where(p => p.PostTags != null && p.PostTags.Any(pt => pt.TagId == query.TagId.Value));
            // Filter by date
            if (query.DateFrom.HasValue)
                posts = posts.Where(p => p.CreatedAt >= query.DateFrom.Value);
            if (query.DateTo.HasValue)
                posts = posts.Where(p => p.CreatedAt <= query.DateTo.Value);
            // Search
            if (!string.IsNullOrWhiteSpace(query.Search))
                posts = posts.Where(p => p.OriginalBody.Contains(query.Search) || (p.TranslatedBody != null && p.TranslatedBody.Contains(query.Search)));
            // Sorting
            if (query.SortBy == "likes")
            {
                if (query.SortDir == "asc")
                    posts = posts.OrderBy(p => p.PostVotes?.Count(v => v.IsUpvote) ?? 0);
                else
                    posts = posts.OrderByDescending(p => p.PostVotes?.Count(v => v.IsUpvote) ?? 0);
            }
            else // date
            {
                if (query.SortDir == "asc")
                    posts = posts.OrderBy(p => p.CreatedAt);
                else
                    posts = posts.OrderByDescending(p => p.CreatedAt);
            }
            // Pagination (load more)
            var skip = (query.Page - 1) * query.PageSize;
            var paged = posts.Skip(skip).Take(query.PageSize + 1).ToList();
            var result = new PostListResult
            {
                Posts = paged.Take(query.PageSize).Select(p => new PostCardViewModel
                {
                    Id = p.Id,
                    Title = p.OriginalTitle,
                    Tag = p.PostTags != null && p.PostTags.Any() ? p.PostTags.First().Tag.Name : "",
                    Upvotes = p.PostVotes?.Count(v => v.IsUpvote) ?? 0,
                    Downvotes = p.PostVotes?.Count(v => !v.IsUpvote) ?? 0
                }).ToList(),
                HasMore = paged.Count > query.PageSize
            };
            return result;
        }

        public async Task<List<TagViewModel>> GetAllTagsAsync()
        {
            var tags = await _tagRepository.GetAllAsync();
            return tags.Select(t => new TagViewModel { Id = t.Id, Name = t.Name }).ToList();
        }
    }
} 