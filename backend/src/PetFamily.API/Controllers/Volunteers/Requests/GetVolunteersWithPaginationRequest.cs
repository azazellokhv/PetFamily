using PetFamily.Application.VolunteersManagement.Queries.GetVolunteersWithPagination;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record GetVolunteersWithPaginationRequest(
    string? LastName,
    int? WorkExperienceTo,
    int? WorkExperienceFrom,
    int Page,
    int PageSize)
{
    public GetFilteredVolunteersWithPaginationQuery ToQuery() =>
        new(LastName, WorkExperienceTo, WorkExperienceFrom, Page, PageSize);
}