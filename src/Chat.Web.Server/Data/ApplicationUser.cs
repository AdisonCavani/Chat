using Microsoft.AspNetCore.Identity;

namespace Chat.Web.Server
{
    /// <summary>
    /// The user data and profile
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        #region Public Properties

        /// <summary>
        /// The users first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The users last name
        /// </summary>
        public string LastName { get; set; }

        #endregion
    }
}
