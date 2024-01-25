using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurantSupportWebApp.Interfaces;
using RestaurantSupportWebApp.Models;
using RestaurantSupportWebApp.ViewModels;
using System.Text.Json;

namespace RestaurantSupportWebApp.Controllers
{
    [Authorize(Roles = "Waiter")]
    public class WaiterController : Controller
    {
        private readonly UserManager<Staff> _userManager;
        private readonly IPositionRepository _positionRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly ITableRepository _tableRepository;

        public WaiterController(UserManager<Staff> userManager, IPositionRepository positionRepository, IOrderRepository orderRepository,
            IMenuItemRepository menuItemRepository, ITableRepository tableRepository)
        {
            _userManager = userManager;
            _positionRepository = positionRepository;
            _orderRepository = orderRepository;
            _menuItemRepository = menuItemRepository;
            _tableRepository = tableRepository;
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

        #region Order
        [HttpGet]
        public IActionResult Order()
        {
            var model = new NewOrderViewModel();
            return View("Order", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] NewOrderViewModel model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                var order = new Order();
                if (user is not null)
                {
                    order.TableId = model.TableId;
                    order.WaiterId = user.Id;
                    order.Comment = model.Comment is null ? "" : model.Comment;
                    order.IsPaid = false;
                }

                _orderRepository.Add(order);

                foreach (var meal in model.Meals)
                {
                    var menuItem = await _menuItemRepository.GetByTitleAsync(meal.Title);
                    var mealInOrder = new Order_MenuItem
                    {
                        OrderId = order.Id,
                        MenuItemId = menuItem.Id,
                        Comment = meal.Comment is null ? "" : meal.Comment,
                        IsReady = false,
                    };

                    _menuItemRepository.AddMenuItemToOrder(mealInOrder);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Receipt
        [HttpGet]
        public IActionResult Receipt(int tableId)
        {
            var model = new CloseOrderViewModel();
            model.TableId = tableId;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Receipt([FromBody] CloseOrderViewModel model)
        {
            var orders = await _orderRepository.GetOrdersByTableId(model.TableId);
            var lastOrder = orders.LastOrDefault();
            var order_menuItems = await _orderRepository.GetAllWithDetaisAsync();
            model.Meals = new List<MealInOrderViewModel>();

            foreach (var order_menuItem in order_menuItems)
            {
                if (order_menuItem.OrderId == lastOrder.Id && !lastOrder.IsPaid)
                {
                    var menuItem = await _menuItemRepository.GetByIdAsync(order_menuItem.MenuItemId);

                    var mealInOrderViewModel = new MealInOrderViewModel
                    {
                        Title = menuItem is null ? string.Empty : menuItem.Title,
                        Price = menuItem is null ? 0 : menuItem.Price,
                    };

                    model.Meals.Add(mealInOrderViewModel);
                }
            }

            string jsonString = JsonSerializer.Serialize(model);
            return Ok(jsonString);
        }

        [HttpPost]
        public async Task<IActionResult> CloseOrder(CloseOrderViewModel model)
        {
            try
            {
                var orders = await _orderRepository.GetOrdersByTableId(model.TableId);
                var lastOrder = orders.LastOrDefault();
                if (lastOrder is not null)
                {
                    lastOrder.IsPaid = true;
                    _orderRepository.Update(lastOrder);
                }

                return View("Receipt");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> GetFreeTables()
        {
            var allTables = await _tableRepository.GetAllAsync();
            var freeTables = new List<Table>();
            foreach (var table in allTables)
            {
                var orders = await _orderRepository.GetOrdersByTableId(table.Id);
                var lastOrder = orders.LastOrDefault();
                if (lastOrder is not null && !lastOrder.IsPaid)
                {
                    continue;
                }
                else
                {
                    freeTables.Add(table);
                }
            }
            string jsonString = JsonSerializer.Serialize(freeTables);
            return Ok(jsonString);
        }

        [HttpGet]
        public async Task<IActionResult> GetNotPaidTables()
        {
            var allTables = await _tableRepository.GetAllAsync();
            var notPaidTables = new List<Table>();
            foreach (var table in allTables)
            {
                var orders = await _orderRepository.GetOrdersByTableId(table.Id);
                var lastOrder = orders.LastOrDefault();
                if (lastOrder is not null && !lastOrder.IsPaid)
                {
                    notPaidTables.Add(table);
                }
            }
            string jsonString = JsonSerializer.Serialize(notPaidTables);
            return Ok(jsonString);
        }

        [HttpGet]
        public async Task<IActionResult> GetMenuItems()
        {
            var menuItems = await _menuItemRepository.GetAllAsync();
            string jsonString = JsonSerializer.Serialize(menuItems);
            return Ok(jsonString);
        }
    }
}
