using Xunit;
using Moq;
using backend.Controllers;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

public class WatchlistsControllerTests
{
    private readonly Mock<IWatchlistRepository> _repoMock;
    private readonly WatchlistsController _controller;

    public WatchlistsControllerTests()
    {
        _repoMock = new Mock<IWatchlistRepository>();
        _controller = new WatchlistsController(_repoMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        // Arrange
        _repoMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Watchlist>());

        // Act
        var result = await _controller.GetAll();

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
}