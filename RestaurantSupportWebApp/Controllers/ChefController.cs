using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurantSupportWebApp.Interfaces;
using RestaurantSupportWebApp.Models;
using RestaurantSupportWebApp.Repositories;
using RestaurantSupportWebApp.ViewModels;
using System.Text.Json;

namespace RestaurantSupportWebApp.Controllers
{
    [Authorize(Roles = "Chef")]
    public class ChefController : Controller
    {
        private readonly UserManager<Staff> _userManager;
        private readonly IPositionRepository _positionRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductInCartRepository _productInCartRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ChefController(UserManager<Staff> userManager, IPositionRepository positionRepository, IScheduleRepository scheduleRepository,
            IMenuItemRepository menuItemRepository, IOrderRepository orderRepository, IProductRepository productRepository,
            IProductInCartRepository productInCartRepository, ICategoryRepository categoryRepository)
        {
            _userManager = userManager;
            _positionRepository = positionRepository;
            _scheduleRepository = scheduleRepository;
            _menuItemRepository = menuItemRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _productInCartRepository = productInCartRepository;
            _categoryRepository = categoryRepository;
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

        #region Cooks
        [HttpGet]
        public async Task<IActionResult> Cooks()
        {
            var cooks = await _userManager.GetUsersInRoleAsync("Cook");
            var cooksViewModels = new List<StaffViewModel>();
            foreach (var cook in cooks)
            {
                var schedule = await _scheduleRepository.GetByIdAsync(cook.ScheduleId);
                cooksViewModels.Add(new StaffViewModel
                {
                    Id = cook.Id,
                    FullName = $"{cook.Name} {cook.Surname} {cook.Patronymic}",
                    ScheduleName = schedule is null ? "" : schedule.Name
                });
            }
            return View(cooksViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> AddCook()
        {
            var model = new AddStaffViewModel();
            return await AddCook(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddCook(AddStaffViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Salary < 0)
            {
                ModelState.AddModelError("", "Заробітна плата не може бути від'ємною");
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null)
                return View(model);

            // selected values
            var position = _positionRepository.GetPositionById(model.PositionId);
            var schedule = await _scheduleRepository.GetByIdAsync(model.ScheduleId);

            var cook = new Staff();

            cook.Name = model.Name;
            cook.Surname = model.Surname;
            cook.Patronymic = model.Patronymic;
            cook.UserName = model.Username;
            cook.Salary = model.Salary;
            cook.Schedule = schedule;
            cook.Position = position;
            cook.ScheduleId = schedule is null ? null : schedule.Id;
            cook.PositionId = position is null ? null : position.Id;

            var result = await _userManager.CreateAsync(cook, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(cook, "Cook");
                return RedirectToAction("Cooks");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetPositions()
        {
            var positions = await _positionRepository.GetAllAsync();
            string jsonString = JsonSerializer.Serialize(positions);
            return Ok(jsonString);
        }

        [HttpPost]
        public async Task<IActionResult> GetSchedules()
        {
            var schedules = await _scheduleRepository.GetAllAsync();
            string jsonString = JsonSerializer.Serialize(schedules);
            return Ok(jsonString);
        }

        [HttpGet]
        public async Task<IActionResult> EditCook(string id)
        {
            var cook = await _userManager.FindByIdAsync(id);
            if (cook is null)
                return NotFound();

            var model = new EditStaffViewModel
            {
                StaffId = cook.Id,
                StaffName = $"{cook.Name} {cook.Surname} {cook.Patronymic}",
                Salary = cook.Salary,
                PositionId = cook.PositionId,
                ScheduleId = cook.ScheduleId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCook(EditStaffViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Salary < 0)
            {
                ModelState.AddModelError("", "Заробітна плата не може бути від'ємною");
                return View(model);
            }

            var cook = await _userManager.FindByIdAsync(model.StaffId);
            if (cook is null)
                return NotFound();

            cook.Salary = model.Salary;
            cook.PositionId = model.PositionId;
            cook.ScheduleId = model.ScheduleId;

            var result = await _userManager.UpdateAsync(cook);

            if (result.Succeeded)
            {
                return RedirectToAction("Cooks");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCook(string id)
        {
            var cook = await _userManager.FindByIdAsync(id);
            if (cook is null)
                return NotFound();

            var result = await _userManager.DeleteAsync(cook);

            if (result.Succeeded)
            {
                return RedirectToAction("Cooks");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return RedirectToAction("Cooks");
        }
        #endregion

        #region Menu
        [HttpGet]
        public async Task<IActionResult> Menu()
        {
            var model = new List<MenuViewModel>();

            var menuItems = await _menuItemRepository.GetAllWithCategories();
            foreach (var menuItem in menuItems)
            {
                model.Add(new MenuViewModel
                {
                    Id = menuItem.Id,
                    Title = menuItem.Title,
                    Price = menuItem.Price,
                    Weight = menuItem.Weight,
                    Category = menuItem.Category is null ? string.Empty : menuItem.Category.Title
                });
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult NewMenuItem()
        {
            var model = new AddMenuItemViewModel();
            return View("AddMenuItem", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddMenuItem([FromBody] AddMenuItemViewModel model)
        {
            try
            {
                if (model.Weight < 0)
                {
                    return BadRequest("Вага не може бути від'ємною");
                }

                if (model.Price < 0)
                {
                    return BadRequest("Ціна не може бути від'ємною");
                }

                var menuItem = new MenuItem
                {
                    Title = model.Title,
                    Description = model.Description is null ? "" : model.Description,
                    Weight = model.Weight,
                    Price = model.Price,
                    PreparationTime = model.PreparationTime,
                    CategoryId = model.CategoryId
                };

                _menuItemRepository.Add(menuItem);

                foreach (var product in model.Products)
                {
                    var productDb = await _productRepository.GetByProductNameAsync(product.Title);
                    var productInMenuItem = new MenuItem_Product
                    {
                        ProductWeight = product.Weight,
                        MenuItemId = menuItem.Id,
                        ProductId = productDb.Id
                    };
                    _menuItemRepository.AddProductToMenuItem(productInMenuItem);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddMenuItemGetCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            string jsonString = JsonSerializer.Serialize(categories);
            return Ok(jsonString);
        }

        [HttpGet]
        public async Task<IActionResult> EditMenu(int id)
        {
            var model = new EditMenuItemViewModel();
            var menuItem = await _menuItemRepository.GetByIdAsync(id);
            if (menuItem is null)
                return NotFound();

            model.Id = menuItem.Id;
            model.Title = menuItem.Title;
            model.Description = menuItem.Description;
            model.Weight = menuItem.Weight;
            model.Price = menuItem.Price;
            model.PreparationTime = menuItem.PreparationTime;
            model.CategoryId = menuItem.CategoryId;

            var products = await _menuItemRepository.GetProductsByMenuItemIdAsync(id);
            model.Products = new List<ProductInMenuItemViewModel>();
            foreach (var product in products)
            {
                model.Products.Add(new ProductInMenuItemViewModel
                {
                    Title = product.Product.Name,
                    Weight = product.ProductWeight
                });
            }
            return View("EditMenu", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditMenuItem([FromBody] EditMenuItemViewModel model)
        {
            try
            {
                if (model.Weight < 0)
                {
                    return BadRequest("Вага не може бути від'ємною");
                }

                if (model.Price < 0)
                {
                    return BadRequest("Ціна не може бути від'ємною");
                }

                var menuItem = await _menuItemRepository.GetByIdAsync(model.Id);
                if (menuItem is null)
                    return NotFound();

                menuItem.Title = model.Title;
                menuItem.Description = model.Description is null ? "" : model.Description;
                menuItem.Weight = model.Weight;
                menuItem.Price = model.Price;
                menuItem.PreparationTime = model.PreparationTime;
                menuItem.CategoryId = model.CategoryId;

                _menuItemRepository.Update(menuItem);

                var products = await _menuItemRepository.GetProductsByMenuItemIdAsync(model.Id);
                foreach (var product in products)
                {
                    _menuItemRepository.DeleteProductFromMenuItem(product);
                }

                foreach (var product in model.Products)
                {
                    var productDb = await _productRepository.GetByProductNameAsync(product.Title);
                    var productInMenuItem = new MenuItem_Product
                    {
                        ProductWeight = product.Weight,
                        MenuItemId = menuItem.Id,
                        ProductId = productDb.Id
                    };
                    _menuItemRepository.AddProductToMenuItem(productInMenuItem);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            try
            {
                var menuItem = await _menuItemRepository.GetByIdAsync(id);
                if (menuItem is null)
                    return NotFound();

                _menuItemRepository.Delete(menuItem);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Products
        [HttpGet]
        public async Task<IActionResult> Products()
        {
            var products = await _productRepository.GetAllAsync();
            var model = new List<ProductsViewModel>();
            foreach (var product in products)
            {
                model.Add(new ProductsViewModel
                {
                    Title = product.Name,
                    Amount = product.AmountInStock
                });
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            var model = new AddProductViewModel();
            return PartialView("_AddProductPartial", model);
        }

        [HttpPost]
        public IActionResult AddProduct(AddProductViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Products");

            var product = new Product
            {
                Name = model.Title,
                IsAllergen = model.IsAllergen,
                Price = model.Price
            };

            _productRepository.Add(product);

            return RedirectToAction("Products");
        }

        [HttpGet]
        public async Task<IActionResult> OrderProducts()
        {
            var model = new List<OrderProductsViewModel>();
            return await OrderProducts(model);
        }

        [HttpPost]
        public async Task<IActionResult> OrderProducts(List<OrderProductsViewModel> model)
        {
            var productsInCart = await _productInCartRepository.GetAllAsync();

            foreach (var productInCart in productsInCart)
            {
                var product = await _productRepository.GetByIdAsync(productInCart.ProductId);
                model.Add(new OrderProductsViewModel
                {
                    Id = productInCart.Id,
                    Title = product.Name,
                    Amount = productInCart.Amount,
                    Price = product.Price * productInCart.Amount
                });
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> OrderProductsSubmit()
        {
            if (!ModelState.IsValid)
                return RedirectToAction("OrderProducts");

            var productsInCart = await _productInCartRepository.GetAllAsync();
            foreach (var productInCart in productsInCart)
            {
                var product = await _productRepository.GetByIdAsync(productInCart.ProductId);
                product.AmountInStock += productInCart.Amount;
                _productRepository.Update(product);
            }

            _productInCartRepository.DeleteAll();

            return RedirectToAction("Products");
        }

        [HttpGet]
        public IActionResult AddOrderProduct()
        {
            var model = new AddOrderProductViewModel();
            return PartialView("_AddOrderProductPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrderProduct(AddOrderProductViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("OrderProducts");

            if (model.Amount < 0)
            {
                ModelState.AddModelError("", "Кількість не може бути від'ємною");
                return RedirectToAction("OrderProducts");
            }

            var product = await _productRepository.GetByProductNameAsync(model.ProductName);
            var productInCart = await _productInCartRepository.GetByProductNameAsync(model.ProductName);
            if (productInCart is null)
            {
                productInCart = new ProductInCart
                {
                    Product = product,
                    Amount = model.Amount
                };
                _productInCartRepository.Add(productInCart);
            }
            else
            {
                productInCart.Amount += model.Amount;
                _productInCartRepository.Update(productInCart);
            }

            return RedirectToAction("OrderProducts");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProductFromCart(int id)
        {
            try
            {
                var productInCart = await _productInCartRepository.GetByIdAsync(id);
                if (productInCart is null)
                    return NotFound();

                _productInCartRepository.Delete(productInCart);

                return RedirectToAction("OrderProducts");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productRepository.GetAllAsync();
            string jsonString = JsonSerializer.Serialize(products);
            return Ok(jsonString);
        }
        #endregion
    }
}
