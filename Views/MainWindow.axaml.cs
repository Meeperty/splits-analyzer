using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using SplitsAnalyzer.ViewModels;
using System;

namespace SplitsAnalyzer.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Opened += (object? e, EventArgs a) =>
            {
                DataContextCast.Initialize();
            };
        }

        public MainWindowViewModel? DataContextCast
        {
            get { return (MainWindowViewModel?)DataContext; }
        }
    }
}
