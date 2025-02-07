using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DesafioBackEndManipulae.Models;

public class VideoService
{
    private readonly IVideoRepository _videoRepository;

    public VideoService(IVideoRepository videoRepository)
    {
        _videoRepository = videoRepository;
    }

    public Task<List<Video>> GetAllAsync() => _videoRepository.GetAllAsync();

    public async Task<Video> GetByIdAsync(int id)
    {
        var video = await _videoRepository.GetByIdAsync(id);
        if (video == null || video.Excluido)
            throw new KeyNotFoundException($"Vídeo com ID {id} não encontrado.");

        return video;
    }

    public async Task AddAsync(Video video)
    {
        if (string.IsNullOrEmpty(video.Titulo))
            throw new ArgumentException("O título do vídeo não pode ser vazio.");

        await _videoRepository.AddAsync(video);
    }

    public async Task UpdateAsync(Video video)
    {
        var existingVideo = await _videoRepository.GetByIdAsync(video.Id);
        if (existingVideo == null)
            throw new KeyNotFoundException($"Vídeo com ID {video.Id} não encontrado.");

        await _videoRepository.UpdateAsync(video);
    }

    public async Task DeleteAsync(int id)
    {
        var video = await _videoRepository.GetByIdAsync(id);
        if (video == null)
            throw new KeyNotFoundException($"Vídeo com ID {id} não encontrado.");

        await _videoRepository.DeleteAsync(id);
    }


    // 🔍 Método de filtragem avançada
    public async Task<List<Video>> FiltrarVideosAsync(
        string titulo,
        string? autor,
        TimeSpan? minDuracao,
        TimeSpan? maxDuracao,
        DateTime? dataMin,
        string q)
    {
        var videos = await _videoRepository.GetAllAsync();

        if (!string.IsNullOrEmpty(titulo))
            videos = videos.Where(v => v.Titulo.Contains(titulo, StringComparison.OrdinalIgnoreCase)).ToList();

        if (!string.IsNullOrEmpty(autor))
            videos = videos.Where(v => v.Autor.Contains(autor, StringComparison.OrdinalIgnoreCase)).ToList();

        if (minDuracao.HasValue)
            videos = videos.Where(v => v.Duracao >= minDuracao.Value).ToList();

        if (maxDuracao.HasValue)
            videos = videos.Where(v => v.Duracao <= maxDuracao.Value).ToList();

        if (dataMin.HasValue)
            videos = videos.Where(v => v.DataPublicacao >= dataMin.Value).ToList();

        if (!string.IsNullOrEmpty(q))
            videos = videos.Where(v => v.Titulo.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                                       v.Descricao.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                                       v.Autor.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();

        return videos.Where(v => !v.Excluido).ToList();
    }
}

