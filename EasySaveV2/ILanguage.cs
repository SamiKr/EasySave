using System;
using System.Collections.Generic;
using System.Text;

namespace EasySaveV2
{
    public interface ILanguage
    {
        string MySaves { get; }
        string Name { get; }
        string pathSource { get; }
        string pathDestination { get; }
        string Type { get; }
        string Crypter { get; }
        string Launch { get; }
        string SavesTypes { get; }
        string Add { get; }
        string Remove { get; }
        string Execute { get; }
        string saveName { get; }
        string saveChecked { get; }
        string Welcome { get; }
        string menuSaves { get; }
        string menuNewSave { get; }
        string menuOptions { get; }
        string optionsTitle { get; }
        string encrypt { get; }
        string changeLanguage { get; }
    }
}
