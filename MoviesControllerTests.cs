using Microsoft.AspNetCore.Mvc;
using MovieAggregator.Controllers;
using MovieAggregator.Models;
using MovieAggregator.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class MoviesControllerTests
{
    private readonly MoviesController _controller;
    private readonly IMovieRepository _repository;

    public MoviesControllerTests()
    {
        _repository = new FakeMovieRepository();
        _controller = new MoviesController(_repository);

        _repository.AddAsync(new Movie { Id = 1, Title = "Test Movie" }).Wait();
    }

    [Fact]
    public async Task GetMovies_ReturnsAllMovies()
    {
        var result = await _controller.GetMovies();
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var movies = Assert.IsType<List<Movie>>(okResult.Value);
        Assert.Single(movies);
    }

    [Fact]
    public async Task GetMovie_ReturnsMovie()
    {
        var result = await _controller.GetMovie(1);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var movie = Assert.IsType<Movie>(okResult.Value);
        Assert.Equal(1, movie.Id);
    }

    [Fact]
    public async Task PostMovie_CreatesMovie()
    {
        var movie = new Movie { Id = 2, Title = "New Movie" };
        var result = await _controller.PostMovie(movie);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdMovie = Assert.IsType<Movie>(createdAtActionResult.Value);
        Assert.Equal("New Movie", createdMovie.Title);
    }

    [Fact]
    public async Task PutMovie_UpdatesMovie()
    {
        var movie = new Movie { Id = 1, Title = "Updated Movie" };
        var result = await _controller.PutMovie(movie.Id, movie);
        Assert.IsType<NoContentResult>(result);

        var updatedMovie = await _repository.GetByIdAsync(movie.Id);
        Assert.Equal("Updated Movie", updatedMovie.Title);
    }

    [Fact]
    public async Task DeleteMovie_DeletesMovie()
    {
        var result = await _controller.DeleteMovie(1);
        Assert.IsType<NoContentResult>(result);
        var movie = await _repository.GetByIdAsync(1);
        Assert.Null(movie);
    }
}
