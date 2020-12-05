using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel;
using EasySaveV2.Models;
using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;

namespace EasySaveV2
{
    public enum SaveTypes
    {
        Complete,
        Differential,
    }

    public static class Types
    {
   
        public static void SaveNewSaveListing(ObservableCollection<Save> saves)
        {
               JSONController.WriteToJsonFile("saves.json", saves);
        }

        public static ObservableCollection<Save> LoadSaveListing()
        {
                var listing = JSONController.ReadFromJsonFile<ObservableCollection<Save>>("saves.json");

                if (listing == null)
                    listing = new ObservableCollection<Save>();

                return listing;
        }
        }
        
}
