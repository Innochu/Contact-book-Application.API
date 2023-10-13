using ContactBook.Core.Services.Interfaces;
using ContactBook.Data.DTOs;
using ContactBook.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace ContactBook.Core.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> CreateNewUserAsync(CreateNewUserDTO model, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return false;
            }
            else
            {
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,

                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        modelState.AddModelError(string.Empty, error.Description);
                    }
                    return false;
                }
                else
                {
                    return true;
                }
            }

        }

        public async Task<bool> UpdateUserAsync(string userId, UpdateDTO model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.PasswordHash = model.Password;
            user.UserName = model.UserName;

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }


        public async Task<PaginationDTO> GetAllUserAsync(int page, int pagesize)
        {
            var totalusers = await _userManager.Users.CountAsync();
            var totalpages = (int)Math.Ceiling(totalusers / (double)pagesize);

            page = Math.Max(1, Math.Min(totalpages, page));

            var users = await _userManager.Users
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pagesize)
                .Take(pagesize)
                .ToListAsync();

            return new PaginationDTO
            {
                TotalUsers = totalusers,
                CurrentPage = page,
                PageSize = pagesize,
                Users = users
            };
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<User> GetUserByidAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
    }
}
