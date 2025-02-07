using System.Text.Json;
using DesafioBackEndManipulae.Models;

public class YouTubeService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private const string BaseUrl = "https://www.googleapis.com/youtube/v3/search";

    public YouTubeService(string apiKey)
    {
        _httpClient = new HttpClient();
        _apiKey = apiKey;
    }

    public async Task<List<Video>> BuscarVideosAsync(string query)
    {
        if (string.IsNullOrEmpty(query))
            throw new ArgumentException("A consulta (query) não pode ser vazia.");

        string url = $"{BaseUrl}?part=snippet&type=video&q={query}&key={_apiKey}&maxResults=10&regionCode=BR&publishedAfter=2022-01-01T00:00:00Z&publishedBefore=2022-12-31T23:59:59Z";

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(url);
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Falha na comunicação com a API do YouTube.", ex);
        }

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Erro ao acessar API do YouTube: {response.StatusCode}");
        }

        string jsonResponse = await response.Content.ReadAsStringAsync();
        return ConverterParaVideos(jsonResponse);
    }


    private List<Video> ConverterParaVideos(string jsonResponse)
    {
        using var doc = JsonDocument.Parse(jsonResponse);
        var root = doc.RootElement;
        var videos = new List<Video>();

        foreach (var item in root.GetProperty("items").EnumerateArray())
        {
            var snippet = item.GetProperty("snippet");

            videos.Add(new Video
            {
                Titulo = snippet.GetProperty("title").GetString(),
                Descricao = snippet.GetProperty("description").GetString(),
                Autor = snippet.GetProperty("channelTitle").GetString(),
                Url = $"https://www.youtube.com/watch?v={item.GetProperty("id").GetProperty("videoId").GetString()}",
                DataPublicacao = DateTime.Parse(snippet.GetProperty("publishedAt").GetString())
            });
        }

        return videos;
    }
}
