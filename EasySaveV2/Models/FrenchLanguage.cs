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
        public string SavesTypes => "Type de sauvegarde";
        public string Add => "Ajouter";
        public string Remove => "Supprimer";
        public string Execute => "Executer";
        public string saveName => "Nom de la sauvegarde";
        public string saveChecked => "Sauvegarde Vérifiée";
        public string welcome => "Bienvenue dans les paramètres";
        public string menuSaves => "Mes sauvegardes";
        public string menuNewSave => "Nouvelle sauvegarde";
        public string menuOptions => "Options";
    }
}