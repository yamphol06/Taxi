using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxi.Common.Enums;
using Taxi.Web.Data.Entities;
using Taxi.Web.Helpers;

namespace Taxi.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext dataContext, IUserHelper userHelper)
        {
            _dataContext = dataContext;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _dataContext.Database.EnsureCreatedAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Juan", "Zuluaga", "jzuluaga55@gmail.com", "350 634 2747", "Calle Luna Calle Sol", UserType.Admin);
            var driver = await CheckUserAsync("2020", "Juan", "Zuluaga", "jzuluaga55@hotmail.com", "350 634 2747", "Calle Luna Calle Sol", UserType.Driver);
            var user1 = await CheckUserAsync("3030", "Juan", "Zuluaga", "carlos.zuluaga@globant.com", "350 634 2747", "Calle Luna Calle Sol", UserType.User);
            var user2 = await CheckUserAsync("4040", "Juan", "Zuluaga", "juanzuluaga2480@correo.itm.edu.co", "350 634 2747", "Calle Luna Calle Sol", UserType.User);
            await CheckTaxisAsync(driver, user1, user2);
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.Driver.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task<UserEntity> CheckUserAsync(
                  string document,
                  string firstName,
                  string lastName,
                  string email,
                  string phone,
                  string address,
                  UserType userType)
        {
            var user = await _userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                user = new UserEntity
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    UserType = userType
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }
            return user;
        }


        private Task CheckTaxisAsync()
        {
            throw new NotImplementedException();
        }

        private async Task CheckTaxisAsync(
           UserEntity driver,
           UserEntity user1,
           UserEntity user2)
        {
            if (!_dataContext.Taxis.Any())
            {
                _dataContext.Taxis.Add(new TaxiEntity
                {
                    User = driver,
                    Plaque = "TPQ123",
                    Trips = new List<TripEntity>
                    {
                        new TripEntity
                        {
                            StartDate = DateTime.UtcNow,
                            EndDate = DateTime.UtcNow.AddMinutes(30),
                            Qualification = 4.5f,
                            Source = "ITM Fraternidad",
                            Target = "ITM Robledo",
                            Remarks = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam in vulputate felis. Curabitur dictum tellus dolor, eget bibendum risus tincidunt id. Morbi iaculis, orci ut tempor faucibus, ipsum lorem ornare dui, sed bibendum ex nisl pulvinar diam. Quisque et ex ut lectus venenatis ultrices id non urna. In vitae ipsum ut lacus aliquam luctus ac non nulla. Integer et lectus a ex hendrerit pharetra ut id urna. Quisque vel eleifend tortor. Nunc egestas elementum lorem. In sed dui id dolor aliquet congue. Morbi at purus facilisis, vestibulum massa sed, cursus tellus.",
                            User = user1
                        },
                        new TripEntity
                        {
                            StartDate = DateTime.UtcNow,
                            EndDate = DateTime.UtcNow.AddMinutes(30),
                            Qualification = 4.8f,
                            Source = "ITM Robledo",
                            Target = "ITM Fraternidad",
                            Remarks = "Curabitur id ante urna. Donec justo eros, eleifend ut fermentum non, scelerisque ac est. Nunc eu maximus ex, id vulputate purus. Curabitur bibendum ipsum at diam placerat, ac viverra lacus euismod. Praesent in tortor magna. Aliquam in pulvinar nisl, at ornare quam. In convallis interdum tempus. Vestibulum hendrerit ullamcorper hendrerit. Nulla sed ipsum nec purus aliquet maximus ut sed dolor. Pellentesque vestibulum, tellus id mollis vestibulum, justo massa cursus augue, venenatis consectetur elit orci nec magna. Sed eget magna lacus. Maecenas faucibus pulvinar vestibulum. Proin eu elit hendrerit, lobortis ex vel, consequat nisl. Sed porta eget libero nec rhoncus.",
                            User = user1
                        }
                    }
                });

                _dataContext.Taxis.Add(new TaxiEntity
                {
                    Plaque = "THW321",
                    User = driver,
                    Trips = new List<TripEntity>
                    {
                        new TripEntity
                        {
                            StartDate = DateTime.UtcNow,
                            EndDate = DateTime.UtcNow.AddMinutes(30),
                            Qualification = 4.5f,
                            Source = "ITM Fraternidad",
                            Target = "ITM Robledo",
                            Remarks = "Muy buen servicio",
                            User = user2
                        },
                        new TripEntity
                        {
                            StartDate = DateTime.UtcNow,
                            EndDate = DateTime.UtcNow.AddMinutes(30),
                            Qualification = 4.8f,
                            Source = "ITM Robledo",
                            Target = "ITM Fraternidad",
                            Remarks = "Conductor muy amable",
                            User = user2
                        }
                    }
                });

                await _dataContext.SaveChangesAsync();
            }
        }
    }
}
