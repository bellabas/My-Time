using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using MyTime.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MyTime.ViewModel
{
    public class MainWindowViewModel : ObservableRecipient
    {
        public ICommand DOBCommand { get; set; }
        public DateTime? DOB { get; set; }
        public string MyElapsedTime { get; set; }

        public static bool IsInDesignMode
        {
            get
            {
                var prop = DesignerProperties.IsInDesignModeProperty;
                return (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;
            }
        }

        public MainWindowViewModel() : this(IsInDesignMode ? null : Ioc.Default.GetService<IMyTimeLogic>())
        {

        }

        public MainWindowViewModel(IMyTimeLogic myTimeLogic)
        {
            DOBCommand = new RelayCommand(
            () =>
            {
                myTimeLogic.Calculate((DateTime)DOB);
            }
            );

            Messenger.Register<MainWindowViewModel, string, string>(this, "MyElapsedTimeStatus", (recipient, msg) =>
            {
                MyElapsedTime = msg;
                OnPropertyChanged(nameof(MyElapsedTime));
            });

            Messenger.Register<MainWindowViewModel, object, string>(this, "DOBStatus", (recipient, msg) =>
            {
                DOB = (DateTime)msg;
                OnPropertyChanged(nameof(DOB));
            });
        }
    }
}
