using Microsoft.AspNetCore.Identity;
using RestaurantSupportWebApp.Models;
using System.Diagnostics;
using System.Text.Json;

namespace RestaurantSupportWebApp.Data
{
    public static class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DataContext>();

                context.Database.EnsureCreated();

                //Addresses
                var addreses = new List<Address>();
                using (var reader = new StreamReader("Data/Json/Addresses.json"))
                {
                    string json = reader.ReadToEnd();
                    addreses = JsonSerializer.Deserialize<List<Address>>(json);
                }
                if (!context.Addresses.Any())
                {
                    foreach (var address in addreses)
                    {
                        context.Addresses.Add(address);
                    }
                    context.SaveChanges();
                }

                //Restaurants
                var restaurants = new List<Restaurant>();
                using (var reader = new StreamReader("Data/Json/Restaurants.json"))
                {
                    string json = reader.ReadToEnd();
                    restaurants = JsonSerializer.Deserialize<List<Restaurant>>(json);
                }
                if (!context.Restaurants.Any())
                {
                    foreach (var restaurant in restaurants)
                    {
                        context.Restaurants.Add(restaurant);
                    }
                    context.SaveChanges();
                }

                //Categories
                var categories = new List<Category>();
                using (var reader = new StreamReader("Data/Json/Categories.json"))
                {
                    string json = reader.ReadToEnd();
                    categories = JsonSerializer.Deserialize<List<Category>>(json);
                }
                if (!context.Categories.Any())
                {
                    foreach (var category in categories)
                    {
                        context.Categories.Add(category);
                    }
                    context.SaveChanges();
                }

                //Products
                var products = new List<Product>();
                using (var reader = new StreamReader("Data/Json/Products.json"))
                {
                    string json = reader.ReadToEnd();
                    products = JsonSerializer.Deserialize<List<Product>>(json);
                }
                if (!context.Products.Any())
                {
                    foreach (var product in products)
                    {
                        context.Products.Add(product);
                    }
                    context.SaveChanges();
                }

                //Positions
                var positions = new List<Position>();
                using (var reader = new StreamReader("Data/Json/Positions.json"))
                {
                    string json = reader.ReadToEnd();
                    positions = JsonSerializer.Deserialize<List<Position>>(json);
                }
                if (!context.Positions.Any())
                {
                    foreach (var position in positions)
                    {
                        context.Positions.Add(position);
                    }
                    context.SaveChanges();
                }

                //Schedules
                var schedules = new List<Schedule>();
                using (var reader = new StreamReader("Data/Json/Schedules.json"))
                {
                    string json = reader.ReadToEnd();
                    schedules = JsonSerializer.Deserialize<List<Schedule>>(json);
                }
                if (!context.Schedules.Any())
                {
                    foreach (var schedule in schedules)
                    {
                        context.Schedules.Add(schedule);
                    }
                    context.SaveChanges();
                }
            }
        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Hostess))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Hostess));
                if (!await roleManager.RoleExistsAsync(UserRoles.Waiter))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Waiter));
                if (!await roleManager.RoleExistsAsync(UserRoles.Chef))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Chef));
                if (!await roleManager.RoleExistsAsync(UserRoles.Cook))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Cook));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<Staff>>();

                string username = "na_hostess";
                string password = "Abc123!";

                var user = await userManager.FindByNameAsync(username);
                if (user == null)
                {
                    user = new Staff
                    {
                        UserName = username,
                        Name = "Надія",
                        Surname = "Атрошенко",
                        Patronymic = "Герасимівна",
                        Salary = 40000,
                        PositionId = 9,
                        ScheduleId = 2
                    };
                    await userManager.CreateAsync(user, password);
                    await userManager.AddToRoleAsync(user, UserRoles.Hostess);
                }

                string usernameChef = "al_chef";
                string passwordChef = "Abc123!";

                var userChef = await userManager.FindByNameAsync(usernameChef);
                if (userChef == null)
                {
                    userChef = new Staff
                    {
                        UserName = usernameChef,
                        Name = "Алла",
                        Surname = "Лебеденко",
                        Patronymic = "Зорянівна",
                        Salary = 40000,
                        PositionId = 3,
                        ScheduleId = 5
                    };
                    await userManager.CreateAsync(userChef, passwordChef);
                    await userManager.AddToRoleAsync(userChef, UserRoles.Chef);
                }

                string usernameChef1 = "zb_chef";
                string passwordChef1 = "Abc123!";

                var userChef1 = await userManager.FindByNameAsync(usernameChef1);
                if (userChef1 == null)
                {
                    userChef = new Staff
                    {
                        UserName = usernameChef1,
                        Name = "Захар",
                        Surname = "Балицький",
                        Patronymic = "Левович",
                        Salary = 30000,
                        PositionId = 4,
                        ScheduleId = 1
                    };
                    await userManager.CreateAsync(userChef, passwordChef1);
                    await userManager.AddToRoleAsync(userChef, UserRoles.Chef);
                }
            }
        }
    }
}
