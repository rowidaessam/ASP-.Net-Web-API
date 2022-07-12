namespace FirstTestWithHelaly.Dtos
{
    public class MovieDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }

        [MaxLength(2500)]
        public string Storeline { get; set; }
        public IFormFile? Poster { get; set; }

        [ForeignKey("Genre")]
        public byte GenreId { get; set; }
    }
}
