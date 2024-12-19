using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using CMS.Data;
using CMS.Enums;
using CMS.Models;

public class CustomUserManager : UserManager<IdentityUser>
{
    private readonly ApplicationDbContext _context;

    public CustomUserManager(
        IUserStore<IdentityUser> store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<IdentityUser> passwordHasher,
        IEnumerable<IUserValidator<IdentityUser>> userValidators,
        IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManager<IdentityUser>> logger,
        ApplicationDbContext context)
        : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
        _context = context;
    }

    // Nadpisanie metody CreateAsync
    public override async Task<IdentityResult> CreateAsync(IdentityUser user, string password)
    {
        // Wywołanie podstawowej logiki tworzenia użytkownika
        var result = await base.CreateAsync(user, password);

        if (result.Succeeded)
        {
            // Dodaj logikę tworzenia wpisu w modelu UserModel
            var userModel = new UserModel
            {
                IdentityUserId = user.Id,
                IdentityUser = user,
                CreatedAt = DateTime.UtcNow,
                Role = UserRolesEnum.Reader
            };

            // Dodanie do bazy danych Twojej logiki
            _context.UserModel.Add(userModel);
            await _context.SaveChangesAsync();
        }

        return result; // Zwróć wynik podstawowej metody
    }
}