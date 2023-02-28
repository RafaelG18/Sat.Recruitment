namespace Sat.Recruitment.Core.Domain
{
    /// <summary>
    /// Represents an user
    /// </summary>
    public partial class User
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the phone
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the user type identifier
        /// </summary>
        public int UserTypeId { get; set; }

        /// <summary>
        /// Gets or sets the money
        /// </summary>
        public decimal Money { get; set; }

        public UserType UserType
        {
            get => (UserType)UserTypeId;
            set => UserTypeId = (int)value;
        }
    }
}