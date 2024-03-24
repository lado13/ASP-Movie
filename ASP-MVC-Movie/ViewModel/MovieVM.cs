using System.ComponentModel.DataAnnotations;

namespace ASP_MVC_Movie.ViewModel
{
    public class MovieVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Title field is required.")]
        public string Title { get; set; }

        public List<int>? SelectedGenreIds { get; set; }

        public List<GenreVM>? AvailableGenres { get; set; }

        public string Country { get; set; }

        [Range(0, 10, ErrorMessage = "The Rating must be between 0 and 10.")]
        public double Rating { get; set; }

        public string Director { get; set; }

        public string Description { get; set; }

        [Display(Name = "Release Date")]
        public int ReleaseDate { get; set; }

        [Display(Name = "Upload Video")]
        public IFormFile? VideoFile { get; set; }
     
    }
}
