using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Text;
using static EasySaveV2.Types;

namespace EasySaveV2.Models
{
    public class Save 
    {
        //public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }

        public string Name { get; set; }

        public string pathSource { get; set; }

        public string pathDestination { get; set; }

        public SaveTypes Type { get; set; }


        /*public int Id
        {
            get
            {
                return id;
            }

            set
            {
                if (value != id)
                {
                    id = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Id"));
                    }
                }
            }
        }
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                if (value != name)
                {
                    name = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                    }
                }
            }
        }

        public string pathSource
        {
            get
            {
                return pathsource;
            }

            set
            {
                if (value != pathsource)
                {
                    pathsource = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("pathSource"));
                    }
                }
            }
        }

        public string pathDestination
        {
            get
            {
                return pathdestination;
            }

            set
            {
                if (value != pathdestination)
                {
                    pathdestination = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("pathDestination"));
                    }
                }
            }
        }

        public SaveTypes Type
        {
            get
            {
                return type;
            }

            set
            {
                if (value != type)
                {
                    type = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Type"));
                    }
                }
            }
        }*/
    }
}
