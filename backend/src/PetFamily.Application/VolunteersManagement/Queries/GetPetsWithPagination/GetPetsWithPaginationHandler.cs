using PetFamily.Application.Abstraction;
using PetFamily.Application.Database;
using PetFamily.Application.DTOs.Queries;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.VolunteersManagement.Queries.GetPetsWithPagination;

public class GetPetsWithPaginationHandler
    : IQueryHandler<PagedList<PetDto>, GetFilteredPetsWithPaginationQuery>
{
    private readonly IReadDbContext _readDbContext;


    public GetPetsWithPaginationHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PagedList<PetDto>> Handle(
        GetFilteredPetsWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var petQuery = _readDbContext.Pets;

        petQuery = petQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.Nickname),
            i => i.Nickname.Contains(query.Nickname!));

        petQuery = petQuery.WhereIf(
            query.PositionTo != null,
            i => i.Position <= query.PositionTo);

        petQuery = petQuery.WhereIf(
            query.PositionFrom != null,
            i => i.Position >= query.PositionFrom);

        return await petQuery
            .ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}