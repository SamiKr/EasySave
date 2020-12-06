using EasySaveV2.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using System.Linq;
using System.Windows;
using System.IO;

namespace EasySaveV2.ViewModel
{
    public class SaveViewModel : ObservableObject
    {
        //Collection of all saves avaiable on file
        private ObservableCollection<Save> _saveListing { get; set; }
        //Collection of viewable saves
        public ObservableCollection<Save> Saves { get; set; }

        //Model of the save we are creating, updating or removing
        private Save _saveModel { get; set; }
        //Model of the save we are currently viewing
        private Save _viewedSave;


        public Save SaveModel
        {
            get
            {
                return _saveModel;
            }
            set
            {
                _saveModel = value;
                OnPropertyChanged("SaveModel");
            }
        }


        private Save save;

        public int Id
        {
            get { return save.Id; }
            set
            {
                if (save.Id != value)
                {
                    save.Id = value;
                    new PropertyChangedEventArgs("Id");
                }
            }
        }
        public string Name
        {
            get { return save.Name; }
            set
            {
                if (save.Name != value)
                {
                    save.Name = value;
                    new PropertyChangedEventArgs("Name");
                }
            }
        }
        public string pathSource
        {
            get { return save.pathSource; }
            set
            {
                if (save.pathSource != value)
                {
                    save.pathSource = value;
                    new PropertyChangedEventArgs("pathSource");
                }
            }
        }
        public string pathDestination
        {
            get { return save.pathDestination; }
            set
            {
                if (save.pathDestination != value)
                {
                    save.pathDestination = value;
                    new PropertyChangedEventArgs("pathDestination");
                }
            }
        }
        

        //Property to access the save being viewed (See above)
        public Save ViewedSave
        {
            get { return _viewedSave; }
            set
            {
                _viewedSave = value;
                   
                OnPropertyChanged("ViewedProduct");
            }
        }
        public Save Save
        {
            get { return save; }
            set { save = value; new PropertyChangedEventArgs("Save"); }
        }

        private IList<Save> SavesList;


        private ICommand _createSave { get; set; }
        public ICommand CreateCommand
        {
            get
            {
                return _createSave;
            }
        }


        public void CreateBackup()
        {
           

            AssignId(SaveModel);
            bool AllInputsValid = false;

            if (AllInputsValid == false)
            {
                if (SaveModel.Name.Length <= 10 && SaveModel.Name.Length >= 1)
                {
                    string curFile = SaveModel.pathSource;
                    bool DirectoryExisting = Directory.Exists(curFile);
                    if (DirectoryExisting == true)
                    {
                        string curDirectory = SaveModel.pathDestination;
                        bool DirectoryExist = Directory.Exists(curDirectory);
                        if (DirectoryExist == true)
                        {
                            //Add save to save collection
                            _saveListing.Add(SaveModel);

                            Update();

                            AllInputsValid = true;
                        } 
                        else
                        {
                            MessageBox.Show("Vérifier votre chemin destination");
                        }

                     }
                    else
                    {
                        MessageBox.Show("Vérifier votre chemin source");
                    }
                }
                else
                {
                    MessageBox.Show("Le nom de la sauvegarde doit etre compris entre 1 et 10");

                }
            }


        }



        public SaveViewModel()
        {

            //Load all existing saves on file
            _saveListing = Types.LoadSaveListing();
            //Initially set viewed saves to all existing ones
            Saves = _saveListing;

            //Set a default empty save model for viewing
            SaveModel = new Save();


            //Save save = new Save();


            _createSave = new RelayCommand(CreateBackup);
           
        }

        //Assign a new ID to a save
        private void AssignId(Save save)
        {
            //Default ID if this is the first and only save
            save.Id = 1;
            if (Saves.Count > 0)
            {
                //Get the biggest ID based on value and increment form that for unique id
                save.Id = Saves.Max(i => i.Id);
                save.Id++;
            }
        }

        //Clears the save model and saves collection to file
        public void Update()
        {
            SaveModel = new Save();
            Types.SaveNewSaveListing(_saveListing);
        }



    }
}
