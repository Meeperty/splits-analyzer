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
        
        public void Initialize()
        {
            splits = new(@"C:\Users\Us\source\repos\SplitsAnalyzer\bin\Debug\net6.0\testing.lss", this);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GameName)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CategoryName)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AttemptCount)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastError)));
        }

        public string GameName
        {
            get 
            {
                if (splits != null)
                    return splits.gameName;
                else
                    return "Not Yet Initialized";
            }
        }

        public string CategoryName
        {
            get
            {
                if (splits != null)
                    return splits.categoryName;
                else
                    return "Not Yet Initialized";
            }
        }

        public string AttemptCount
        {
            get
            {
                if (splits != null)
                    return splits.attemptCount + " attempts";
                else
                    return "Not Yet Initialized";
            }
        }

        public string LastError
        {
            get
            {
                if (splits != null)
                    return splits.lastError;
                else
                    return "Not Yet Initialized";
            }
        }
        
        //file path to splits
        private string splitsPath = @"C:\Users\Us\source\repos\SplitsAnalyzer\bin\Debug\net6.0\testing.lss";
        public string SplitsPath
        {
            get { return splitsPath; }
            set 
            { 
                this.RaiseAndSetIfChanged(ref splitsPath, value); 
                splits = new(splitsPath, this);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GameName)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CategoryName)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AttemptCount)));
            }
        }

        public void UpdateSplitsError()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastError)));
        }

        internal Splits splits;
    }
}
