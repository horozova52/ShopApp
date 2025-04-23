using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using ShopApp.Core;
using ShopApp.Shared.DTO;
using ShopApp.UseCases.Services.Email;

namespace ShopApp.Server.Components.Account;

internal sealed class IdentityNoOpEmailSender : IEmailSender<ApplicationUser>
{
    private readonly IEmailService _customEmailService;
    private readonly IConfiguration _config;

    public IdentityNoOpEmailSender(IEmailService customEmailService, IConfiguration config)
    {
        _customEmailService = customEmailService;
        _config = config;
    }

    public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
    {
        var dto = new EmailMessageDTO
        {
            From = _config["SmtpSettings:Username"],
            To = email,
            Subject = "Confirm your email",
            Body = $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.",
            IsHtml = true
        };

        return _customEmailService.SendEmailAsync(dto);
    }

    public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
    {
        var dto = new EmailMessageDTO
        {
            From = _config["SmtpSettings:Username"],
            To = email,
            Subject = "Reset your password",
            Body = $"Please reset your password by <a href='{resetLink}'>clicking here</a>.",
            IsHtml = true
        };

        return _customEmailService.SendEmailAsync(dto);
    }

    public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
    {
        var dto = new EmailMessageDTO
        {
            From = _config["SmtpSettings:Username"],
            To = email,
            Subject = "Reset your password",
            Body = $"Please reset your password using the following code: {resetCode}",
            IsHtml = false
        };

        return _customEmailService.SendEmailAsync(dto);
    }
}
