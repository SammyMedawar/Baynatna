using Microsoft.AspNetCore.Mvc;
using Baynatna.Services;
using Baynatna.ViewModels;
using Microsoft.AspNetCore.Http;
using Baynatna.Repositories.Interfaces;

namespace Baynatna.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly ITagRepository _tagRepository;
        public PostController(IPostService postService, ITagRepository tagRepository)
        {
            _postService = postService;
            _tagRepository = tagRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                return RedirectToAction("Login", "User");
            var tags = await _tagRepository.GetAllAsync();
            ViewBag.Tags = tags.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePostViewModel model)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                return RedirectToAction("Login", "User");
            if (!ModelState.IsValid)
            {
                var tags = await _tagRepository.GetAllAsync();
                ViewBag.Tags = tags.ToList();
                return View(model);
            }
            var userId = HttpContext.Session.GetInt32("UserId").Value;
            var result = await _postService.CreatePostAsync(userId, model);
            if (!result.Success)
            {
                var tags = await _tagRepository.GetAllAsync();
                ViewBag.Tags = tags.ToList();
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return View(model);
            }
            // TODO: Redirect to post details or home for now
            return RedirectToAction("Index", "Post");
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PostListQuery query)
        {
            var postsResult = await _postService.GetPostsAsync(query);
            var tags = await _postService.GetAllTagsAsync();
            ViewBag.Tags = tags;
            ViewBag.Query = query;
            return View(postsResult);
        }

        [HttpPost]
        public async Task<IActionResult> Vote(int postId, bool isUpvote, string reason)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Json(new { success = false, error = "You must be logged in to vote." });
            var result = await _postService.VoteAsync(userId.Value, postId, isUpvote, reason);
            if (result.Success)
                return Json(new { success = true });
            return Json(new { success = false, error = result.ErrorMessage });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var post = await _postService.GetPostDetailsAsync(id, userId);
            if (post == null)
                return NotFound();
            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "User");
            var result = await _postService.DeletePostAsync(id, userId.Value);
            if (!result.Success)
            {
                TempData["Error"] = result.ErrorMessage;
                return RedirectToAction("Details", new { id });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(AddCommentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill in all required fields.";
                return RedirectToAction("Details", new { id = model.PostId, action = model.VoteAction });
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "User");

            // Additional validation
            if (string.IsNullOrWhiteSpace(model.Body?.Trim()))
            {
                TempData["Error"] = "Comment text is required.";
                return RedirectToAction("Details", new { id = model.PostId, action = model.VoteAction });
            }

            if (string.IsNullOrWhiteSpace(model.VoteAction))
            {
                TempData["Error"] = "Please select upvote or downvote.";
                return RedirectToAction("Details", new { id = model.PostId });
            }

            var result = await _postService.AddCommentAsync(userId.Value, model);
            if (!result.Success)
            {
                TempData["Error"] = result.ErrorMessage;
                return RedirectToAction("Details", new { id = model.PostId, action = model.VoteAction });
            }

            return RedirectToAction("Details", new { id = model.PostId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteComment(int id, int postId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "User");
            var result = await _postService.DeleteCommentAsync(id, userId.Value);
            if (!result.Success)
            {
                TempData["Error"] = result.ErrorMessage;
            }
            return RedirectToAction("Details", new { id = postId });
        }
    }
} 