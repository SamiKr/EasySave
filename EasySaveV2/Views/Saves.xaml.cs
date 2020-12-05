using EasySaveV2.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasySaveV2.Views
{
    /// <summary>
    /// Logique d'interaction pour Saves.xaml
    /// </summary>
    public partial class Saves : UserControl
    {
        SaveViewModel saveModel;
        public Saves()
        {
            InitializeComponent();
            DataContextChanged += new DependencyPropertyChangedEventHandler(View_DataContextChanged);
        }
        void View_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //Set viewmodel to the data context
            saveModel = DataContext as SaveViewModel;
            //By default, select the first product
            saveListview.SelectedIndex = 0;
            
            checkedListview.SelectedIndex = 0;
        }
    }
}
