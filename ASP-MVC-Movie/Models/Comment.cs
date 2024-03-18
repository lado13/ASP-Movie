namespace ASP_MVC_Movie.Models
{
    public class Comment
    {


        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime PostedAt { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }



    }
}
