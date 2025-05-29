using PetFamily.Application.VolunteersManagement.Queries.GetPetsWithPagination;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record GetPetsWithPaginationRequest(
    string? Nickname,
    int? PositionTo,
    int? PositionFrom,
    int Page,
    int PageSize)
{
    public GetFilteredPetsWithPaginationQuery ToQuery() =>
        new(Nickname, PositionTo, PositionFrom, Page, PageSize);
}