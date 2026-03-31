using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;

namespace UtmMarket.Core.Repositories;

/// <summary>
/// Criteria for filtering sales transactions.
/// Optimized for Native AOT using C# 14 features.
/// </summary>
public record SaleFilter
{
    public string? Folio { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public SaleStatus? Status { get; init; }

    public decimal? MinTotal 
    { 
        get => field; 
        init => field = value >= 0 ? value : throw new ArgumentException("MinTotal cannot be negative."); 
    }
}

/// <summary>
/// Defines the persistence contract for the Sale Aggregate Root.
/// Pure domain abstraction designed for high-performance .NET 10 environments.
/// </summary>
public interface ISaleRepository
{
    /// <summary>
    /// Streams all sales from the database asynchronously.
    /// Utilizes .NET 10 AsyncEnumerable improvements for zero-allocation streaming.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An asynchronous stream of Sale domain objects.</returns>
    IAsyncEnumerable<Sale> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single sale by its unique identifier including its details.
    /// </summary>
    /// <param name="id">The unique identifier of the sale.</param>
    /// <param name="transaction">Optional transaction context.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The Sale aggregate if found; otherwise, null.</returns>
    Task<Sale?> GetByIdAsync(int id, IDbTransaction? transaction = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds sales matching the specified criteria without using dynamic expressions.
    /// </summary>
    /// <param name="filter">The filtering criteria.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A filtered asynchronous stream of Sale domain objects.</returns>
    IAsyncEnumerable<Sale> FindAsync(SaleFilter filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists a new sale and its details into the database.
    /// </summary>
    /// <param name="sale">The domain aggregate to persist.</param>
    /// <param name="transaction">Optional transaction context.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The persisted sale with its generated identity.</returns>
    Task<Sale> AddAsync(Sale sale, IDbTransaction? transaction = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing sale header and manages the replacement/update of its details.
    /// </summary>
    /// <param name="sale">The domain aggregate with updated information.</param>
    /// <param name="transaction">Optional transaction context.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task UpdateAsync(Sale sale, IDbTransaction? transaction = null, CancellationToken cancellationToken = default);
}
