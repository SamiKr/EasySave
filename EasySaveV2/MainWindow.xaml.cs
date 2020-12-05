using EasySaveV2.ViewModel;
using EasySaveV2.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasySaveV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SaveViewModel saveModel;
        public MainWindow()
        {
            InitializeComponent();
            ConfirmFileDependencies();
            this.DataContext = new SaveViewModel();

        }
        private void Saves_Clicked(object sender, RoutedEventArgs e)
        {
            this.DataContext = new NewSaveViewModel();
        }

        private void NewSaves_Clicked(object sender, RoutedEventArgs e)
        {
            this.DataContext = new SaveViewModel();
        }
        private void Params_Clicked(object sender, RoutedEventArgs e)
        {
            this.DataContext = new ParamsViewModel();
        }


        private void ConfirmFileDependencies()
        { 
            if (!File.Exists("saves.json"))
            {
                File.Create("saves.json");
            }
        }
    }
}
