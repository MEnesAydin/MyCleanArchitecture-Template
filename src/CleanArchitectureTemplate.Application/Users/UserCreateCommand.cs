using CleanArchitectureTemplate.Application.Common;
using CleanArhictecture_2025.Domain.Users;
using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureTemplate.Application.Users;

public sealed record UserCreateCommand(
    string Email,
    string UserName,
    string Password,
    string FirstName,
    string LastName,
    string PhoneNumber) : IRequest<Result<string>>;

public sealed class RegisterCommandValidator : AbstractValidator<UserCreateCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email boş olamaz")
            .EmailAddress().WithMessage("Hatalı email düzeni");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre boş olamaz")
            .MinimumLength(6).WithMessage("Şifre 6 karakterden uzun olmalı");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("İlk isim alanı boş olamaz")
            .MaximumLength(50).WithMessage("İlk isim alanı 50 karakterden fazla olamaz");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Soyad alanı boş olamaz")
            .MaximumLength(50).WithMessage("Soyad alanı 50 karakterden fazla olamaz");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Telefon alanı boş olamaz")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Hatalı telefon düzeni");
    }
}

internal sealed class UserCreateCommandHandler(
    UserManager<AppUser> userManager,
    IUnitOfWork unitOfWork) : IRequestHandler<UserCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        AppUser? user = await userManager.Users.FirstOrDefaultAsync(p => p.Email == request.Email || p.UserName == request.UserName, cancellationToken);

        if (user is not null)
        {
            return Result<string>.Failure("Mail adresi veya kullanıcı adı zaten kayıtlı");
        }

        AppUser newUser = request.Adapt<AppUser>();

        var createResult = await userManager.CreateAsync(newUser, request.Password);
        if (!createResult.Succeeded)
        {
            string errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
            return Result<string>.Failure(errors);
        }
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Kayıt başarılı";

    }
}

