using PetFamily.Application.Abstraction;

namespace PetFamily.Application.VolunteersManagement.Queries.GetPetsWithPagination;

public record GetFilteredPetsWithPaginationQuery(
    string? Nickname,
    int? PositionTo,
    int? PositionFrom,
    int Page, 
    int PageSize) : IQuery;