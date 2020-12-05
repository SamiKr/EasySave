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
    /// Logique d'interaction pour NewSave.xaml
    /// </summary>
    public partial class NewSave : UserControl
    {
        SaveViewModel saveModel;
        public NewSave()
        {
            InitializeComponent();
            //Assign event call when data context is set
            DataContextChanged += new DependencyPropertyChangedEventHandler(View_DataContextChanged);
        }
        //Set 'ViewModel'
        void View_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //Set viewmodel to the data context
            saveModel = DataContext as SaveViewModel;
            //By default, select the first product
        
        }


        private void typeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
