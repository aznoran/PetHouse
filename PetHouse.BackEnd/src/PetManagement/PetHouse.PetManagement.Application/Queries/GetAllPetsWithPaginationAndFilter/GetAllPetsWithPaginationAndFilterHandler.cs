using System.Linq.Expressions;
using PetHouse.Core.Abstraction;
using PetHouse.Core.Dtos.PetManagment;
using PetHouse.Core.Extensions;
using PetHouse.Core.Models;

namespace PetHouse.PetManagement.Application.Queries.GetAllPetsWithPaginationAndFilter;

public class GetAllPetsWithPaginationAndFilterHandler : IQueryHandler<GetAllPetsWithPaginationAndFilterQuery, PagedList<PetDto>>
{
    // ReSharper disable once InconsistentNaming
    private const int MAXIMUM_AGE_DIFFERENCE = 5;
    
    private readonly IReadDbContext _readDbContext;

    public GetAllPetsWithPaginationAndFilterHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PagedList<PetDto>> Handle(GetAllPetsWithPaginationAndFilterQuery query,
        CancellationToken cancellationToken = default)
    {
        var queryPet = _readDbContext.Pets.AsQueryable();

        if (query.Name is not null)
        {
            queryPet = queryPet
                .Where(p => p.Name.ToUpper().StartsWith(query.Name.ToUpper()));
        }
        
        if (query.SpeciesId is not null)
        {
            queryPet = queryPet
                .Where(p => p.PetIdentifier.SpeciesId == query.SpeciesId);
        }
        
        if (query.BreedId is not null)
        {
            queryPet = queryPet
                .Where(p => p.PetIdentifier.BreedId == query.BreedId);
        }
        
        if (query.Color is not null)
        {
            queryPet = queryPet
                .Where(p => p.Color.ToUpper().StartsWith(query.Color.ToUpper()));
        }
        
        if (query.Weight is not null)
        {
            queryPet = queryPet
                .Where(p => p.Weight >= query.Weight);
        }
        
        if (query.Height is not null)
        {
            queryPet = queryPet
                .Where(p => p.Height >= query.Height);
        }
        
        if (query.IsCastrated is not null)
        {
            queryPet = queryPet
                .Where(p => p.IsCastrated == query.IsCastrated);
        }
        
        if (query.IsVaccinated is not null)
        {
            queryPet = queryPet
                .Where(p => p.IsVaccinated == query.IsVaccinated);
        }
        
        if (query.BirthdayDate is not null)
        {
            queryPet = queryPet
                .Where(p => 
                    Math.Abs(p.BirthdayDate.Year - query.BirthdayDate.Value.Year) < MAXIMUM_AGE_DIFFERENCE);
        }
        
        if (query.City is not null)
        {
            queryPet = queryPet
                .Where(p => p.City.ToUpper().StartsWith(query.City.ToUpper()));
        }
        
        if (query.Street is not null)
        {
            queryPet = queryPet
                .Where(p => p.Street.ToUpper().StartsWith(query.Street.ToUpper()));
        }
        
        if (query.Country is not null)
        {
            queryPet = queryPet
                .Where(p => p.Country.ToUpper().StartsWith(query.Country.ToUpper()));
        }

        if (query.PetStatus is not null)
        {
            queryPet = queryPet
                .Where(p => (int)p.PetStatus == query.PetStatus);
        }

        if (query.SortBy is not null)
        {
            Expression<Func<PetDto, object>> selector = query.SortBy.ToLower() switch
            {
                "name" => (dto => dto.Name),
                "position" => (dto => dto.Position),
                "species_id" => (dto => dto.PetIdentifier.SpeciesId),
                "breed_id" => (dto => dto.PetIdentifier.BreedId),
                "description" => (dto => dto.Description),
                "color" => (dto => dto.Color),
                "health_info" => (dto => dto.HealthInfo),
                "weight" => (dto => dto.Weight),
                "height" => (dto => dto.Height),
                "is_castrated" => (dto => dto.IsCastrated),
                "is_vaccinated" => (dto => dto.IsCastrated),
                "birthday_date" => (dto => dto.BirthdayDate),
                "city" => (dto => dto.City),
                "street" => (dto => dto.Street),
                "country" => (dto => dto.Country),
                "phone_number" => (dto => dto.PhoneNumber),
                "pet_status" => (dto => dto.PetStatus),
                _ => (dto => dto.Id)
            };

            if (query.SortDirection is not null)
            {
                if (query.SortDirection == "desc")
                {
                    queryPet = queryPet.OrderByDescending(selector);
                }
                else
                {
                    queryPet = queryPet.OrderBy(selector);
                }
            }
            else
            {
                queryPet = queryPet.OrderBy(selector);
            }
        }
        
        var pagedList = await queryPet.ToPagedList(query.Page, query.PageSize, cancellationToken);

        var newItems = new List<PetDto>();

        foreach (var pet in pagedList.Items)
        {
            if (pet.PetPhotos != null)
                pet.PetPhotos = pet.PetPhotos.OrderByDescending(photo => photo.IsPhotoMain).ToList();
            newItems.Add(pet);
        }

        return new PagedList<PetDto>
        {
            Items = newItems,
            Page = pagedList.Page,
            PageSize = pagedList.PageSize,
            TotalCount = pagedList.TotalCount
        };
    }
}


