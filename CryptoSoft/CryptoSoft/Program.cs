using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;


namespace CryptoSoft
{
    class Program
    {
        public static void Main(string[] args)
        {
            Encrypt(args[0], args[1], args[2]);
        }


        public static void CreateLogs(string logName, string name, string pathSource, string pathDestination, TimeSpan timeElapsed, TimeSpan TimeCrypting)
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
            newObject.Add("timeCrypting", TimeCrypting);

            //Verify if the folder exist
            // If not, he create the folder


            // Verify if the file with the date of the day exists or not
            // If it doesn't exist, creation/initiialization of the file
            // Write all of the elements in the json file
            string logsFolder = Environment.GetEnvironmentVariable("PathLog");
            if (System.IO.Directory.Exists(logsFolder) == false)
            {
                Console.WriteLine("create folder logs");
                Directory.CreateDirectory(logsFolder);
            }

            string logFileName = Environment.GetEnvironmentVariable("PathLog") + "Logs-" + today + ".json";

            if (System.IO.File.Exists(logFileName))
            {
                string file = Environment.GetEnvironmentVariable("PathLog") + "Logs-" + today + ".json";
                //Console.WriteLine(file);
                Write(newObject, file);

            }
            else
            {
                System.IO.File.WriteAllText(logFileName, "[ ]");
                //Console.WriteLine(logFileName);
                Write(newObject, logFileName);

            }

            //Console.WriteLine("Your Logs has been created");
        }



        public static void Encrypt(string source, string destination, string extension)
        {
            string key = "ThatAnImpressivKey";
            System.IO.Directory.CreateDirectory(destination);
            if (System.IO.Directory.Exists(source))
            {
                string[] files = System.IO.Directory.GetFiles(source);
                foreach (string f in files)
                {
                    if (Path.GetExtension(f) == extension)
                    {
                        DateTime now = DateTime.Now;
                        string text = File.ReadAllText(f);
                        string fileName = Path.GetFileName(f);
                        var result = new StringBuilder();
                        Stopwatch stopWatch = new Stopwatch();
                        for (int c = 0; c < text.Length; c++)
                        {
                            stopWatch.Start();
                            result.Append((char)((uint)text[c] ^ (uint)key[c % key.Length]));
                        }
                        string destFile = destination + "\\" + fileName;
                        File.WriteAllText(destFile, result.ToString());
                        stopWatch.Stop();
                        DateTime after = DateTime.Now;
                        System.TimeSpan ts = stopWatch.Elapsed;
                        string txt=null;
                        if (ts.TotalMilliseconds == 0)
                            txt = "No encryption";
                        if (ts.TotalMilliseconds > 0)
                            txt = "Successfull encryptation";
                        if (ts.TotalMilliseconds < 0)
                            txt = "Failed encryptation";
                        CreateLogs("Cryptage : " + fileName, txt, source, destination, TimeSpan.Zero, ts);
                        
                    }
                }

                string[] directories = System.IO.Directory.GetDirectories(source);
                foreach (string d in directories)
                {
                    string directory = System.IO.Path.GetFileName(d);
                    string newFolder = destination + '\\' + directory;
                    System.IO.Directory.CreateDirectory(newFolder);
                    Encrypt(d, newFolder, extension);
                }
            }
            else
            {
                Console.WriteLine("Source path does not exist! \n");
            }
        }
        public static JArray Read(string file)
        {
            string text = File.ReadAllText(file);
            List<Object> result = JsonConvert.DeserializeObject<List<Object>>(text);
            var obj = JArray.Parse(text);
            return obj;
        }
        // Concatenation of objects and writing in the json file
        public static void Write(JObject obj, string file)
        {
            JArray array = Read(file);
            array.Add(obj);
            File.WriteAllText(file, array.ToString());
        }


    }
}
