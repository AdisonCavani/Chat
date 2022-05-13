﻿using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Chat.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Chat.ViewModel.Input;

/// <summary>
/// The view model for a text entry to edit a string value
/// </summary>
public partial class TextEntryViewModel : ObservableObject
{
    public string Label { get; set; }

    public string OriginalText { get; set; }

    public string EditedText { get; set; }

    public bool Editing { get; set; }

    public bool Working { get; set; }

    public Func<Task<bool>> CommitAction { get; set; }

    [ICommand]
    public void Edit()
    {
        // Set the edited text to the current value
        EditedText = OriginalText;

        // Go into edit mode
        Editing = true;
    }

    [ICommand]
    public void Cancel()
    {
        Editing = false;
    }

    [ICommand]
    public void Save()
    {
        // Store the result of a commit call
        var result = default(bool);

        // Save currently saved value
        var currentSavedValue = OriginalText;

        RunCommandAsync(() => Working, async () =>
        {
            // While working, come out of edit mode
            Editing = false;

            // Commit the changed text
            // So we can see it while it is working
            OriginalText = EditedText;

            // Try and do the work
            result = CommitAction == null || await CommitAction();

        }).ContinueWith(_ =>
        {
            // If we succeeded...
            // Nothing to do
            // If we fail...
            if (!result)
            {
                // Restore original value
                OriginalText = currentSavedValue;

                // Go back into edit mode
                Editing = true;
            }
        });
    }

    // TODO: remove legacy BaseViewModel helpers
    private readonly object mPropertyValueCheckLock = new();

    private async Task RunCommandAsync(Expression<Func<bool>> updatingFlag, Func<Task> action)
    {
        lock (mPropertyValueCheckLock)
        {
            if (updatingFlag.GetPropertyValue())
                return;

            updatingFlag.SetPropertyValue(true);
        }

        try
        {
            await action();
        }
        finally
        {
            updatingFlag.SetPropertyValue(false);
        }
    }
}
