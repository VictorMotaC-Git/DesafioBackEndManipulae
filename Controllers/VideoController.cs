using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DesafioBackEndManipulae.Models;


[Route("api/[controller]")]
[ApiController]
[Authorize]
public class VideoController : ControllerBase
{
    private readonly VideoService _videoService;

    public VideoController(VideoService videoService)
    {
        _videoService = videoService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Video>>> GetVideos()
    {
        return await _videoService.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Video>> GetVideo(int id)
    {
        var video = await _videoService.GetByIdAsync(id);
        if (video == null) return NotFound();
        return video;
    }

    [HttpPost]
    public async Task<ActionResult> AddVideo([FromBody] Video video)
    {
        await _videoService.AddAsync(video);
        return CreatedAtAction(nameof(GetVideo), new { id = video.Id }, video);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateVideo(int id, [FromBody] Video video)
    {
        if (id != video.Id) return BadRequest();
        await _videoService.UpdateAsync(video);
        return NoContent();
    }

    // 🔒 Somente ADMIN pode excluir vídeos
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteVideo(int id)
    {
        await _videoService.DeleteAsync(id);
        return NoContent();
    }

    // Qualquer usuário autenticado pode visualizar vídeos
    [HttpGet("filtrar")]
    [Authorize]
    public async Task<ActionResult<List<Video>>> FiltrarVideos(
        [FromQuery] string titulo,
        [FromQuery] string autor,
        [FromQuery] int? minDuracao,
        [FromQuery] int? maxDuracao,
        [FromQuery] DateTime? dataMin,
        [FromQuery] string q)
    {
        var duracaoMin = minDuracao.HasValue ? TimeSpan.FromMinutes(minDuracao.Value) : (TimeSpan?)null;
        var duracaoMax = maxDuracao.HasValue ? TimeSpan.FromMinutes(maxDuracao.Value) : (TimeSpan?)null;

        var videos = await _videoService.FiltrarVideosAsync(titulo, autor, duracaoMin, duracaoMax, dataMin, q);

        if (videos.Count == 0)
            return NotFound("Nenhum vídeo encontrado com os critérios informados.");

        return Ok(videos);
    }
}
