using ASP_MVC_Movie.Models;
using ASP_MVC_Movie.Role;
using ASP_MVC_Movie.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP_MVC_Movie.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IWebHostEnvironment _environment;


        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.Name,
                    Email = model.Email,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (model.AvatarImage != null && model.AvatarImage.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_environment.WebRootPath, "avatars");
                        Directory.CreateDirectory(uploadsFolder);

                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.AvatarImage.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.AvatarImage.CopyToAsync(fileStream);
                        }

                        user.Avatar = uniqueFileName;
                        await _userManager.UpdateAsync(user);
                    }

                    await _userManager.AddToRoleAsync(user, ApplicationRoles.User);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Movie");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Wrong password");
                    return View(model);
                }

                return RedirectToAction("Index", "Movie");
            }
            return View(model);
        }

     
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Movie");
        }




        [HttpGet]
        public IActionResult ShowAllUsers()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }



        public IActionResult RemoveUser(string userId)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            if (user != null)
            {
                var result = _userManager.DeleteAsync(user).Result;
                if (result.Succeeded)
                {
                    return RedirectToAction("ShowAllUsers");
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                return NotFound();
            }
        }





        [HttpPost]
        public async Task<IActionResult> UserInfoEdit(EditUserVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            user.UserName = model.Name;

            if (model.AvatarImage != null && model.AvatarImage.Length > 0)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "avatars");
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.AvatarImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.AvatarImage.CopyToAsync(fileStream);
                }

                user.Avatar = uniqueFileName;
                await _userManager.UpdateAsync(user);
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            return RedirectToAction("Index", "Movie");
        }
























    }
}
