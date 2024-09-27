using PetHouse.Application.Abstraction;

namespace PetHouse.Infrastructure.Repositories.Commands.AddBreed;

public record AddBreedCommand(Guid Id, string Name) : ICommand;
