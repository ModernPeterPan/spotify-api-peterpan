using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;

namespace SpotifyWrapper.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpotifyController : ControllerBase
{
    private readonly SpotifyClient _spotify;

    public SpotifyController(SpotifyClient spotify)
    {
        _spotify = spotify;
    }

    [HttpGet("artist/{name}")]
    public async Task<IActionResult> SearchArtist(string name)
    {
        var results = await _spotify.Search.Item(new SearchRequest(
            SearchRequest.Types.Artist, name));

        var artist = results.Artists?.Items?.FirstOrDefault();
        if (artist == null) return NotFound();

        return Ok(new
        {
            artist.Name,
            artist.Genres,
            ImageUrl = artist.Images?.FirstOrDefault()?.Url,
            SpotifyUrl = artist.ExternalUrls?.GetValueOrDefault("spotify")
        });
    }
}