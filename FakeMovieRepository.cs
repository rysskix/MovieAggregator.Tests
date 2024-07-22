using MovieAggregator.Models;
using MovieAggregator.Repositories;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

public class FakeMovieRepository : IMovieRepository
{
    private readonly List<Movie> _movies = new List<Movie>();

    public Task AddAsync(Movie entity)
    {
        _movies.Add(entity);
        return Task.CompletedTask;
    }

    public Task AddRangeAsync(IEnumerable<Movie> entities)
    {
        _movies.AddRange(entities);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Movie>> FindAsync(Expression<Func<Movie, bool>> predicate)
    {
        return Task.FromResult(_movies.AsQueryable().Where(predicate).AsEnumerable());
    }

    public Task<IEnumerable<Movie>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Movie>>(_movies);
    }

    public Task<Movie> GetByIdAsync(int id)
    {
        return Task.FromResult(_movies.SingleOrDefault(m => m.Id == id));
    }

    public Task<Movie> GetMovieWithActorsAsync(int id)
    {
        return Task.FromResult(_movies.SingleOrDefault(m => m.Id == id));
    }

    public Task RemoveAsync(Movie entity)
    {
        _movies.Remove(entity);
        return Task.CompletedTask;
    }

    public Task RemoveRangeAsync(IEnumerable<Movie> entities)
    {
        foreach (var entity in entities)
        {
            _movies.Remove(entity);
        }
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Movie entity)
    {
        var movie = _movies.SingleOrDefault(m => m.Id == entity.Id);
        if (movie != null)
        {
            movie.Title = entity.Title;
        }
        return Task.CompletedTask;
    }
}
