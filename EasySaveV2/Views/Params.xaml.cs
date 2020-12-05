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
    /// Logique d'interaction pour Params.xaml
    /// </summary>
    public partial class Params : UserControl
    {

        ParamsViewModel saveCryptedModel;
        public Params()
        {
            InitializeComponent();
            DataContextChanged += new DependencyPropertyChangedEventHandler(View_DataContextChanged);
        }
        void View_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //Set viewmodel to the data context
            saveCryptedModel = DataContext as ParamsViewModel;
            //By default, select the first product
            //saveCryptedListview.SelectedIndex = 0;

            //cryptedListview.SelectedIndex = 0;
        }
    }
}
