using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurantSupportWebApp.Interfaces;
using RestaurantSupportWebApp.Models;
using RestaurantSupportWebApp.ViewModels;

namespace RestaurantSupportWebApp.Controllers
{
    [Authorize(Roles = "Cook")]
    public class CookController : Controller
    {
        private readonly UserManager<Staff> _userManager;
        private readonly IPositionRepository _positionRepository;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IOrderRepository _orderRepository;

        public CookController(UserManager<Staff> userManager, IPositionRepository positionRepository, IMenuItemRepository menuItemRepository,
            IOrderRepository orderRepository)
        {
            _userManager = userManager;
            _positionRepository = positionRepository;
            _menuItemRepository = menuItemRepository;
            _orderRepository = orderRepository;
        }

        #region Account
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new MyAccountViewModel();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            model.FullName = $"{user.Name} {user.Surname} {user.Patronymic}";
            model.Salary = user.Salary;
            var position = _positionRepository.GetPositionById(user.PositionId);
            model.Position = position is null ? null : position.Title;

            return View(model);
        }
        #endregion

        #region Schedule
        public IActionResult Schedule()
        {
            return View();
        }
        #endregion

        #region Orders
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            var order_menuItems = await _orderRepository.GetAllWithDetaisAsync();

            var model = new List<OrdersViewModel>();
            foreach (var order_menuItem in order_menuItems)
            {
                var menuItem = await _menuItemRepository.GetByIdAsync(order_menuItem.MenuItemId);

                var orderViewModel = new OrdersViewModel
                {
                    Id = order_menuItem.Id,
                    OrderId = order_menuItem.OrderId,
                    MenuItemId = order_menuItem.MenuItemId,
                    MenuItemTitle = menuItem is null ? string.Empty : menuItem.Title,
                    Comment = order_menuItem.Comment,
                    IsReady = order_menuItem.IsReady
                };

                if (!orderViewModel.IsReady)
                {
                    model.Add(orderViewModel);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SetReady(int id)
        {
            var order_menuItem = await _orderRepository.GetOrderDetailsById(id);
            order_menuItem.IsReady = true;
            _orderRepository.UpdateOrderDetails(order_menuItem);

            return RedirectToAction("Orders");
        }
        #endregion
    }
}
