using System.ComponentModel.DataAnnotations;

namespace ASP_MVC_Movie.Models
{
    public class MovieGenre
    {
        [Key]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
