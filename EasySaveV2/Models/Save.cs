using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Text;
using static EasySaveV2.Types;

namespace EasySaveV2.Models
{
    // Class backup
    public class Save 
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string pathSource { get; set; }

        public string pathDestination { get; set; }

        public SaveTypes Type { get; set; }
    }
}
