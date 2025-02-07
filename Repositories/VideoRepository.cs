using Microsoft.EntityFrameworkCore;
using DesafioBackEndManipulae.Models;
using DesafioBackEndManipulae.Data;

public class VideoRepository : IVideoRepository
{
    private readonly AppDbContext _context;

    public VideoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Video>> GetAllAsync()
    {
        return await _context.Videos.Where(v => !v.Excluido).ToListAsync();
    }

    public async Task<Video> GetByIdAsync(int id)
    {
        return await _context.Videos.FindAsync(id);
    }

    public async Task AddAsync(Video video)
    {
        _context.Videos.Add(video);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Video video)
    {
        _context.Videos.Update(video);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var video = await _context.Videos.FindAsync(id);
        if (video != null)
        {
            video.Excluido = true;
            await _context.SaveChangesAsync();
        }
    }
}
