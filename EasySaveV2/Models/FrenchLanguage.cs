using System;

namespace EasySaveV2.Models
{
    public class FrenchLanguage : ILanguage
    {
        public string MySaves => "Mes sauvegardes";
        public string Name => "Nom";
        public string pathSource => "Chemin source";
        public string pathDestination => "Chemin destination";
        public string Type => "Type";
        public string Crypter => "Crypter";
        public string Launch => "Lance les sauvegardes";
        public string SavesTypes => "Type des suavegardes";
        public string Add => "Ajouter";
        public string saveName => "Nom de la sauvegarde";
        public string saveChecked => "Sauvegarde Vérifiée";
        public string welcome => "Bienvenue dans les paramètres";
    }
}