using System.ComponentModel.DataAnnotations;

namespace ASP_MVC_Movie.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Genre name required")]
        public string Name { get; set; }
        public List<MovieGenre> MovieGenres { get; set; }


    }
}
