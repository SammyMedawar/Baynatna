using Microsoft.AspNetCore.Mvc;
using Baynatna.Services;
using Baynatna.Models;
using Baynatna.ViewModels;

namespace Baynatna.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await _userService.LoginAsync(model.Username, model.Password);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return View(model);
            }
            // TODO: Set auth cookie/session
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await _userService.RegisterAsync(model.Username, model.Password, model.ConfirmPassword, model.Token);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return View(model);
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult TokenRequest() => View();

        [HttpPost]
        public async Task<IActionResult> TokenRequest(TokenRequestViewModel model)
        {
            //TODO: WE NEED TO CHANGE model.IdOrProof to be an image (iformfile) instead
            if (!ModelState.IsValid) return View(model);
            var result = await _userService.RequestTokenAsync(model.Phone, model.Email, model.IdOrProof);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return View(model);
            }
            ViewBag.Message = "Token request submitted.";
            return View();
        }
    }
} 