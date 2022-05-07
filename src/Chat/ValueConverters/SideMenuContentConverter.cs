using System;
using System.Globalization;
using Chat.Controls.Chat.ChatList;
using Chat.Core.DataModels;

namespace Chat.ValueConverters;

/// <summary>
/// A converter that takes a <see cref="SideMenuContent"/> and converts it to the 
/// correct UI element
/// </summary>
public class SideMenuContentConverter : BaseValueConverter<SideMenuContentConverter>
{
    /// <summary>
    /// An instance of the current chat list control
    /// </summary>
    protected ChatListControl mChatListControl = new();

    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Get the side menu type
        var sideMenuType = (SideMenuContent)value;

        // Switch based on type
        return sideMenuType switch
        {
            // Chat 
            SideMenuContent.Chat => mChatListControl,
            // Unknown
            _ => "No UI yet, sorry :)",
        };
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
