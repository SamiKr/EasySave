using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using EasySaveV2.Models;

namespace EasySaveV2.ViewModel
{
    // Notify view after modification
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static ILanguage CurrentLanguage { get; protected set; }



        static ObservableObject()
        {
            var tmp = System.IO.Path.Combine(System.IO.Path.GetPathRoot(Environment.SystemDirectory), "EasySave");
            System.IO.Directory.CreateDirectory(tmp);
            var file = System.IO.Path.Combine(tmp, "Language.txt");
            if (System.IO.File.Exists(file) == false)
                System.IO.File.WriteAllText(file, "en");
            string text = File.ReadAllText(Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "EasySave", "Language.txt"));
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
