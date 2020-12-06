using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using EasySaveV2.Models;

namespace EasySaveV2.ViewModel
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static ILanguage CurrentLanguage { get; protected set; }

        static ObservableObject()
        {
            
            string text = File.ReadAllText("Language.txt");
            if (text == "en")
            {
                CurrentLanguage = new EnglishLanguage();
            }
            else
            {
                CurrentLanguage = new FrenchLanguage();
            }
            
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
}
}
