using EasySaveV2.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace EasySaveV2.ViewModel
{
    class NewSaveViewModel : ObservableObject
    {
        //Collection of all saves avaiable on file
        private ObservableCollection<Save> _saveListing { get; set; }
        //
        private ObservableCollection<Save> _checkedListing { get; set; }
        public ObservableCollection<Save> Saves { get; set; }

        public ObservableCollection<Save> SavesChecked { get; set; }

        public Save _viewedSave;
        public Save ViewedSave
        {
            get { return _viewedSave; }
            set
            {
                _viewedSave = value;

                OnPropertyChanged("ViewedSave");
            }
        }
        //Model of the save we are creating, updating or removing
        private Save _saveModel { get; set; }


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
        private ICommand _removeSave { get; set; }
        public ICommand RemoveCommand
        {
            get
            {
                return _removeSave;
            }
        }
        private ICommand _executeSave { get; set; }
        public ICommand ExecuteCommand
        {
            get
            {
                return _executeSave;
            }
        }

        private ICommand _addList { get; set; }
        public ICommand AddListCommand
        {
            get
            {
                return _addList;
            }
        }
        private ICommand _removeCheck { get; set; }
        public ICommand RemoveCheckCommand
        {
            get
            {
                return _removeCheck;
            }
        }
        private ICommand _saveSequential { get; set; }
        public ICommand sequentialSavesCommand
        {
            get
            {
                return _saveSequential;
            }
        }
        //Remove the save passed from collection
        public void RemoveSave(object save)
        {
            //Get the product passed through parameter
            var oldSave = save as Save;
            //Remove product from collection
            _saveListing.Remove(oldSave);

            Update();
        }

        public void AddList(object save)
        {
            //Get the product passed through parameter
            var checkedSave = save as Save;
    
            bool found = false;
            foreach (var l in _checkedListing)
            {
                if (l.Id == checkedSave.Id)
                {
                    found = true;
                }
            }
            if (found == false)
            {
                _checkedListing.Add(checkedSave);
            }
            else
            {
                MessageBox.Show("Sauvegarde dèjà séléctionnée !");
            }
            
            
        }
        public void RemoveCheck(object save)
        {
            //Get the product passed through parameter
            var checkSave = save as Save;
            //Remove product from collection
            _checkedListing.Remove(checkSave);

        }

        public void executeCheckedSaves()
        {
            //int nbSaves = SavesChecked.Count();
            foreach (var saveChecked in SavesChecked)
            {

                string source = saveChecked.pathSource;
                string target = saveChecked.pathDestination;

                System.IO.Directory.CreateDirectory(target);

                if (System.IO.Directory.Exists(source))
                {
                    string[] files = System.IO.Directory.GetFiles(source);


                    for (var i = 0; i < files.Length; i++)
                    {


                        if (saveChecked.Type == SaveTypes.Complete)
                        {
                            // Use static Path methods to extract only the file name from the path.
                            string fileName = System.IO.Path.GetFileName(files[i]);
                            string destFile = System.IO.Path.Combine(target, fileName);
                            System.IO.File.Copy(files[i], destFile, true);
                            
                        }
                        /*else if (savetoexecute.Type.Equals("Differential"))
                        {

                        }*/
                    }
                }
            }
            MessageBox.Show("Vos sauvegardes ont été effectuées");
            _checkedListing.Clear();
        }
        public void ExecuteSave(object save)
        {
            var savetoexecute = save as Save;
            // If the directory already exists, this method does not create a new directory.
        
            string source = savetoexecute.pathSource;
            string target = savetoexecute.pathDestination;

            System.IO.Directory.CreateDirectory(target);

            if (System.IO.Directory.Exists(source))
            {
                
                string[] files = System.IO.Directory.GetFiles(source);
                string[] filest = System.IO.Directory.GetFiles(target);

                for (var i = 0; i < files.Length; i++)
                {

                    bool detect = ProcessDetecte();
                    if (detect == false)
                    {
                        if (savetoexecute.Type == SaveTypes.Complete)
                        {
                            // Use static Path methods to extract only the file name from the path.
                            string fileName = System.IO.Path.GetFileName(files[i]);
                            string destFile = System.IO.Path.Combine(target, fileName);
                            DateTime now = DateTime.Now;
                            System.IO.File.Copy(files[i], destFile, true);
                            DateTime after = DateTime.Now;
                            System.TimeSpan diff = after - now;
                            CreateLogs("Created new save", savetoexecute.Name, savetoexecute.pathSource, savetoexecute.pathDestination, diff);
                            List<string> remainingFiles = new List<string>();
                            int res = 0;
                            for (var y = i; y < files.Length; y++)
                            {
                                remainingFiles.Add(files[y]);
                                res += files[y].Length;
                            }

                            // Timespan of the transfert
                            diff = after - now;
                            string remainingSize = res + " bytes";
                            CreateState(savetoexecute.Name, savetoexecute.pathSource, savetoexecute.pathDestination, diff, "actif", files.Length, "100%", remainingFiles.Count, remainingSize);


                        }
                        else if (savetoexecute.Type == SaveTypes.Differential)
                        {

                            if (files.Length != filest.Length)
                            {
                                string fileName2 = System.IO.Path.GetFileName(files[i]);
                                string destFile2 = System.IO.Path.Combine(target, fileName2);
                                List<String> listNew = new List<string>();

                                bool end = false;

                                for (var y = 0; y < files.Length; y++)
                                {
                                    if (end == false)
                                    {
                                        for (var h = 0; h < filest.Length; h++)
                                        {
                                            if (System.IO.Path.GetFileName(files[y]) == System.IO.Path.GetFileName(filest[h]))
                                            {
                                                end = true;

                                                DateTime fs = System.IO.Directory.GetLastWriteTime(files[i]);
                                                DateTime fst = System.IO.Directory.GetLastWriteTime(filest[i]);
                                                if (fs != fst)
                                                {

                                                    // Use static Path methods to extract only the file name from the path.


                                                    DateTime now = DateTime.Now;
                                                    System.IO.File.Copy(files[i], destFile2, true);
                                                    DateTime after = DateTime.Now;
                                                    System.TimeSpan diff = after - now;
                                                    CreateLogs("Created new save", savetoexecute.Name, savetoexecute.pathSource, savetoexecute.pathDestination, diff);
                                                    List<string> remainingFiles = new List<string>();
                                                    int res = 0;

                                                    for (var b = i; b < files.Length; b++)
                                                    {


                                                        remainingFiles.Add(files[b]);

                                                        res += files[b].Length;
                                                    }

                                                    diff = after - now;
                                                    string remainingSize = res + " bytes";
                                                    CreateState(savetoexecute.Name, source, destFile2, diff, "Actif", files.Length, "100%", remainingFiles.Count, remainingSize);
                                                }
                                                else
                                                {

                                                    CreateState(savetoexecute.Name, source, destFile2, TimeSpan.Zero, "Actif", files.Length, "100%", 0, "0");
                                                }
                                            }
                                        }
                                        if (end == false)
                                        {
                                            listNew.Add(files[y]);
                                        }

                                    }
                                }
                                for (var j = 0; j < listNew.Count; j++)
                                {
                                    DateTime now = DateTime.Now;
                                    System.IO.File.Copy(listNew[j], destFile2, true);
                                    DateTime after = DateTime.Now;
                                    System.TimeSpan diff = after - now;
                                    CreateLogs("Created New Save", savetoexecute.Name, savetoexecute.pathSource, savetoexecute.pathDestination, diff);
                                    List<string> remainingFiles = new List<string>();
                                    int res = 0;

                                    for (var y = i; y < files.Length; y++)
                                    {


                                        remainingFiles.Add(files[y]);

                                        res += files[y].Length;
                                    }
                                    diff = after - now;
                                    string remainingSize = res + " bytes";
                                    CreateState(savetoexecute.Name, source, destFile2, diff, "Actif", files.Length, "100%", remainingFiles.Count, remainingSize);
                                }
                            }
                            else
                            {
                                DateTime fs = System.IO.Directory.GetLastWriteTime(files[i]);
                                DateTime fst = System.IO.Directory.GetLastWriteTime(filest[i]);
                                string fileName2 = System.IO.Path.GetFileName(files[i]);
                                string destFile2 = System.IO.Path.Combine(target, fileName2);
                                if (fs != fst)
                                {

                                    // Use static Path methods to extract only the file name from the path.

                                    DateTime now = DateTime.Now;
                                    System.IO.File.Copy(files[i], destFile2, true);
                                    DateTime after = DateTime.Now;
                                    System.TimeSpan diff = after - now;
                                    CreateLogs("Created New Save", savetoexecute.Name, savetoexecute.pathSource, savetoexecute.pathDestination, diff);
                                    List<string> remainingFiles = new List<string>();
                                    int res = 0;

                                    for (var y = i; y < files.Length; y++)
                                    {


                                        remainingFiles.Add(files[y]);

                                        res += files[y].Length;
                                    }
                                    diff = after - now;
                                    string remainingSize = res + " bytes";
                                    CreateState(savetoexecute.Name, source, destFile2, diff, "Actif", files.Length, "100%", remainingFiles.Count, remainingSize);
                                }
                                else
                                {

                                    CreateState(savetoexecute.Name, source, destFile2, TimeSpan.Zero, "Actif", files.Length, "100%", 0, "0");
                                }
                            }

                        }
                        string[] directories = System.IO.Directory.GetDirectories(source);
                        foreach (string d in directories)
                        {

                            string directory = System.IO.Path.GetFileName(d);
                            //Console.WriteLine("Directory source : " + d + "\n");
                            string newFolder = target + "\\" + directory;
                            //Console.WriteLine("Directory destination : " + newFolder + "\n");
                            System.IO.Directory.CreateDirectory(newFolder);

                            string myname = savetoexecute.Name;
                            string mysource = d;
                            string mytarget = newFolder;
                            string mytype = savetoexecute.Type.ToString();
                            int type;
                            if (mytype == "Complete")
                            {
                                type = 1;
                            }
                            else
                            {
                                type = 2;
                            }
                            executeSaveDirectory(myname, mysource, mytarget, type);
                        }
                        MessageBox.Show("Votre sauvegarde a été effectuée");
                    }

                    else
                    {
                        MessageBox.Show("Impossible de lancer la sauvegarde, merci de fermer les logiciels métier");
                    }
                    
                }
            
            }
       
        }
         
             
        
        public NewSaveViewModel(){

            //Load all existing saves on file
            _saveListing = Types.LoadSaveListing();

            // initialize list
            _checkedListing = new ObservableCollection<Save>();
 
            //Initially set viewed saves to all existing ones
            Saves = _saveListing;
            
            SavesChecked = _checkedListing;
            
            //
            _removeSave = new RelayCommand(RemoveSave);
            _executeSave = new RelayCommand(ExecuteSave);
            _addList = new RelayCommand(AddList);
            _removeCheck = new RelayCommand(RemoveCheck);
            _saveSequential = new RelayCommand(executeCheckedSaves);
        }

        //Clears the save model and saves collection to file
        public void Update()
        {
            SaveModel = new Save();
            Types.SaveNewSaveListing(_saveListing);
        }

        public void CreateLogs(string logName, string name, string pathSource, string pathDestination, TimeSpan timeElapsed)
        {
            // Recup date & time of the day
            DateTime dateTime = DateTime.Now;
            // Formalize the date for json filename
            string today = DateTime.Now.ToString("dd-MM-yyyy");

            // Creating JObject to stock all the elements 
            JObject newObject = new JObject();
            newObject.Add("LogName", logName);
            newObject.Add("name", name);
            newObject.Add("pathSource", pathSource);
            newObject.Add("pathDestination", pathDestination);
            newObject.Add("dateTime", dateTime);
            newObject.Add("timeElapsed", timeElapsed);

            ObservableCollection<Save> save;
            //Verify if the folder exist
            // If not, he create the folder


            // Verify if the file with the date of the day exists or not
            // If it doesn't exist, creation/initiialization of the file
            // Write all of the elements in the json file
            string logsFolder = Environment.GetEnvironmentVariable("PathLog");
            if (System.IO.Directory.Exists(logsFolder) == false)
            {
                Directory.CreateDirectory(logsFolder);
            }

            string logFileName = Environment.GetEnvironmentVariable("PathLog") + "Logs-" + today + ".json";

            if (System.IO.File.Exists(logFileName))
            {
                
                Write(newObject, logFileName);

            }
            else
            {
                System.IO.File.WriteAllText(logFileName, "[ ]");
          
                Write(newObject, logFileName);

            }


        }
        public void CreateState(string name, string pathSource, string pathDestination, TimeSpan timeElapsed, string state, int size, string progression, int remainingFiles, string remainingSize)
        {
            // Recup date & time of the day
            DateTime dateTime = DateTime.Now;
            // Formalize the date for json filename
            string today = DateTime.Now.ToString("dd-MM-yyyy");

            // Creating JObject to stock all the elements
            JObject newObject = new JObject();
            newObject.Add("name", name);
            newObject.Add("pathSource", pathSource);
            newObject.Add("pathDestination", pathDestination);
            newObject.Add("dateTime", dateTime);
            newObject.Add("timeElapsed", timeElapsed);
            newObject.Add("state", state); //Actif non actif
            newObject.Add("size", size);
            newObject.Add("progression", progression);
            newObject.Add("remaining Files", remainingFiles);
            newObject.Add("remaining Size", remainingSize);

            // Verify if the file exists or not
            // If it doesn't exist, creation/initiialization of the file
            // Write all of the elements in the json file
            string saveFolder = Environment.GetEnvironmentVariable("PathState");
            if (System.IO.Directory.Exists(saveFolder) == false)
            {
                Directory.CreateDirectory(saveFolder);
            }
            string stateFileName = Environment.GetEnvironmentVariable("PathState") + "\\States.json";
            if (System.IO.File.Exists(stateFileName))
            {
                 Write(newObject, stateFileName);
            }
            else
            {
                System.IO.File.WriteAllText(stateFileName, "[ ]");
        
                Write(newObject, stateFileName);

            }



        }
        public void executeSaveDirectory(string name, string source, string target, int type)
        {
            

            // To copy a folder's contents to a new location:
            // Create a new target folder.
            // If the directory already exists, this method does not create a new directory.
            System.IO.Directory.CreateDirectory(target);

            // To copy all the files in one directory to another directory.
            // Get the files in the source folder. (To recursively iterate through
            // all subfolders under the current directory, see
            // "How to: Iterate Through a Directory Tree.")
            // Note: Check for target path was performed previously
            //       in this code example.
            if (System.IO.Directory.Exists(source))
            {
                string[] files = System.IO.Directory.GetFiles(source);
                string[] filest = System.IO.Directory.GetFiles(target);


                for (var i = 0; i < files.Length; i++)
                {

                    if (type == 1)
                    {
                        //Console.WriteLine("Full backup");

                        //Console.WriteLine("File : " + files[i] + "\n");
                        // Use static Path methods to extract only the file name from the path.
                        string fileName = System.IO.Path.GetFileName(files[i]);
                        string destFile = System.IO.Path.Combine(target, fileName);
                        //Console.WriteLine("File copied " + files[i] + "\n");
                        DateTime now = DateTime.Now;
                        System.IO.File.Copy(files[i], destFile, true);
                        DateTime after = DateTime.Now;
                        System.TimeSpan diff = after - now;
                        CreateLogs("Created New Save", name, source, target, diff);
                        List<string> remainingFiles = new List<string>();
                        int res = 0;

                        for (var y = i; y < files.Length; y++)
                        {


                            remainingFiles.Add(files[y]);

                            res += files[y].Length;
                        }
                        diff = after - now;
                        string remainingSize = res + " bytes";
                        CreateState(name, source, destFile, diff, "Actif", files.Length, "100%", remainingFiles.Count, remainingSize);
                    }
                    if (type == 2)
                    {
                        //Console.WriteLine("Differential backup");

                        if (files.Length != filest.Length)
                        {
                            string fileName2 = System.IO.Path.GetFileName(files[i]);
                            string destFile2 = System.IO.Path.Combine(target, fileName2);
                            List<String> listNew = new List<string>();

                            bool end = false;

                            for (var y = 0; y < files.Length; y++)
                            {
                                if (end == false)
                                {
                                    for (var h = 0; h < filest.Length; h++)
                                    {
                                        if (System.IO.Path.GetFileName(files[y]) == System.IO.Path.GetFileName(filest[h]))
                                        {
                                            end = true;

                                            DateTime fs = System.IO.Directory.GetLastWriteTime(files[i]);
                                            DateTime fst = System.IO.Directory.GetLastWriteTime(filest[i]);
                                            if (fs != fst)
                                            {
                                                //Console.WriteLine("date source : " + fs + "\n");
                                                //Console.WriteLine("date destination : " + fst + "\n");
                                                //Console.WriteLine("File : " + files[i] + "\n");
                                                // Use static Path methods to extract only the file name from the path.

                                                //Console.WriteLine("File copied " + files[i] + "\n");
                                                DateTime now = DateTime.Now;
                                                System.IO.File.Copy(files[i], destFile2, true);
                                                DateTime after = DateTime.Now;
                                                System.TimeSpan diff = after - now;
                                                CreateLogs("Created New Save", name, source, target, diff);
                                                List<string> remainingFiles = new List<string>();
                                                int res = 0;

                                                for (var b = i; b < files.Length; b++)
                                                {


                                                    remainingFiles.Add(files[b]);

                                                    res += files[b].Length;
                                                }
                                                diff = after - now;
                                                string remainingSize = res + " bytes";
                                                CreateState(name, source, destFile2, diff, "Actif", files.Length, "100%", remainingFiles.Count, remainingSize);
                                            }
                                            else
                                            {
                                                //Console.WriteLine("No change for this file \n");
                                                CreateState(name, source, destFile2, TimeSpan.Zero, "Actif", files.Length, "100%", 0, "0");
                                            }
                                        }
                                    }
                                    if (end == false)
                                    {
                                        listNew.Add(files[y]);
                                    }

                                }
                            }
                            for (var j = 0; j < listNew.Count; j++)
                            {
                                DateTime now = DateTime.Now;
                                System.IO.File.Copy(listNew[j], destFile2, true);
                                DateTime after = DateTime.Now;
                                System.TimeSpan diff = after - now;
                                CreateLogs("Created New Save", name, source, target, diff); ;
                                List<string> remainingFiles = new List<string>();
                                int res = 0;

                                for (var y = i; y < files.Length; y++)
                                {


                                    remainingFiles.Add(files[y]);

                                    res += files[y].Length;
                                }
                                diff = after - now;
                                string remainingSize = res + " bytes";
                                CreateState(name, source, destFile2, diff, "Actif", files.Length, "100%", remainingFiles.Count, remainingSize);
                            }
                        }
                        else
                        {
                            DateTime fs = System.IO.Directory.GetLastWriteTime(files[i]);
                            DateTime fst = System.IO.Directory.GetLastWriteTime(filest[i]);
                            string fileName2 = System.IO.Path.GetFileName(files[i]);
                            string destFile2 = System.IO.Path.Combine(target, fileName2);
                            if (fs != fst)
                            {
                                //Console.WriteLine("date source : " + fs + "\n");
                                //Console.WriteLine("date destination : " + fst + "\n");
                                //Console.WriteLine("File : " + files[i] + "\n");
                                // Use static Path methods to extract only the file name from the path.

                                //Console.WriteLine("File copied " + files[i] + "\n");
                                DateTime now = DateTime.Now;
                                System.IO.File.Copy(files[i], destFile2, true);
                                DateTime after = DateTime.Now;
                                System.TimeSpan diff = after - now;
                                CreateLogs("Created New Save", name, source, target, diff);
                                List<string> remainingFiles = new List<string>();
                                int res = 0;

                                for (var y = i; y < files.Length; y++)
                                {


                                    remainingFiles.Add(files[y]);

                                    res += files[y].Length;
                                }
                                diff = after - now;
                                string remainingSize = res + " bytes";
                                CreateState(name, source, destFile2, diff, "Actif", files.Length, "100%", remainingFiles.Count, remainingSize);
                            }
                            else
                            {
                                //Console.WriteLine("No change for this file \n");
                                CreateState(name, source, destFile2, TimeSpan.Zero, "Actif", files.Length, "100%", 0, "0");
                            }
                        }

                    }


                }



                string[] directories = System.IO.Directory.GetDirectories(source);
                foreach (string d in directories)
                {

                    string directory = System.IO.Path.GetFileName(d);
                    //Console.WriteLine("Directory source : " + d + "\n");
                    string newFolder = target + '\\' + directory;
                    //Console.WriteLine("Directory destination : " + newFolder + "\n");
                    System.IO.Directory.CreateDirectory(newFolder);

                    executeSaveDirectory(name, d, newFolder, type);
                }
            }
            else
            {
                Console.WriteLine("Source path does not exist! \n");
            }

        }
        public JArray Read(string file)
        {
            string text = File.ReadAllText(file);
            List<Save> result = JsonConvert.DeserializeObject<List<Save>>(text);
            var obj = JArray.Parse(text);
            return obj;
        }
        // Concatenation of objects and writing in the json file
        public void Write(JObject obj, string file)
        {
            JArray array = Read(file);
            array.Add(obj);
            File.WriteAllText(file, array.ToString());
        }
        static bool ProcessDetecte()
        {
            List<string> para = new List<string>();
            string FileToRead = Environment.GetEnvironmentVariable("BusinessProcess");
            List<string> Processarray = new List<string>();
            bool detect = false;
            using (StreamReader ReaderObject = new StreamReader(FileToRead))
            {
                string Line;
                while ((Line = ReaderObject.ReadLine()) != null)
                {
                    para.Add(Line);

                }
            }

            Process[] pro = Process.GetProcesses();
            foreach (Process p in pro)
            {
                Processarray.Add(p.ProcessName);
            }
   

            for (var j = 0; j < Processarray.Count; j++)
            {
                for (var y = 0; y < para.Count; y++)
                {
                    if (Processarray[j] == para[y])
                    {
                        detect = true;
                    }
                }
            }
            return detect;
        }
    }
}
