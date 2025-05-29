using Dapper;
using PetFamily.Application.Abstraction;
using PetFamily.Application.Database;
using PetFamily.Application.DTOs.Queries;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.VolunteersManagement.Queries.GetVolunteersWithPagination;

public class GetVolunteersWithPaginationHandler
    : IQueryHandler<PagedList<VolunteerDto>, GetFilteredVolunteersWithPaginationQuery>
{
    private readonly IReadDbContext _readDbContext;


    public GetVolunteersWithPaginationHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PagedList<VolunteerDto>> Handle(
        GetFilteredVolunteersWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var volunteerQuery = _readDbContext.Volunteers;

        volunteerQuery = volunteerQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.LastName),
            i => i.LastName.Contains(query.LastName!));

        volunteerQuery = volunteerQuery.WhereIf(
            query.WorkExperienceTo != null,
            i => i.WorkExperience <= query.WorkExperienceTo);

        volunteerQuery = volunteerQuery.WhereIf(
            query.WorkExperienceFrom != null,
            i => i.WorkExperience >= query.WorkExperienceFrom);

        return await volunteerQuery
            .ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}

public class GetVolunteersWithPaginationHandlerDapper
    : IQueryHandler<PagedList<VolunteerDto>, GetFilteredVolunteersWithPaginationQuery>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetVolunteersWithPaginationHandlerDapper(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<PagedList<VolunteerDto>> Handle(
        GetFilteredVolunteersWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.Create();

        var sql = $"""
                      SELECT id, description_value, full_name_last_name FROM volunteers
                      ORDER BY work_experience_value LIMIT @PageSize OFFSET @Offset
                   """;
        var parameters = new DynamicParameters();

        var totalCount = await connection.ExecuteScalarAsync<long>(
            "SELECT COUNT(*) FROM volunteers");
        
        parameters.Add("@PageSize", query.PageSize);
        parameters.Add("@Offset", (query.Page - 1) * query.PageSize);
        
        var volunteer = await connection.QueryAsync<VolunteerDto>(sql, parameters);

        return new PagedList<VolunteerDto>()
        {
            Items = volunteer.ToList(),
            TotalCount = totalCount,
            PageSize = query.PageSize,
            Page = query.Page,
            
        };

    }
}