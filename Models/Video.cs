namespace DesafioBackEndManipulae.Models
{
    public class Video
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Autor { get; set; }
        public string Url { get; set; }
        public TimeSpan Duracao { get; set; }
        public DateTime DataPublicacao { get; set; }
        public bool Excluido { get; set; } = false;
    }
}
