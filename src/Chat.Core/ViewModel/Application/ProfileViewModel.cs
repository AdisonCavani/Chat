using System.Windows.Input;

namespace Chat.Core
{
    /// <summary>
    /// The profile state as a view model
    /// </summary>
    public class ProfileViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The current users name
        /// </summary>
        public TextEntryViewModel Name { get; set; }

        /// <summary>
        /// The current users username
        /// </summary>
        public TextEntryViewModel Username { get; set; }

        /// <summary>
        /// The current users password
        /// </summary>
        public TextEntryViewModel Password { get; set; }

        /// <summary>
        /// The current users email
        /// </summary>
        public TextEntryViewModel Email { get; set; }

        #endregion

        #region Public Commands

        /// <summary>
        /// The command to close the profile menu
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        /// The command to open the profile menu
        /// </summary>
        public ICommand OpenCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProfileViewModel()
        {
            // Create commands
            CloseCommand = new RelayCommand(Close);
            OpenCommand = new RelayCommand(Open);

            // TODO: Remove this with real information pulled from our database in future
            Name = new TextEntryViewModel { Label = "Name", OriginalText = "Adison Cavani" };
            Username = new TextEntryViewModel { Label = "Username", OriginalText = "adison" };
            Password = new TextEntryViewModel { Label = "Password", OriginalText = "*************" };
            Email = new TextEntryViewModel { Label = "Email", OriginalText = "test@email.com" };
        }

        #endregion

        /// <summary>
        /// Closes the profile menu
        /// </summary>
        public void Close()
        {
            // Close the profile menu
            IoC.Application.ProfileMenuVisible = false;
        }

        /// <summary>
        /// Closes the profile menu
        /// </summary>
        public void Open()
        {
            // Open the profile menu
            IoC.Application.ProfileMenuVisible = true;
        }
    }
}
