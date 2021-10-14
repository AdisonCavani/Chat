using System.Windows;

namespace Chat
{
    /// <summary>
    /// The view model for the custom flat style
    /// </summary>
    public class WindowViewModel : BaseViewModel
    {
        #region Private Member
        /// <summary>
        /// The window this view model controls
        /// </summary>
        private Window mWindow;
        #endregion

        #region Public Properties
        public string Text { get; set; } = "My string";
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="window"></param>
        public WindowViewModel(Window window)
        {

        }
        #endregion
    }
}
