using PetHouse.Core.Abstraction;

namespace PetHouse.Accounts.Application.Commands.Register;

public record RegisterCommand(string Email, string Password) : ICommand;
