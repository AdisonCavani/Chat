﻿using Chat.Core.Models.Requests;
using Chat.WebApi.Models.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.WebApi.Extensions;

public static class Validators
{
    public static void AddValidators(this IServiceCollection services)
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;

        services.AddScoped<IValidator<ChangePasswordDto>, ChangePasswordDtoValidator>();
        services.AddScoped<IValidator<ConfirmEmailDto>, ConfirmEmailDtoValidator>();
        services.AddScoped<IValidator<LoginCredentialsDto>, LoginCredentialsDtoValidator>();
        services.AddScoped<IValidator<PasswordRecoveryDto>, PasswordRecoveryDtoValidator>();
        services.AddScoped<IValidator<PasswordRecoveryTokenDto>, PasswordRecoveryTokenDtoValidator>();
        services.AddScoped<IValidator<RegisterCredentialsDto>, RegisterCredentialsDtoValidator>();
        services.AddScoped<IValidator<ResendVerificationEmailDto>, ResendVerificationEmailDtoValidator>();
        services.AddScoped<IValidator<ResetPasswordDto>, ResetPasswordDtoValidator>();
    }
}
