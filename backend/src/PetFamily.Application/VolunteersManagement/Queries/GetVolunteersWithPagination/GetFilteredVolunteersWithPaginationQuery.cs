using PetFamily.Application.Abstraction;

namespace PetFamily.Application.VolunteersManagement.Queries.GetVolunteersWithPagination;

public record GetFilteredVolunteersWithPaginationQuery(
    string? LastName,
    int? WorkExperienceTo,
    int? WorkExperienceFrom,
    int Page, 
    int PageSize) : IQuery;