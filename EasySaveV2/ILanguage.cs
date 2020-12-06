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
        string saveName { get; }
        string saveChecked { get; }
        string welcome { get; }
    }
}
