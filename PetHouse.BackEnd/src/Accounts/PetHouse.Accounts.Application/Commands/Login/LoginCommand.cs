using PetHouse.Core.Abstraction;

namespace PetHouse.Accounts.Application.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand;
