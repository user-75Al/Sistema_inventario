using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace UtmMarket.Infrastructure.Data;

/// <summary>
/// Define el contrato para la creación de conexiones a la base de datos.
/// </summary>
public interface IDbConnectionFactory
{
    ValueTask<IDbConnection> CreateConnectionAsync(CancellationToken ct = default);
}
