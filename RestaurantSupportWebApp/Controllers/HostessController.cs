using Microsoft.AspNetCore.Mvc;
using RestaurantSupportWebApp.Models;
using RestaurantSupportWebApp.ViewModels;
using RestaurantSupportWebApp.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using RestaurantSupportWebApp.Interfaces;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace RestaurantSupportWebApp.Controllers
{
    [Authorize(Roles = "Hostess")]
    public class HostessController : Controller
    {
        private readonly IStaffRepository _staffRepository;
        private readonly UserManager<Staff> _userManager;
        private readonly IPositionRepository _positionRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly ITableRepository _tableRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IClientRepository _clientRepository;

        public HostessController(UserManager<Staff> userManager, IStaffRepository staffRepository, IPositionRepository positionRepository, IScheduleRepository scheduleRepository,
            ITableRepository tableRepository, IReservationRepository reservationRepository, IRestaurantRepository restaurantRepository,
            IClientRepository clientRepository)
        {
            _staffRepository = staffRepository;
            _userManager = userManager;
            _positionRepository = positionRepository;
            _scheduleRepository = scheduleRepository;
            _tableRepository = tableRepository;
            _reservationRepository = reservationRepository;
            _restaurantRepository = restaurantRepository;
            _clientRepository = clientRepository;
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

        #region Reservations
        [HttpGet]
        public IActionResult Reservations()
        {
            var model = new ReservationViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Reservations(ReservationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = new Client();

            client.Name = model.ClientName;
            string regex = @"^ *\+?(\d{2})[ -]*(\d) *(\d{2})[ -]*(\d{3})[ -]*(\d{2})[ -]*(\d{2}) *$";
            if (Regex.IsMatch(model.ClientPhone, regex))
            {
                client.PhoneNumber = long.Parse(Regex.Replace(model.ClientPhone, regex, "$1$2$3$4$5$6"));
            }
            else
            {
                ModelState.AddModelError("", "Номер телефону повинен бути у форматі +380XXXXXXXXX");
                return View(model);
            }
            _clientRepository.Add(client);

            var reservation = new Reservation();

            reservation.StartTime = model.StartTime;
            reservation.EndTime = model.EndTime;
            reservation.NumberOfPeople = model.NumberOfPeople;
            reservation.TableId = model.TableId;
            reservation.Table = await _tableRepository.GetByIdAsync(model.TableId);
            reservation.Client = client;
            reservation.ClientId = client.Id;
            reservation.Comment = model.Comment is null ? "" : model.Comment;

            _reservationRepository.Add(reservation);

            return RedirectToAction("Hall");
        }

        [HttpPost]
        public async Task<IActionResult> ReservationsGetTables([FromBody] ReservationViewModel model)
        {
            var tables = await _tableRepository.GetAllAsync();
            var reservations = await _reservationRepository.GetAllAsync();

            List<int> availableTablesIds = new List<int>();
            foreach (var table in tables)
            {
                if (table.Hall == model.Hall && table.NumberOfSeats >= model.NumberOfPeople)
                {
                    bool isAvailable = true;
                    foreach (var reservation in reservations)
                    {
                        if (table.Id == reservation.TableId)
                        {
                            if (model.StartTime >= reservation.StartTime && model.StartTime <= reservation.EndTime)
                            {
                                isAvailable = false;
                            }
                            if (model.EndTime <= reservation.StartTime && model.EndTime >= reservation.EndTime)
                            {
                                isAvailable = false;
                            }
                        }
                    }
                    if (isAvailable)
                    {
                        availableTablesIds.Add(table.Id);
                    }
                }
            }

            string jsonString = JsonSerializer.Serialize(availableTablesIds);
            return Ok(jsonString);
        }

        public async Task<IActionResult> ReservationsGetAvailableTables()
        {
            var tables = await _tableRepository.GetAllAsync();
            string jsonString = JsonSerializer.Serialize(tables);
            return Ok(jsonString);
        }
        #endregion

        #region Hall
        [HttpGet]
        public async Task<IActionResult> Hall()
        {
            var tables = await _tableRepository.GetAllAsync();
            var reservations = await _reservationRepository.GetAllAsync();
            var model = new List<TableViewModel>();

            foreach (var table in tables)
            {
                var tableViewModel = new TableViewModel
                {
                    TableId = table.Id,
                    Hall = table.Hall,
                    Capacity = table.NumberOfSeats,
                    Reservation = "Відсутнє"
                };

                var reservationsSorted = reservations.OrderBy(r => r.StartTime);

                foreach (var reservation in reservationsSorted)
                {
                    if (table.Id == reservation.TableId && tableViewModel.ReservationId == null)
                    {
                        if (reservation.StartTime >= DateTime.Today)
                        {
                            tableViewModel.Reservation =
                            $"{reservation.StartTime.ToString("dd.MM.yyyy")} {reservation.StartTime.ToString("HH:mm")} " +
                            $"- {reservation.EndTime.ToString("dd.MM.yyyy")} {reservation.EndTime.ToString("HH:mm")}";
                            tableViewModel.ReservationId = reservation.Id;
                        }
                        else
                        {
                            tableViewModel.Reservation = "Відсутнє";
                        }
                    }
                }

                model.Add(tableViewModel);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult AddTable()
        {
            var model = new AddEditTableViewModel();
            return PartialView("_AddTablePartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddTable(AddEditTableViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Hall");

            var restaurant = await _restaurantRepository.GetByIdAsync(model.RestaurantId);

            var table = new Table
            {
                Hall = model.Hall,
                NumberOfSeats = model.NumberOfSeats,
                RestaurantId = model.RestaurantId,
                Restaurant = restaurant
            };

            _tableRepository.Add(table);

            return RedirectToAction("Hall");
        }

        [HttpPost]
        public async Task<IActionResult> AddTableGetRestaurants()
        {
            var restaurants = await _restaurantRepository.GetAllAsync();
            string jsonString = JsonSerializer.Serialize(restaurants);
            return Ok(jsonString);
        }

        [HttpGet]
        public async Task<IActionResult> EditTable(int id)
        {
            var table = await _tableRepository.GetByIdAsync(id);
            var model = new AddEditTableViewModel
            {
                TableId = table.Id,
                Hall = table.Hall,
                NumberOfSeats = table.NumberOfSeats,
            };

            string jsonString = JsonSerializer.Serialize(model);
            return Ok(jsonString);
        }

        [HttpPost]
        public async Task<IActionResult> EditTable(AddEditTableViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Hall");

            var table = await _tableRepository.GetByIdAsync(model.TableId);

            table.Hall = model.Hall;
            table.NumberOfSeats = model.NumberOfSeats;

            _tableRepository.Update(table);

            return RedirectToAction("Hall");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteTable(int id)
        {
            try
            {
                var table = await _tableRepository.GetByIdAsync(id);
                if (table is null)
                    return NotFound();

                _tableRepository.Delete(table);
                return RedirectToAction("Hall");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Waiters
        [HttpGet]
        public async Task<IActionResult> Waiters()
        {
            var waiters = await _userManager.GetUsersInRoleAsync("Waiter");
            var waitersViewModels = new List<StaffViewModel>();
            foreach (var waiter in waiters)
            {
                var schedule = await _scheduleRepository.GetByIdAsync(waiter.ScheduleId);
                waitersViewModels.Add(new StaffViewModel
                {
                    Id = waiter.Id,
                    FullName = $"{waiter.Name} {waiter.Surname} {waiter.Patronymic}",
                    ScheduleName = schedule is null ? "" : schedule.Name
                });
            }
            return View(waitersViewModels);
        }

        [HttpGet]
        public IActionResult AddWaiter()
        {
            var model = new AddStaffViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddWaiter(AddStaffViewModel model)
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

            var waiter = new Staff();

            waiter.Name = model.Name;
            waiter.Surname = model.Surname;
            waiter.Patronymic = model.Patronymic;
            waiter.UserName = model.Username;
            waiter.Salary = model.Salary;
            waiter.Schedule = schedule;
            waiter.Position = position;
            waiter.ScheduleId = schedule is null ? null : schedule.Id;
            waiter.PositionId = position is null ? null : position.Id;

            var result = await _userManager.CreateAsync(waiter, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(waiter, "Waiter");
                return RedirectToAction("Waiters");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddWaiterGetPositions()
        {
            var positions = await _positionRepository.GetAllAsync();
            string jsonString = JsonSerializer.Serialize(positions);
            return Ok(jsonString);
        }

        [HttpPost]
        public async Task<IActionResult> AddWaiterGetSchedules()
        {
            var schedules = await _scheduleRepository.GetAllAsync();
            string jsonString = JsonSerializer.Serialize(schedules);
            return Ok(jsonString);
        }

        [HttpGet]
        public async Task<IActionResult> EditWaiter(string id)
        {
            var waiter = await _userManager.FindByIdAsync(id);
            if (waiter is null)
                return NotFound();

            var model = new EditStaffViewModel
            {
                StaffId = waiter.Id,
                StaffName = $"{waiter.Name} {waiter.Surname} {waiter.Patronymic}",
                Salary = waiter.Salary,
                PositionId = waiter.PositionId,
                ScheduleId = waiter.ScheduleId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditWaiter(EditStaffViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Waiters");

            var waiter = await _userManager.FindByIdAsync(model.StaffId);

            if (waiter is null)
                return NotFound();

            waiter.Salary = model.Salary;
            waiter.ScheduleId = model.ScheduleId;
            waiter.PositionId = model.PositionId;

            _staffRepository.Update(waiter);

            return RedirectToAction("Waiters");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteWaiter(string id)
        {
            try
            {
                var waiter = await _userManager.FindByIdAsync(id);
                if (waiter is null)
                    return NotFound();

                _staffRepository.Delete(waiter);
                return RedirectToAction("Waiters");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Placement
        public async Task<IActionResult> Placement()
        {
            var tables = await _tableRepository.GetAllAsync();
            var reservations = await _reservationRepository.GetAllAsync();
            var model = new List<PlacementViewModel>();

            foreach (var table in tables)
            {
                var placementViewModel = new PlacementViewModel
                {
                    TableId = table.Id,
                    Reservation = "Відсутнє"
                };

                foreach (var reservation in reservations)
                {
                    if (table.Id == reservation.TableId && reservation.StartTime.Date == DateTime.Today)
                    {
                        var client = await _clientRepository.GetByIdAsync(reservation.ClientId);
                        placementViewModel.CustomerName = $"{client.Name}";
                        placementViewModel.Reservation =
                            $"{reservation.StartTime.ToString("HH:mm")} - {reservation.EndTime.ToString("HH:mm")}";
                        placementViewModel.ReservationId = reservation.Id;
                        placementViewModel.Comment = reservation.Comment;
                    }
                }

                if (!string.IsNullOrEmpty(placementViewModel.CustomerName))
                {
                    model.Add(placementViewModel);
                }
            }
            return View(model);
        }
        #endregion
    }
}
