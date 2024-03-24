using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_MVC_Movie.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public List<MovieGenre> MovieGenres { get; set; }
        public string Country { get; set; }
        [Range(0, 10, ErrorMessage = "The Rating must be between 0 and 10.")]
        public double Rating { get; set; }
        public string Director { get; set; }
        public string Description { get; set; }
        public int ReleaseDate { get; set; }
        public string? FilePath { get; set; }

        [Display(Name = "Upload Video")]
        [NotMapped]
        public IFormFile VideoFile { get; set; }
        public List<Comment>? Comments { get; set; }

    }
}
  