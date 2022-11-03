using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ProjectName.Auth.Application.DTOs.Account
{
    public sealed record AuthenticationResponse
    {
        public Guid Id { get; init; }
        public string Email { get; init; } = default!;
        public List<string> Roles { get; init; } = default!;
        public bool IsVerified { get; init; }
        public string JWToken { get; init; } = default!;
    }
}
