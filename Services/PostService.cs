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
        private readonly ICommentRepository _commentRepository;
        private readonly ICommentVoteRepository _commentVoteRepository;
        public PostService(IPostRepository postRepository, ITagRepository tagRepository, ICommentRepository commentRepository, ICommentVoteRepository commentVoteRepository)
        {
            _postRepository = postRepository;
            _tagRepository = tagRepository;
            _commentRepository = commentRepository;
            _commentVoteRepository = commentVoteRepository;
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

        public async Task<ServiceResult> VoteAsync(int userId, int postId, bool isUpvote, string reason)
        {
            // Check if post exists
            var post = (await _postRepository.GetAllAsync()).FirstOrDefault(p => p.Id == postId);
            if (post == null)
                return new ServiceResult { Success = false, ErrorMessage = "Post not found." };
            // Cannot vote on own post
            if (post.UserId == userId)
                return new ServiceResult { Success = false, ErrorMessage = "You cannot vote on your own post." };
            // Check for existing vote
            if (post.PostVotes != null && post.PostVotes.Any(v => v.UserId == userId))
                return new ServiceResult { Success = false, ErrorMessage = "You have already voted on this post." };
            // Reason required
            if (string.IsNullOrWhiteSpace(reason))
                return new ServiceResult { Success = false, ErrorMessage = "Reason is required." };
            // Add vote
            var vote = new PostVote
            {
                PostId = postId,
                UserId = userId,
                IsUpvote = isUpvote,
                Reason = reason,
                CreatedAt = DateTime.UtcNow
            };
            post.PostVotes = post.PostVotes ?? new List<PostVote>();
            post.PostVotes.Add(vote);
            _postRepository.Update(post);
            await _postRepository.SaveChangesAsync();
            return new ServiceResult { Success = true };
        }

        public async Task<PostDetailsViewModel?> GetPostDetailsAsync(int postId, int? userId)
        {
            var post = (await _postRepository.GetAllAsync()).FirstOrDefault(p => p.Id == postId);
            if (post == null) return null;
            var comments = await GetCommentsAsync(postId, userId);
            var upvotes = post.PostVotes?.Count(v => v.IsUpvote) ?? 0;
            var downvotes = post.PostVotes?.Count(v => !v.IsUpvote) ?? 0;
            bool? userVote = null;
            if (userId.HasValue && post.PostVotes != null)
            {
                var vote = post.PostVotes.FirstOrDefault(v => v.UserId == userId.Value);
                if (vote != null) userVote = vote.IsUpvote;
            }
            return new PostDetailsViewModel
            {
                Id = post.Id,
                Title = post.OriginalTitle,
                TranslatedTitle = post.TranslatedTitle,
                Body = post.OriginalBody,
                TranslatedBody = post.TranslatedBody,
                IsTranslatedByModerator = post.IsTranslatedByModerator,
                Tag = post.PostTags != null && post.PostTags.Any() ? post.PostTags.First().Tag.Name : null,
                CreatedAt = post.CreatedAt,
                IsOwner = userId.HasValue && post.UserId == userId.Value,
                IsQuarantined = post.IsQuarantined,
                Comments = comments,
                NewComment = new AddCommentViewModel { PostId = postId },
                Upvotes = upvotes,
                Downvotes = downvotes,
                UserVote = userVote
            };
        }

        public async Task<ServiceResult> DeletePostAsync(int postId, int userId)
        {
            var post = (await _postRepository.GetAllAsync()).FirstOrDefault(p => p.Id == postId);
            if (post == null)
                return new ServiceResult { Success = false, ErrorMessage = "Post not found." };
            if (post.UserId != userId)
                return new ServiceResult { Success = false, ErrorMessage = "You are not authorized to delete this post." };
            _postRepository.Delete(post);
            await _postRepository.SaveChangesAsync();
            return new ServiceResult { Success = true };
        }

        public async Task<ServiceResult> AddCommentAsync(int userId, AddCommentViewModel model)
        {
            // Check if post exists
            var post = (await _postRepository.GetAllAsync()).FirstOrDefault(p => p.Id == model.PostId);
            if (post == null)
                return new ServiceResult { Success = false, ErrorMessage = "Post not found." };
            if (string.IsNullOrEmpty(model.VoteAction))
                return new ServiceResult { Success = false, ErrorMessage = "Upvote or downvote is required." };

            // Check if user has already voted on this post
            if (post.PostVotes != null && post.PostVotes.Any(v => v.UserId == userId))
                return new ServiceResult { Success = false, ErrorMessage = "You have already voted on this post." };

            // Add the comment
            var comment = new Comment
            {
                PostId = model.PostId,
                UserId = userId,
                OriginalBody = model.Body,
                TranslatedBody = model.TranslatedBody,
                VoiceMessageUrl = model.VoiceMessageUrl,
                IsTranslatedByModerator = false,
                IsDeletedByModerator = false,
                CreatedAt = DateTime.UtcNow
            };
            await _commentRepository.AddAsync(comment);
            await _commentRepository.SaveChangesAsync();

            // Add the post vote
            bool isUpvote = model.VoteAction.ToLower() == "upvote";
            var postVote = new PostVote
            {
                PostId = model.PostId,
                UserId = userId,
                IsUpvote = isUpvote,
                Reason = $"Vote added with comment #{comment.Id}",
                CreatedAt = DateTime.UtcNow
            };

            post.PostVotes = post.PostVotes ?? new List<PostVote>();
            post.PostVotes.Add(postVote);
            _postRepository.Update(post);
            await _postRepository.SaveChangesAsync();

            return new ServiceResult { Success = true };
        }

        public async Task<ServiceResult> DeleteCommentAsync(int commentId, int userId)
        {
            var comment = (await _commentRepository.GetAllAsync()).FirstOrDefault(c => c.Id == commentId);
            if (comment == null)
                return new ServiceResult { Success = false, ErrorMessage = "Comment not found." };
            if (comment.UserId != userId)
                return new ServiceResult { Success = false, ErrorMessage = "You are not authorized to delete this comment." };

            // Get the post to remove associated post vote
            var post = (await _postRepository.GetAllAsync()).FirstOrDefault(p => p.Id == comment.PostId);
            if (post?.PostVotes != null)
            {
                var postVote = post.PostVotes.FirstOrDefault(v => v.UserId == userId);
                if (postVote != null)
                {
                    post.PostVotes.Remove(postVote);
                    _postRepository.Update(post);
                }
            }

            // Remove any comment votes
            var commentVotes = (await _commentVoteRepository.GetAllAsync()).Where(v => v.CommentId == commentId);
            foreach (var vote in commentVotes)
            {
                _commentVoteRepository.Delete(vote);
            }

            // Delete the comment
            _commentRepository.Delete(comment);

            // Save all changes
            await _postRepository.SaveChangesAsync();
            await _commentRepository.SaveChangesAsync();
            await _commentVoteRepository.SaveChangesAsync();

            return new ServiceResult { Success = true };
        }

        public async Task<ServiceResult> VoteCommentAsync(int userId, int commentId, bool isUpvote)
        {
            var comment = (await _commentRepository.GetAllAsync()).FirstOrDefault(c => c.Id == commentId);
            if (comment == null)
                return new ServiceResult { Success = false, ErrorMessage = "Comment not found." };
            if (comment.UserId == userId)
                return new ServiceResult { Success = false, ErrorMessage = "You cannot vote on your own comment." };
            var existingVote = (await _commentVoteRepository.GetAllAsync()).FirstOrDefault(v => v.CommentId == commentId && v.UserId == userId);
            if (existingVote != null)
                return new ServiceResult { Success = false, ErrorMessage = "You have already voted on this comment." };
            var vote = new CommentVote
            {
                CommentId = commentId,
                UserId = userId,
                IsUpvote = isUpvote,
                CreatedAt = DateTime.UtcNow
            };
            await _commentVoteRepository.AddAsync(vote);
            await _commentVoteRepository.SaveChangesAsync();
            return new ServiceResult { Success = true };
        }

        public async Task<List<CommentViewModel>> GetCommentsAsync(int postId, int? userId)
        {
            var comments = (await _commentRepository.GetAllAsync()).Where(c => c.PostId == postId).ToList();
            var votes = await _commentVoteRepository.GetAllAsync();
            return comments.Select(c => new CommentViewModel
            {
                Id = c.Id,
                PostId = c.PostId,
                UserId = c.UserId,
                Body = c.OriginalBody,
                TranslatedBody = c.TranslatedBody,
                CreatedAt = c.CreatedAt,
                IsOwner = userId.HasValue && c.UserId == userId.Value,
                Upvotes = votes.Count(v => v.CommentId == c.Id && v.IsUpvote),
                Downvotes = votes.Count(v => v.CommentId == c.Id && !v.IsUpvote),
                CanVote = userId.HasValue && c.UserId != userId.Value && !votes.Any(v => v.CommentId == c.Id && v.UserId == userId.Value),
                CanDelete = userId.HasValue && c.UserId == userId.Value
            }).ToList();
        }
    }
} 