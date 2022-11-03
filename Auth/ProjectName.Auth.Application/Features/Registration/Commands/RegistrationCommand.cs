using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using ProjectName.Auth.Application.Enums;
using ProjectName.Auth.Application.Exceptions;
using ProjectName.Auth.Application.Wrappers;
using ProjectName.Auth.Domain.Entities.Identity;

namespace ProjectName.Auth.Application.Features.Registration.Commands
{    
    public sealed record RegistrationCommand : IRequest<Response<string>>
    {
        [Required] public string FirstName { get; init; } = default!;
        [Required] public string LastName { get; init; } = default!;
        [Required] public string MiddleName { get; set; } = default!;
        [Required][EmailAddress] public string Email { get; init; } = default!;
        [Required] public string Gender { get; set; } = default!;
        [Required] public string Language { get; set; } = default!;
        [Required][MinLength(6)] public string UserName { get; init; } = default!;
        [Required][MinLength(6)] public string Password { get; init; } = default!;
        [Required][Compare("Password")] public string ConfirmPassword { get; init; } = default!;
    }

    public class RegistrationCommandHandler : IRequestHandler<RegistrationCommand, Response<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public RegistrationCommandHandler(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<Response<string>> Handle(RegistrationCommand request, CancellationToken cancellationToken)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                throw new ApiException($"Username '{request.UserName}' is already taken.");
            }
            var user = _mapper.Map<User>(request);

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail is not null) throw new ApiException($"Email {request.Email} is already registered.");


            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded == false) throw new ApiException($"{result.Errors}");


            await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());
            var verificationUri = string.Empty;
            //var verificationUri = await SendVerificationEmail(user, origin);
            //TODO: Attach Email Service here and configure it via appsettings
            //await _emailService.SendAsync(new Application.DTOs.Email.EmailRequest() { From = "mail@codewithmukesh.com", To = user.Email, Body = $"Please confirm your account by visiting this URL {verificationUri}", Subject = "Confirm Registration" });
            return new Response<string>($"{user.Id}", message: $"User Registered. Please confirm your account by visiting this URL {verificationUri}");


        }
    }
}
