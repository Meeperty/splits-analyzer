using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using ReactiveUI;
using SplitsAnalyzer.Models;

namespace SplitsAnalyzer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        
        public string GameName
        {
            get { return splits.gameName; }
        }

        public string CategoryName
        {
            get { return splits.categoryName; }
        }

        public string AttemptCount
        {
            get { return  splits.attemptCount.ToString() + " Attempts"; }
        }
        
        //file path to splits
        private string splitsPath = @"C:\Users\Us\source\repos\SplitsAnalyzer\bin\Debug\net6.0\testing.lss";
        public string SplitsPath
        {
            get { return splitsPath; }
            set 
            { 
                this.RaiseAndSetIfChanged(ref splitsPath, value); 
                splits = new(splitsPath);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GameName)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CategoryName)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AttemptCount)));
            }
        }
        
        internal Splits splits = new(@"C:\Users\Us\source\repos\SplitsAnalyzer\bin\Debug\net6.0\testing.lss");
    }
}
