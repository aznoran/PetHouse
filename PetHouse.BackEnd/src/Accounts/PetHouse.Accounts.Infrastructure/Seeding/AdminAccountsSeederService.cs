using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetHouse.Accounts.Application;
using PetHouse.Accounts.Domain.Models;
using PetHouse.Accounts.Infrastructure.Managers;
using PetHouse.Accounts.Infrastructure.Options;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.Accounts.Infrastructure.Seeding;

public class AdminAccountsSeederService
{
    private readonly AdminOptions _adminOptions;
    private readonly PermissionManager _permissionManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly RolePermissionManager _rolePermissionManager;
    private readonly ILogger<AdminAccountsSeederService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;
    private readonly IAccountManager _accountManager;

    public AdminAccountsSeederService(PermissionManager permissionManager,
        RoleManager<Role> roleManager,
        RolePermissionManager rolePermissionManager,
        ILogger<AdminAccountsSeederService> logger,
        IUnitOfWork unitOfWork,
        IOptions<AdminOptions> adminOptions,
        UserManager<User> userManager,
        IAccountManager accountManager)
    {
        _permissionManager = permissionManager;
        _roleManager = roleManager;
        _rolePermissionManager = rolePermissionManager;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _accountManager = accountManager;
        _adminOptions = adminOptions.Value;
    }

    public async Task SeedAsync()
    {
        var json = await File.ReadAllTextAsync("etc/accounts.json");

        var res = JsonSerializer.Deserialize<RolePermissionOptions>(json)
                  ?? throw new ApplicationException("Could not deserialize role permission config.");

        _logger.LogInformation("started seeding...");

        await SeedPermissions(res);
        await SeedRoles(res);
        await SeedRolePermissions(res);
        await SeedAdmin();
        
        _logger.LogInformation("seeding completed successfully");
    }

    private async Task SeedPermissions(RolePermissionOptions options)
    {
        _logger.LogInformation("started permissions seeding...");
        foreach (var permissionTemp in options.Permissions.Values)
        {
            foreach (var permission in permissionTemp)
            {
                await _permissionManager.AddPermissionIfNotExists(permission);
            }
        }

        _logger.LogInformation("permissions seeding completed");
        await _unitOfWork.SaveChanges();
    }

    private async Task SeedRoles(RolePermissionOptions options)
    {
        _logger.LogInformation("started roles seeding...");
        foreach (var role in options.Roles.Keys)
        {
            var res = await _roleManager.FindByNameAsync(role);

            if (res is null)
                await _roleManager.CreateAsync(new Role(){Name = role});
        }

        _logger.LogInformation("roles seeding completed");
        await _unitOfWork.SaveChanges();
    }

    private async Task SeedRolePermissions(RolePermissionOptions options)
    {
        _logger.LogInformation("started role_permissions seeding...");
        foreach (var role in options.Roles.Keys)
        {
            await _rolePermissionManager.AddRolePermissionsIfNotExists(role, options.Roles[role]);
        }

        _logger.LogInformation("role_permissions seeding completed");
        await _unitOfWork.SaveChanges();
    }
    
    private async Task SeedAdmin()
    {
        _logger.LogInformation("started admin seeding...");
        var adminRole = await _roleManager.FindByNameAsync(AdminAccount.ADMIN)
                        ?? throw new ApplicationException("Could not find admin role.");
        
        var user = User.CreateAdmin(_adminOptions.Username, _adminOptions.Email, adminRole);

        if (user.IsFailure)
        {
            throw new ArgumentException(user.Error.Message);
        }

        await _userManager.CreateAsync(user.Value, _adminOptions.Password);

        var fullName = FullName.Create(_adminOptions.Username, _adminOptions.Username).Value;

        var adminAccount = new AdminAccount()
            { FullName = fullName, User = user.Value, Id = Guid.NewGuid() };

        await _accountManager.AddAdminAccount(adminAccount);

        _logger.LogInformation("admin seeding completed");
        await _unitOfWork.SaveChanges();
    }
}