using PetHouse.Core.Abstraction;

namespace PetHouse.Accounts.Application.Commands.Refresh;

public record RefreshCommand(Guid RefreshToken) : ICommand;
