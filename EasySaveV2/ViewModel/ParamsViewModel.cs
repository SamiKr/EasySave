using EasySaveV2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.IO;

namespace EasySaveV2.ViewModel
{
    public class ParamsViewModel : ObservableObject
    {
        //Collection of all saves avaiable on file
        private ObservableCollection<SaveCrypted> _saveCryptedListing { get; set; }
        //Model of the save we are creating, updating or removing
        private SaveCrypted _saveCryptedModel { get; set; }

        public ObservableCollection<SaveCrypted> SavesCrypted { get; set; }

        public ObservableCollection<SaveCrypted> SavesToCrypt { get; set; }

        public SaveCrypted _viewedSaveCrypted;

        private int _SelectedIndex;

        public int SelectedIndex
        {
            get => _SelectedIndex;
            set
            {
                _SelectedIndex = value;
                File.WriteAllText("../Language.txt", value == 0 ? "en" : "fr");
            }
        }

        public SaveCrypted ViewedSaveCrypted
        {
            get { return _viewedSaveCrypted; }
            set
            {
                _viewedSaveCrypted = value;

                OnPropertyChanged("ViewedSave");
            }
        }

        public SaveCrypted SaveCryptedModel
        {
            get
            {
                return _saveCryptedModel;
            }
            set
            {
                _saveCryptedModel = value;
                OnPropertyChanged("SaveCryptedModel");
            }
        }
        
        private SaveCrypted saveCrypted;

        public string Source
        {
            get { return saveCrypted.source; }
            set
            {
                if (saveCrypted.source != value)
                {
                    saveCrypted.source = value;
                    new PropertyChangedEventArgs("Source");
                }
            }
        }
        public string Destination
        {
            get { return saveCrypted.destination; }
            set
            {
                if (saveCrypted.destination != value)
                {
                    saveCrypted.destination = value;
                    new PropertyChangedEventArgs("Destination");
                }
            }
        }
        public string Extension
        {
            get { return saveCrypted.extension; }
            set
            {
                if (saveCrypted.extension != value)
                {
                    saveCrypted.extension = value;
                    new PropertyChangedEventArgs("Extension");
                }
            }
        }
        private ICommand _createCrypting { get; set; }
        // Trigger createCrypt
        public ICommand EncrypteCommand
        {
            get
            {
                return _createCrypting;
            }
        }
        // Display view params
        public ParamsViewModel(){

            _saveCryptedListing = new ObservableCollection<SaveCrypted>();

            SavesCrypted = _saveCryptedListing;

           
            SaveCryptedModel = new SaveCrypted();
            _createCrypting = new RelayCommand(CreateCrypt);
            //SavesToCrypt = ;

            _SelectedIndex = File.ReadAllText("Language.txt") == "en" ? 0 : 1;
        }

        // Launch softWare CryptoSoft
        public void CreateCrypt()
        {
            _saveCryptedListing.Add(SaveCryptedModel);

            ProcessStartInfo startInfo = new ProcessStartInfo(@"..\..\..\CryptoSoft\CryptoSoft.exe");
            var extension = "." + SaveCryptedModel.extension;
            startInfo.CreateNoWindow = true;
            startInfo.Arguments = SaveCryptedModel.source + " " + SaveCryptedModel.destination + " " + extension;
            Process.Start(startInfo);
            MessageBox.Show("Votre cryptage a été effectué");
          
        }

    }

}