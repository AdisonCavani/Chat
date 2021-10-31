using System.Windows.Input;

namespace Chat.Core
{
    /// <summary>
    /// The profile state as a view model
    /// </summary>
    public class ProfileViewModel : BaseViewModel
    {
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
