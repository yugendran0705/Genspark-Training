using System;

namespace DocumentSharingAPI.Models.Dtos
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
