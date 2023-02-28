using System.Collections.Generic;
using System.Threading.Tasks;
using Sat.Recruitment.Core.Domain;

namespace Sat.Recruitment.Core.Services
{
    /// <summary>
    /// Represents the user service interface
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Get all users saved in txt file
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the users
        /// </returns>
        Task<IList<User>> GetAllUsersAsync();

        /// <summary>
        /// Gets an user by email
        /// </summary>
        /// <param name="email">User email</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the user
        /// </returns>
        Task<User> GetUserByEmailAsync(string email);

        /// <summary>
        /// Inserts an user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// </returns>
        Task InsertUserAsync(User user);

        /// <summary>
        /// Deletes an user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// </returns>
        Task DeleteUserAsync(User user);
    }
}