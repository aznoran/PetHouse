using Microsoft.EntityFrameworkCore;
using PetHouse.Accounts.Contracts.Dtos;
using PetHouse.Core.Abstraction;
using PetHouse.Core.Dtos.PetManagment;

namespace PetHouse.Accounts.Application.Queries.GetAccountById;

public class GetAccountByIdHandler : IQueryHandler<GetAccountByIdQuery, UserDto?>
{
    private readonly IReadDbContext _readDbContext;

    public GetAccountByIdHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<UserDto?> Handle(GetAccountByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var user = await _readDbContext.Users
            .Include(u => u.Roles)
            .Include(u => u.AdminAccount)
            .Include(u => u.VolunteerAccount)
            .Include(u => u.ParticipantAccount)
            .FirstOrDefaultAsync(u => u.Id == query.UserId, cancellationToken);

        if (user is null)
        {
            return null;
        }
        
        var roles = user.Roles is null ? [] : user.Roles.Select(r => new RoleDto(){Id = r.Id, Name = r.Name});
        
        var socialNetworks = user.SocialNetworks is null ? [] : user.SocialNetworks.Select(r => new SocialNetworksDto(r.Link, r.Name));
        
        var adminAccount = user.AdminAccount is not null ? new AdminAccountDto(){
            Id = user.AdminAccount.Id,
            UserId = user.Id,
            Name = user.AdminAccount.Name,
            Surname = user.AdminAccount.Surname
        } : null;

        var volunteerAccount = user.VolunteerAccount is not null ? new VolunteerAccountDto()
        {
            Id = user.VolunteerAccount.Id,
            UserId = user.Id,
            Name = user.VolunteerAccount.Name,
            Surname = user.VolunteerAccount.Surname,
            Requisites = user.VolunteerAccount.Requisites is null
                ? []
                : user.VolunteerAccount.Requisites.Select(r => new RequisiteDto(r.Name, r.Description)).ToList(),
            Certificates = user.VolunteerAccount.Certificates is null ? [] : user.VolunteerAccount.Certificates
        } : null;

        var participantAccount = user.ParticipantAccount is not null ? new ParticipantAccountDto()
        {
            Id = user.ParticipantAccount.Id,
            UserId = user.Id,
            Name = user.ParticipantAccount.Name,
            Surname = user.ParticipantAccount.Surname
        } : null;
        
        var userDto = new UserDto()
        {
            Id = user.Id,
            UserName = user.UserName,
            Roles = roles.ToList(),
            SocialNetworks = socialNetworks.ToList(),
            AdminAccount = adminAccount,
            ParticipantAccount = participantAccount,
            VolunteerAccount = volunteerAccount
        };

        return userDto;
    }
}