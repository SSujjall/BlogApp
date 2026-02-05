using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlogApp.Application.DTOs
{
    public class UserDTO
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class RegisterDTO
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }

    public class ResendVerificationDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class RegisterResponseDTO
    {
        public string EmailConfirmToken { get; set; }
    }

    public class ForgotPasswordResponseDTO
    {
        public string ForgotPasswordToken { get; set; }
    }

    public class ResetPasswordDTO
    {
        [Required]
        public string Password { get; set; } = null!;

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Token { get; set; } = null!;
    }

    public class LoginDTO
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }

    public class LoginResponseDTO
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }

    public class RefreshTokenRequestDTO
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }

    public class UpdateUserPwDTO
    {
        [JsonIgnore]
        public string? UserId { get; set; }

        [Required(ErrorMessage = "Current password is required")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New password is required")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match")]
        public string ConfirmPassword { get; set; } // Optional: password confirmation
    }
}