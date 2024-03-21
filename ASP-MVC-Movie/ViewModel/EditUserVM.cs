using System.ComponentModel.DataAnnotations;

namespace ASP_MVC_Movie.ViewModel
{
    public class EditUserVM
    {

        public int Id { get; set; }

        public string? Name { get; set; }

        [Display(Name = "Avatar")]
        [DataType(DataType.Upload)]
        public IFormFile? AvatarImage { get; set; }


    }
}
