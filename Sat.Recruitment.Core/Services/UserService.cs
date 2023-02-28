using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sat.Recruitment.Core.Domain;
using Sat.Recruitment.Core.Infrastructure;

namespace Sat.Recruitment.Core.Services
{
    /// <summary>
    /// Represents the user service implementation
    /// </summary>
    public class UserService : IUserService
    {
        #region Constants

        private const string USER_FILE_PATH = "/Files/Users.txt";

        #endregion

        #region Fields

        private readonly IFileProvider _fileProvider;

        #endregion

        #region Ctor

        public UserService(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        #endregion

        #region Utilities

        protected async Task<IEnumerable<string>> GetAllUsersFileLinesAsync()
        {
            return await _fileProvider.ReadAllLinesAsync($"{_fileProvider.ContentRootPath}{USER_FILE_PATH}");
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get all users saved in txt file
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the users
        /// </returns>
        public async Task<IList<User>> GetAllUsersAsync()
        {
            var fileLines = (await GetAllUsersFileLinesAsync()) ?? new List<string>();

            return fileLines.Select(x =>
            {
                var splited = x.Split(",");
                _ = int.TryParse(splited[4], out var type);
                _ = decimal.TryParse(splited[5], out var money);
                return new User
                {
                    Name = splited[0],
                    Email = splited[1],
                    Phone = splited[2],
                    Address = splited[3],
                    UserTypeId = type,
                    Money = money
                };
            }).ToList();
        }

        /// <summary>
        /// Gets an user by email
        /// </summary>
        /// <param name="email">User email</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the user
        /// </returns>
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return (await GetAllUsersAsync()).FirstOrDefault(x => x.Email.ToUpperInvariant().Trim() == email.ToUpperInvariant().Trim());
        }

        /// <summary>
        /// Inserts an user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// </returns>
        public async Task InsertUserAsync(User user)
        {
            var line = $"{user.Name},{user.Email},{user.Phone},{user.Address},{user.UserTypeId},{user.Money}".Trim();
            await _fileProvider.AppendAllLinesAsync($"{_fileProvider.ContentRootPath}{USER_FILE_PATH}", new[] { line });
        }

        /// <summary>
        /// Deletes an user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// </returns>
        public async Task DeleteUserAsync(User user)
        {
            var fileLines = (await GetAllUsersFileLinesAsync()).Where(x => !x.Contains(user.Email));
            await _fileProvider.WriteAllLinesAsync($"{_fileProvider.ContentRootPath}{USER_FILE_PATH}", fileLines);
        }

        #endregion
    }
}