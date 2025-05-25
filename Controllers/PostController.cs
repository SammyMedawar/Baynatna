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
            return RedirectToAction("Index", "Home");
        }
    }
} 