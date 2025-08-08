using Microsoft.EntityFrameworkCore;
using SimpleWarehouse.Application;
using SimpleWarehouse.Core.Domain;
using SimpleWarehouse.Infrastructure.Data;
using SimpleWarehouse.Infrastructure.Repositories;
using Xunit;

namespace SimpleWarehouse.Tests.Infrastructure.Repositories;

public class ResourceRepositoryTests
{
    private readonly DbContextOptions<WarehouseDbContext> _dbContextOptions;
    private readonly List<Guid> _idList;

    public ResourceRepositoryTests()
    {
        _idList = new List<Guid>();
        for (var i = 0; i < 5; i++)
        {
            _idList.Add(Guid.NewGuid());
        }
        var dbName = $"WarehouseDb_{DateTime.Now.ToFileTimeUtc()}";
        _dbContextOptions = new DbContextOptionsBuilder<WarehouseDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
    }

    [Fact]
    public async Task GetAsync_ShouldSuccess_WhenResourceExists()
    {
        // Arrange
        var repository = await CreateRepositoryAsync();

        // Act
        var resourceResult = await repository.GetAsync(_idList[0], CancellationToken.None);

        // Assert
        Assert.True(resourceResult.IsSuccess);
        Assert.Equal(_idList[0], resourceResult.Value!.Id);
    }

    [Fact]
    public async Task GetAsync_ShouldFailed_WhenResourceNotExists()
    {
        // Arrange
        var repository = await CreateRepositoryAsync();

        // Act
        var resourceResult = await repository.GetAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        Assert.False(resourceResult.IsSuccess);
        Assert.Null(resourceResult.Value);
    }

    [Fact]
    public async Task GetAllAsync_ShouldSuccess_WhenResourcesExists()
    {
        // Arrange
        var repository = await CreateRepositoryAsync();

        // Act
        var resourceResult = await repository.GetAllAsync(CancellationToken.None);

        // Assert
        Assert.True(resourceResult.IsSuccess);
        var result = resourceResult.Value!.ToList();
        Assert.Equal(_idList.Count, result.Count);
        for (var i = 0; i < _idList.Count; i++)
        {
            Assert.Equal(_idList[i], result[i].Id);
        }
    }

    [Fact]
    public async Task AddAsync_ShouldSuccess_WhenResourceNotExists()
    {
        // Arrange
        var repository = await CreateRepositoryAsync();
        var resource = new Resource()
        {
            Id = Guid.NewGuid(),
            Name = Guid.NewGuid().ToString(),
        };

        // Act
        var resourceResult = await repository.AddAsync(resource, CancellationToken.None);

        // Assert
        Assert.True(resourceResult.IsSuccess);
    }

    // [Fact]
    public async Task DeleteAsync_ShouldSuccess_WhenResourceExists()
    {
        // Arrange
        var repository = await CreateRepositoryAsync();

        // Act
        var resourceResult = await repository.DeleteAsync(_idList[0], CancellationToken.None);
        var getResourceResult = await repository.GetAsync(_idList[0], CancellationToken.None);

        // Assert
        Assert.True(resourceResult.IsSuccess);
        Assert.False(getResourceResult.IsSuccess);
        Assert.Equal(RepositoryError.NotFound, getResourceResult.Error);
    }

    [Fact]
    public async Task UpdateAsync_ShouldSuccess_WhenResourceExists()
    {
        // Arrange
        var repository = await CreateRepositoryAsync();
        var resource = new Resource()
        {
            Id = _idList[0],
            Name = Guid.NewGuid().ToString(),
        };

        // Act
        var resourceResult = await repository.UpdateAsync(resource, CancellationToken.None);
        var getResourceResult = await repository.GetAsync(resource.Id, CancellationToken.None);

        // Assert
        Assert.True(resourceResult.IsSuccess);
        Assert.True(getResourceResult.IsSuccess);
        Assert.Equal(resource.Name, getResourceResult.Value!.Name);
    }

    [Fact]
    public async Task UpdateAsync_ShouldFailed_WhenResourceNotExists()
    {
        // Arrange
        var repository = await CreateRepositoryAsync();
        var resource = new Resource()
        {
            Id = Guid.NewGuid(),
            Name = Guid.NewGuid().ToString(),
        };

        // Act
        var resourceResult = await repository.UpdateAsync(resource, CancellationToken.None);

        // Assert
        Assert.False(resourceResult.IsSuccess);
    }

    [Fact]
    public async Task CheckUniqueNameAsync_ShouldSuccess_WhenResourceExists()
    {
        // Arrange
        var repository = await CreateRepositoryAsync();

        // Act
        var resourceResult = await repository.CheckUniqueNameAsync(_idList[0].ToString(), CancellationToken.None);
        var getResourceResult = await repository.GetAsync(_idList[0], CancellationToken.None);

        // Assert
        Assert.True(resourceResult.IsSuccess);
        Assert.False(resourceResult.Value);
        Assert.True(getResourceResult.IsSuccess);
        Assert.Equal(getResourceResult.Value!.Name, getResourceResult.Value!.Name);
    }

    private async Task<ResourceRepository> CreateRepositoryAsync()
    {
        var context = new WarehouseDbContext(_dbContextOptions);
        await PopulateDataAsync(context);
        context.ChangeTracker.Clear();
        return new ResourceRepository(context);
    }

    private async Task PopulateDataAsync(WarehouseDbContext context)
    {
        foreach (var t in _idList)
        {
            var resource = new Resource()
            {
                Id = t,
                Name = t.ToString(),
            };
            await context.Resources.AddAsync(resource, CancellationToken.None);
        }

        await context.SaveChangesAsync(CancellationToken.None);
    }
}