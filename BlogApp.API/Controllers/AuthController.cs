using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.EmailService.Model;
using BlogApp.Application.Helpers.EmailService.Service;
using BlogApp.Application.Interface.IServices;
using BlogApp.Domain.Entities;
using Castle.Core.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BlogApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public AuthController(IAuthService authService, IEmailService emailService)
        {
            _authService = authService;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterDTO registerDto)
        {
            var response = await _authService.RegisterUser(registerDto);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return StatusCode((int)response.StatusCode, response);
            }

            var verificationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { token = response.Data.EmailConfirmToken, email = registerDto.Email }, Request.Scheme);
            var emailMessage = new EmailMessage(new[] { registerDto.Email }, "Please confirm your email", $"Please confirm your email by clicking the link: {verificationLink}");
            _emailService.SendEmail(emailMessage);
            return Ok(response);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var response = await _authService.ConfirmEmailVerification(token, email);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var response = await _authService.GenerateForgotPasswordLink(email);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var forgotPasswordLink = Url.Action(nameof(ResetPassword), "Authentication", new { token = response.Data, email }, Request.Scheme);
                var message = new EmailMessage(new string[] { email }, "Forgot Password Link", forgotPasswordLink!);
                _emailService.SendEmail(message);
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            var model = new ResetPasswordDTO
            {
                Token = token,
                Email = email
            };

            return Ok(new
            {
                model
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            var response = await _authService.ResetPasswordAsync(model);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginDTO model)
        {
            var response = await _authService.LoginUser(model);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return StatusCode((int)response.StatusCode, response);
            }

            return Ok(response);
        }
    }
}