using System.Collections.Generic;

namespace Chat.Core
{
    /// <summary>
    /// The design-time data for a <see cref="MenuItemViewModel"/>
    /// </summary>
    public class MenuDesignModel : MenuViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static MenuDesignModel Instance => new();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MenuDesignModel()
        {
            Items = new List<MenuItemViewModel>(new[]
            {
                new MenuItemViewModel { Type = MenuItemType.Header, Text = "Design time header..." },
                new MenuItemViewModel { Text = "Menu item 1", Icon = IconType.File },
                new MenuItemViewModel { Text = "Menu item 2", Icon = IconType.Picture }
            });
        }

        #endregion
    }
}
