using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Sat.Recruitment.Core;
using Sat.Recruitment.Core.Domain;
using Sat.Recruitment.Core.Services;

namespace Sat.Recruitment.Test.Services.Tests
{
    [TestFixture]
    public class UserServiceTests : BaseTest
    {
        #region Fields

        private IUserService _userService;

        #endregion

        #region SetUp

        [OneTimeSetUp]
        public void SetUp()
        {
            _userService = GetService<IUserService>();
        }

        #endregion

        #region Tests

        [Test]
        public async Task CanExtractUsersFromFile()
        {
            var users = await _userService.GetAllUsersAsync();
            foreach (var user in users)
                Console.WriteLine($"Name: {user.Name} - Email: {user.Email} - Address: {user.Address} - Type: {user.UserType}");
            
            users.Should().NotBeEmpty();
        }

        [Test]
        public async Task CanAddNewUserToFile()
        {
            var user = new User
            {
                Name = "Mike",
                Email = "Mike@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = UserType.Normal,
                Money = 123
            };

            CommonHelper.IsValidEmail(user.Email).Should().BeTrue();

            await _userService.InsertUserAsync(user);

            var users = await _userService.GetAllUsersAsync();
            users.Any(x => x.Name == user.Name).Should().BeTrue();
        }

        [Test]
        public async Task CannotAddDuplicatedUserToFile()
        {
            var user = new User
            {
                Name = "Agustina",
                Email = "Agustina@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = UserType.Normal,
                Money = 123
            };

            var users = await _userService.GetAllUsersAsync();
            var alreadyExists = users.Any(x => x.Email == user.Email || x.Phone == user.Phone || (x.Name == user.Name && x.Address == user.Address));
            alreadyExists.Should().BeTrue();

            if (!alreadyExists)
                await _userService.InsertUserAsync(user);
        }

        [Test]
        public async Task CanDeleteUserFromFile()
        {
            await CanAddNewUserToFile();
            var initialUsers= (await _userService.GetAllUsersAsync()).Count;
            var user = new User
            {
                Email = "Mike@gmail.com"
            };

            await _userService.DeleteUserAsync(user);
            var newTotal = (await _userService.GetAllUsersAsync()).Count;

            newTotal.Should().BeLessThan(initialUsers);
        }

        #endregion
    }
}