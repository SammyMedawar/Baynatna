using System.Threading.Tasks;
using Baynatna.Repositories;
using Baynatna.Repositories.Interfaces;
using Baynatna.Models;
using Baynatna.ViewModels;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Baynatna.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IVerificationTokenRepository _tokenRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(IUserRepository userRepository, IVerificationTokenRepository tokenRepository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<ServiceResult> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
                return new ServiceResult { Success = false, ErrorMessage = "Invalid username or password." };
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result == PasswordVerificationResult.Failed)
                return new ServiceResult { Success = false, ErrorMessage = "Invalid username or password." };
            return new ServiceResult { Success = true, Data = user };
        }

        public async Task<ServiceResult> RegisterAsync(string username, string password, string confirmPassword, string token)
        {
            if (password != confirmPassword)
                return new ServiceResult { Success = false, ErrorMessage = "Passwords do not match." };
            var existingUser = await _userRepository.GetByUsernameAsync(username);
            if (existingUser != null)
                return new ServiceResult { Success = false, ErrorMessage = "Username already exists." };
            var verificationToken = (await _tokenRepository.GetAllAsync()).FirstOrDefault(t => t.Token == token && !t.IsUsed);
            if (verificationToken == null)
                return new ServiceResult { Success = false, ErrorMessage = "Invalid or used token." };
            var user = new User { Username = username };
            user.PasswordHash = _passwordHasher.HashPassword(user, password);
            await _userRepository.AddAsync(user);

            verificationToken.IsUsed = true;
            verificationToken.IssuedToUserId = user.Id;
            await _tokenRepository.UpdateAsync(verificationToken);
            return new ServiceResult { Success = true };
        }

        public async Task<ServiceResult> RequestTokenAsync(string phone, string email, string idOrProof)
        {
            // TODO: Implement token request logic (e.g., store request, notify admin, etc.)
            await Task.CompletedTask;
            return new ServiceResult { Success = true };
        }
    }
} 