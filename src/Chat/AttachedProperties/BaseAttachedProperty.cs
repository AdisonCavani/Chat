using System;
using System.Windows;

namespace Chat.AttachedProperties;

/// <summary>
/// A base attached property to replace the vanilla WPF attached property
/// </summary>
/// <typeparam name="TParent">The parent class to be the attached property</typeparam>
/// <typeparam name="TProperty">The type of this attached property</typeparam>
public abstract class BaseAttachedProperty<TParent, TProperty>
    where TParent : new()
{
    /// <summary>
    /// Fired when the value changes
    /// </summary>
    public event Action<DependencyObject, DependencyPropertyChangedEventArgs> ValueChanged = (_, _) => { };

    /// <summary>
    /// Fired when the value changes, even when the value is the same
    /// </summary>
    public event Action<DependencyObject, object> ValueUpdated = (_, _) => { };

    /// <summary>
    /// A singleton instance of our parent class
    /// </summary>
    public static TParent Instance { get; private set; } = new();

    /// <summary>
    /// The attached property for this class
    /// </summary>
    public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached(
        "Value",
        typeof(TProperty),
        typeof(BaseAttachedProperty<TParent, TProperty>),
        new UIPropertyMetadata(
            default(TProperty),
            OnValuePropertyChanged,
            OnValuePropertyUpdated
        ));

    /// <summary>
    /// The callback event when the <see cref="ValueProperty"/> is changed
    /// </summary>
    /// <param name="d">The UI element that had it's property changed</param>
    /// <param name="e">The arguments for the event</param>
    private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // Call the parent function
        (Instance as BaseAttachedProperty<TParent, TProperty>)?.OnValueChanged(d, e);

        // Call event listeners
        (Instance as BaseAttachedProperty<TParent, TProperty>)?.ValueChanged(d, e);
    }

    /// <summary>
    /// The callback event when the <see cref="ValueProperty"/> is changed, even if it is the same value
    /// </summary>
    /// <param name="d">The UI element that had it's property changed</param>
    /// <param name="value">The arguments for the event</param>
    private static object OnValuePropertyUpdated(DependencyObject d, object value)
    {
        // Call the parent function
        (Instance as BaseAttachedProperty<TParent, TProperty>)?.OnValueUpdated(d, value);

        // Call event listeners
        (Instance as BaseAttachedProperty<TParent, TProperty>)?.ValueUpdated(d, value);

        // Return the value
        return value;
    }

    /// <summary>
    /// Gets the attached property
    /// </summary>
    /// <param name="d">The element to get the property from</param>
    /// <returns></returns>
    public static TParent GetValue(DependencyObject d) => (TParent)d.GetValue(ValueProperty);

    /// <summary>
    /// Sets the attached property
    /// </summary>
    /// <param name="d">The element to get the property from</param>
    /// <param name="value">The value to set the property to</param>
    public static void SetValue(DependencyObject d, TProperty value) => d.SetValue(ValueProperty, value);

    /// <summary>
    /// The method that is called when any attached property of this type is changed
    /// </summary>
    /// <param name="sender">The UI element that this property was changed for</param>
    /// <param name="e">The arguments for this event</param>
    protected virtual void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) { }

    /// <summary>
    /// The method that is called when any attached property of this type is changed, even if the value is the same
    /// </summary>
    /// <param name="sender">The UI element that this property was changed for</param>
    /// <param name="value">The arguments for this event</param>
    protected virtual void OnValueUpdated(DependencyObject sender, object value) { }
}
