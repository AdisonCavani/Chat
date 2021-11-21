using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace Chat
{
    /// <summary>
    /// Interaction logic for ChatPage.xaml
    /// </summary>
    public partial class ChatPage : BasePage<ChatMessageListViewModel>, IComponentConnector
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ChatPage() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page</param>
        public ChatPage(ChatMessageListViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Fired when the view model changes
        /// </summary>
        protected override void OnViewModelChanged()
        {
            // Make sure UI exists first
            if (ChatMessageList == null)
                return;

            // Fade in chat message list
            // TODO: remove fade in
            var storyboard = new Storyboard();
            storyboard.AddFadeIn(1, from: true);
            storyboard.Begin(ChatMessageList);

            // Make the message box focused
            MessageText.Focus();
        }

        #endregion

        /// <summary>
        /// Preview the input into the message box and respond as required
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageText_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // Get the text box
            var textbox = sender as TextBox;

            // Check if we have pressed enter
            if (e.Key == Key.Enter)
            {
                // If we have control pressed
                if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
                {
                    // Add a new line at the point where the cursor is
                    var index = textbox.CaretIndex;

                    // Insert the new line
                    textbox.Text = textbox.Text.Insert(index, Environment.NewLine);

                    // Shift the caret forward to the new line
                    textbox.CaretIndex = index + Environment.NewLine.Length;
                }

                else
                    // Send the message
                    ViewModel.Send();

                // Mark this key as handled
                e.Handled = true;
            }
        }
    }
}
