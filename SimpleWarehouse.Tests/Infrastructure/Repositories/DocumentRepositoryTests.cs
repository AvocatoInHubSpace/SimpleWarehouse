using Microsoft.EntityFrameworkCore;
using SimpleWarehouse.Application;
using SimpleWarehouse.Core.Domain;
using SimpleWarehouse.Infrastructure.Data;
using SimpleWarehouse.Infrastructure.Repositories;
using Xunit;

namespace SimpleWarehouse.Tests.Infrastructure.Repositories;

public class DocumentRepositoryTests
{
    private readonly DbContextOptions<WarehouseDbContext> _dbContextOptions;
    private readonly List<Guid> _idList = [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()];

    public DocumentRepositoryTests()
    {
        var dbName = $"WarehouseDb_{DateTime.Now.ToFileTimeUtc()}";
        _dbContextOptions = new DbContextOptionsBuilder<WarehouseDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
    }

    [Fact]
    public async Task GetAsync_ShouldSuccess_WhenDocumentExists()
    {
        // Arrange
        var repository = await CreateRepositoryAsync();

        // Act
        var documentResult = await repository.GetAsync(_idList[0], CancellationToken.None);

        // Assert
        Assert.True(documentResult.IsSuccess);
        Assert.Equal(_idList[0], documentResult.Value!.Id);
    }

    
    [Fact]
    public async Task GetAllAsync_ShouldSuccess_WhenDocumentsExists()
    {
        // Arrange
        var repository = await CreateRepositoryAsync();

        // Act
        var documentResult = await repository.GetAllAsync(CancellationToken.None);

        // Assert
        Assert.True(documentResult.IsSuccess);
        var result = documentResult.Value!.ToList();
        Assert.Equal(_idList.Count, result.Count);
        for (var i = 0; i < _idList.Count; i++)
        {
            Assert.Equal(_idList[i], result[i].Id);
        }
    }

    [Fact]
    public async Task GetAsync_ShouldFailed_WhenDocumentNotExists()
    {
        // Arrange
        var repository = await CreateRepositoryAsync();

        // Act
        var documentResult = await repository.GetAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        Assert.False(documentResult.IsSuccess);
        Assert.Null(documentResult.Value);
    }

    [Fact]
    public async Task AddAsync_ShouldSuccess_WhenDocumentNotExists()
    {
        // Arrange
        var repository = await CreateRepositoryAsync();
        var document = new Document()
        {
            Id = Guid.NewGuid(),
            Number = Guid.NewGuid().ToString(),
            Date = DateTime.Now,
            ResourceSupplies = []
        };

        // Act
        var documentResult = await repository.AddAsync(document, CancellationToken.None);

        // Assert
        Assert.True(documentResult.IsSuccess);
    }

    // [Fact]
    // The methods 'ExecuteDelete' and 'ExecuteDeleteAsync' are not supported by the InMemory database provider.
    public async Task DeleteAsync_ShouldSuccess_WhenDocumentExists()
    {
        // Arrange
        var repository = await CreateRepositoryAsync();

        // Act
        var documentResult = await repository.DeleteAsync(_idList[0], CancellationToken.None);
        var getDocumentResult = await repository.GetAsync(_idList[0], CancellationToken.None);

        // Assert
        Assert.True(documentResult.IsSuccess);
        Assert.False(getDocumentResult.IsSuccess);
        Assert.Equal(RepositoryError.NotFound, getDocumentResult.Error);
    }

    [Fact]
    public async Task UpdateAsync_ShouldSuccess_WhenDocumentExists()
    {
        // Arrange
        var repository = await CreateRepositoryAsync();
        var document = new Document()
        {
            Id = _idList[0],
            Number = Guid.NewGuid().ToString(),
            Date = DateTime.Now,
            ResourceSupplies = []
        };
        
        // Act
        var documentResult = await repository.UpdateAsync(document, CancellationToken.None);
        var getDocumentResult = await repository.GetAsync(document.Id, CancellationToken.None);

        // Assert
        Assert.True(documentResult.IsSuccess);
        Assert.True(getDocumentResult.IsSuccess);
        Assert.Equal(document.Number, getDocumentResult.Value!.Number);
        Assert.Equal(document.Date, getDocumentResult.Value!.Date);
    }

    [Fact]
    public async Task CheckUniqueNameAsync_ShouldSuccess_WhenDocumentExists()
    {
        // Arrange
        var repository = await CreateRepositoryAsync();
        
        // Act
        var documentResult = await repository.CheckUniqueNameAsync(_idList[0].ToString(), CancellationToken.None);
        var getDocumentResult = await repository.GetAsync(_idList[0], CancellationToken.None);

        // Assert
        Assert.True(documentResult.IsSuccess);
        Assert.False(documentResult.Value);
        Assert.True(getDocumentResult.IsSuccess);
        Assert.Equal(getDocumentResult.Value!.Number, getDocumentResult.Value!.Number);
        Assert.Equal(getDocumentResult.Value!.Date, getDocumentResult.Value!.Date);
    }

    private async Task<DocumentRepository> CreateRepositoryAsync()
    {
        var context = new WarehouseDbContext(_dbContextOptions);
        await PopulateDataAsync(context);
        context.ChangeTracker.Clear();
        return new DocumentRepository(context);
    }

    private async Task PopulateDataAsync(WarehouseDbContext context)
    {
        foreach (var t in _idList)
        {
            var document = new Document()
            {
                Id = t,
                Number = t.ToString(),
                Date = DateTime.Now,
                ResourceSupplies = []
            };
            await context.Documents.AddAsync(document, CancellationToken.None);
        }
        
        await context.SaveChangesAsync(CancellationToken.None);
    }
}