using Microsoft.AspNetCore.Mvc;
using Baynatna.Services;
using Baynatna.ViewModels;
using Microsoft.AspNetCore.Http;
using Baynatna.Repositories.Interfaces;

namespace Baynatna.Controllers
{
    public class ComplaintController : Controller
    {
        private readonly IComplaintService _complaintService;
        private readonly ITagRepository _tagRepository;
        public ComplaintController(IComplaintService complaintService, ITagRepository tagRepository)
        {
            _complaintService = complaintService;
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
        public async Task<IActionResult> Create(CreateComplaintViewModel model)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                return RedirectToAction("Login", "User");
            if (!ModelState.IsValid)
            {
                var tags = await _tagRepository.GetAllAsync();
                ViewBag.Tags = tags.ToList();
                return View(model);
            }
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var result = await _complaintService.CreateComplaintAsync(userId, model);
            if (!result.Success)
            {
                var tags = await _tagRepository.GetAllAsync();
                ViewBag.Tags = tags.ToList();
                ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "An unknown error occurred.");
                return View(model);
            }
            return RedirectToAction("Index", "Complaint");
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] ComplaintListQuery query)
        {
            var complaints = await _complaintService.GetComplaintsAsync(query);
            var tags = await _complaintService.GetAllTagsAsync();

            var viewModel = new ComplaintListResult
            {
                Complaints = complaints.Select(c => new ComplaintListItemViewModel
                {
                    Id = c.Id,
                    ThreadId = c.ThreadId,
                    OriginalBody = c.OriginalBody,
                    // Map other properties as needed
                }).ToList()
            };

            ViewBag.Tags = tags;
            ViewBag.Query = query;
            return View(viewModel); 
        }


        [HttpPost]
        public async Task<IActionResult> Vote(int complaintId, bool isUpvote, string reason)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Json(new { success = false, error = "You must be logged in to vote." });
            var result = await _complaintService.VoteAsync(userId.Value, complaintId, isUpvote, reason);
            if (result.Success)
                return Json(new { success = true });
            return Json(new { success = false, error = result.ErrorMessage });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var complaint = await _complaintService.GetComplaintDetailsAsync(id, userId);
            if (complaint == null)
                return NotFound();
            return View(complaint);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "User");
            var result = await _complaintService.DeleteComplaintAsync(id, userId.Value);
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
                return RedirectToAction("Details", new { id = model.ComplaintId, action = model.VoteAction });
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "User");

            // Additional validation
            if (string.IsNullOrWhiteSpace(model.Body?.Trim()))
            {
                TempData["Error"] = "Comment text is required.";
                return RedirectToAction("Details", new { id = model.ComplaintId, action = model.VoteAction });
            }

            if (string.IsNullOrWhiteSpace(model.VoteAction))
            {
                TempData["Error"] = "Please select upvote or downvote.";
                return RedirectToAction("Details", new { id = model.ComplaintId });
            }

            var result = await _complaintService.AddCommentAsync(userId.Value, model);
            if (!result.Success)
            {
                TempData["Error"] = result.ErrorMessage;
                return RedirectToAction("Details", new { id = model.ComplaintId, action = model.VoteAction });
            }

            return RedirectToAction("Details", new { id = model.ComplaintId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteComment(int id, int complaintId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "User");
            var result = await _complaintService.DeleteCommentAsync(id, userId.Value);
            if (!result.Success)
            {
                TempData["Error"] = result.ErrorMessage;
            }
            return RedirectToAction("Details", new { id = complaintId });
        }
    }
} 