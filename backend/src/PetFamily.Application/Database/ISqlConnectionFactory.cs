using System.Data;

namespace PetFamily.Application.Database;

public interface ISqlConnectionFactory
{
    public IDbConnection Create();
}