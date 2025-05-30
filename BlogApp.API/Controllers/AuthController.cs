﻿using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.EmailService.Model;
using BlogApp.Application.Helpers.EmailService.Service;
using BlogApp.Application.Helpers.GoogleAuthService.Model;
using BlogApp.Application.Helpers.GoogleAuthService.Service;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Application.Interface.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace BlogApp.API.Controllers
{
    [EnableRateLimiting("WritePolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        private readonly IGoogleAuthService _googleAuthService;

        public AuthController(IAuthService authService, IEmailService emailService,
                              IGoogleAuthService googleAuthService)
        {
            _authService = authService;
            _emailService = emailService;
            _googleAuthService = googleAuthService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterDTO registerDto)
        {
            var response = await _authService.RegisterUser(registerDto);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return Accepted(response);
            }

            var verificationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { token = response.Data.EmailConfirmToken, email = registerDto.Email }, Request.Scheme);
            var emailMessage = new EmailMessage(new[] { registerDto.Email }, "Please confirm your email", $"Please confirm your email by clicking the link: {verificationLink}");
            await _emailService.SendEmailAsync(emailMessage);
            return Ok(response);
        }

        [EnableRateLimiting("ReadPolicy")]
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

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var response = await _authService.GenerateForgotPasswordLink(email);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var forgotPasswordLink = Url.Action(nameof(ResetPassword), "Auth", new { token = response.Data.ForgotPasswordToken, email }, Request.Scheme);
                var message = new EmailMessage(new string[] { email }, "Password Reset Link", $"Click the link to reset password: {forgotPasswordLink!}");
                await _emailService.SendEmailAsync(message);
                return Ok(response);
            }

            return BadRequest(response);
        }

        [EnableRateLimiting("ReadPolicy")]
        [HttpGet("get-reset-password-model")]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            var model = new
            {
                Token = token,
                Email = email
            };

            var response = ApiResponse<object>.Success(model, "Reset Password Token.");
            return Ok(response);
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

        [EnableRateLimiting("ReadPolicy")]
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginDTO model)
        {
            var response = await _authService.LoginUser(model);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return Accepted(response);
            }

            return Ok(response);
        }

        #region Google Auth
        [EnableRateLimiting("ReadPolicy")]
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginDTO model)
        {
            var response = await _googleAuthService.HandleGoogleLogin(model);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return StatusCode((int)response.StatusCode, response);
            }

            return Ok(response);
        }
        #endregion

        [EnableRateLimiting("ReadPolicy")]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDTO model)
        {
            var response = await _authService.RefreshToken(model);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return StatusCode((int)response.StatusCode, response);
            }

            return Ok(response);
        }

        [EnableRateLimiting("ReadPolicy")]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _authService.LogoutUser(userId);
            return Ok(response);
        }

        [EnableRateLimiting("ReadPolicy")]
        [HttpGet("auth-test")]
        [Authorize]
        public async Task<IActionResult> AuthorizeTest()
        {
            var authHeader = HttpContext.Request.Headers.Authorization.ToString();
            string jwtToken = authHeader.Replace("Bearer ", "");
            var jwt = new JwtSecurityToken(jwtToken);
            var res = $"Authenticated! {Environment.NewLine}";
            res += $"{Environment.NewLine} Jwt Exp Time: {jwt.ValidTo.ToLocalTime()}, " +
                $"Current Time: {DateTime.Now.ToLongTimeString()}";
            return Ok(res);
        }
    }
}