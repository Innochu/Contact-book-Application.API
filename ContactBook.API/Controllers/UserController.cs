using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ContactBook.Core.Services.Interfaces;
using ContactBook.Data.DTOs;
using ContactBook.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactBook.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _appUserService;
        private readonly UserManager<User> _userManager;
        public UserController(IUserService appUserService, UserManager<User> userManager)
        {
            _appUserService = appUserService;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add-new-user")]
        public async Task<ActionResult> AddNewUser([FromBody] CreateNewUserDTO model)
        {
            var result = await _appUserService.CreateNewUserAsync(model, ModelState);
            if (!result)
            {
                return BadRequest(ModelState);
            }
            return Ok(new
            {
                Message = "User created successfully"
            });
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateUser(string id, [FromBody] UpdateDTO model)
        {
            var userUpdate = await _appUserService.UpdateUserAsync(id, model);
            if (!userUpdate)
            {
                return BadRequest(new
                {
                    Message = "Update failed"
                });
            }
            return Ok(new
            {
                Message = "User Updated successfully"
            });

        }
        [HttpGet("all-users")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetAllUsers(int page, int pageSize)
        {
            var paginatedResult = await _appUserService.GetAllUserAsync(page, pageSize);
            return Ok(paginatedResult);
        }

        [HttpGet("email")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _appUserService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new
                {
                    Message = "User not found"
                });
            }
            return Ok($"User '{user.UserName}' was found ");
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _appUserService.GetUserByidAsync(id);
            if (user == null)
            {
                return NotFound(new
                {
                    Message = "User not found"
                });
            }
            return Ok($"User '{user.UserName}' was found ");
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var userDeleted = await _appUserService.DeleteUserAsync(id);
            if (userDeleted == null)
            {
                return BadRequest(new
                {
                    Message = "User not found or failed to delete user"
                });
            }
            return Ok(new
            {
                Message = "User deleted successfully"
            });
        }

        [HttpPatch("image/{id}")]
        public async Task<IActionResult> UpUserLoadImage(string id, IFormFile image)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { Messsage = "User not found" });
            }
            if (image == null)
            {
                return BadRequest(new { Messsage = "image file is required" });
            }
            if (image.Length <= 0)
            {
                return BadRequest(new { Message = "image file is empty" });
            }

            var cloudinary = new Cloudinary(new Account(
              "dh7zw7o9y",
              "985116681226713",
              "tZ5GBhj-DmkK4kG2GaASFnsjDbk"
            ));
            var upLoad = new ImageUploadParams
            {
                File = new FileDescription(image.FileName, image.OpenReadStream())
            };
            var upLoadResult = await cloudinary.UploadAsync(upLoad);

            user.ImageUrl = upLoadResult.SecureUri.AbsoluteUri;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                return BadRequest(new { Message = "image update failed" });
            }
            return Ok(new { Message = "user image updated successfully" });
        }
    }
}
