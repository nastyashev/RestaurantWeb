using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurantSupportWebApp.Interfaces;
using RestaurantSupportWebApp.Models;
using RestaurantSupportWebApp.ViewModels;
using System.Text.Json;

namespace RestaurantSupportWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Staff> _signInManager;
        private readonly UserManager<Staff> _userManager;
        private readonly IScheduleRepository _scheduleRepository;

        public AccountController(SignInManager<Staff> signInManager, UserManager<Staff> userManager,
            IScheduleRepository scheduleRepository)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _scheduleRepository = scheduleRepository;
        }

        #region Register
        public IActionResult Login()
        {
            var model = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
                return View(loginViewModel);

            var user = await _userManager.FindByNameAsync(loginViewModel.Username);

            if (user != null)
            {
                var passwordCheck = await _signInManager.CheckPasswordSignInAsync(user, loginViewModel.Password, false);

                if (passwordCheck.Succeeded)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        var role = await _userManager.GetRolesAsync(user);

                        if (role[0] == "Hostess")
                            return RedirectToAction("Schedule", "Hostess");

                        if (role[0] == "Waiter")
                            return RedirectToAction("Schedule", "Waiter");

                        if (role[0] == "Chef")
                            return RedirectToAction("Schedule", "Chef");

                        if (role[0] == "Cook")
                            return RedirectToAction("Schedule", "Cook");

                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("", "Invalid Password");
                return View(loginViewModel);
            }

            ModelState.AddModelError("", "Invalid Username");
            return View(loginViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Schedule
        [HttpGet]
        public async Task<IActionResult> Schedule()
        {
            var user = await _userManager.GetUserAsync(User);
            var schedule = new Schedule();
            if (user != null)
            {
                schedule = await _scheduleRepository.GetByIdAsync(user.ScheduleId);
            }

            string jsonString = JsonSerializer.Serialize(schedule);
            return Ok(jsonString);
        }
        #endregion

        #region Policy

        #endregion
    }
}
