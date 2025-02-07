using Microsoft.AspNetCore.Mvc;
using DesafioBackEndManipulae.Models;

[Route("api/[controller]")]
[ApiController]
public class YouTubeController : ControllerBase
{
    private readonly YouTubeService _youTubeService;
    private readonly VideoService _videoService;

    public YouTubeController(YouTubeService youTubeService, VideoService videoService)
    {
        _youTubeService = youTubeService;
        _videoService = videoService;
    }

    [HttpGet("buscar")]
    public async Task<ActionResult<List<Video>>> BuscarVideos([FromQuery] string query)
    {
        var videos = await _youTubeService.BuscarVideosAsync(query);
        return Ok(videos);
    }

    [HttpPost("salvar")]
    public async Task<ActionResult> SalvarVideos([FromQuery] string query)
    {
        var videos = await _youTubeService.BuscarVideosAsync(query);

        foreach (var video in videos)
        {
            await _videoService.AddAsync(video);
        }

        return Ok(new { mensagem = "Vídeos salvos com sucesso!", quantidade = videos.Count });
    }
}
