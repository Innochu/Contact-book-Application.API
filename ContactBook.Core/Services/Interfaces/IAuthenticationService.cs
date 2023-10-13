using ContactBook.Data.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ContactBook.Core.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> RegisterUserAsync(RegistrationDTO model, ModelStateDictionary modelState, string role);

        Task<string> LoginAsync(LogInDTO model);
    }


}
