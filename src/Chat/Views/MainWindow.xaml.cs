﻿using System;
using Chat.WPFViewModels;

namespace Chat.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();

        DataContext = new WindowViewModel();
    }

    private void AppWindow_Deactivated(object sender, EventArgs e)
    {
        // Show overlay if we lose focus
        (DataContext as WindowViewModel).DimmableOverlayVisible = true;
    }

    private void AppWindow_Activated(object sender, EventArgs e)
    {
        // Hide overlay if we are focused
        (DataContext as WindowViewModel).DimmableOverlayVisible = false;
    }
}