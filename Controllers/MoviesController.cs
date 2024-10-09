using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoPizza.Data;
using ContosoPizza.Models;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly MovieContext _context;

    public MoviesController(MovieContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetMovies()
    {
        return Ok(await _context.Movie.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMovie(int id)
    {
        var movie = await _context.Movie.FindAsync(id);

        if (movie == null)
        {
            return NotFound();
        }

        return Ok(movie);
    }

    // POST: api/MoviesApi
    [HttpPost]
    public async Task<ActionResult<Movie>> CreateMovie(Movie movie)
    {
        if (movie == null)
        {
            return BadRequest();
        }

        _context.Movie.Add(movie);
        await _context.SaveChangesAsync();

        // Returns a 201 Created response with the newly created movie's route
        return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
    }

    // PUT: api/MoviesApi/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMovie(int id, Movie movie)
    {
        if (id != movie.Id)
        {
            return BadRequest();
        }

        _context.Entry(movie).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MovieExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent(); // Return 204 No Content for successful update
    }

    // DELETE: api/MoviesApi/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        var movie = await _context.Movie.FindAsync(id);
        if (movie == null)
        {
            return NotFound();
        }

        _context.Movie.Remove(movie);
        await _context.SaveChangesAsync();

        return NoContent(); // Return 204 No Content for successful deletion
    }

    private bool MovieExists(int id)
    {
        return _context.Movie.Any(e => e.Id == id);
    }


}