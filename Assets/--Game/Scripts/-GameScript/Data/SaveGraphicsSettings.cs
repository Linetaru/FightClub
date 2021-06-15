using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu
{
    // Si ça devient trop chiant tout regrouper dans une classe static et modifier le save manager puisqu'on peut pas serialize des class static
    [CreateAssetMenu(fileName = "GraphicsSettings_", menuName = "Settings/GraphicsSettings", order = 1)]
    public class SaveGraphicsSettings : ScriptableObject, ISavable
    {

        const string QualityLevelName = "QualityLevel";
        const string ResolutionName = "Resolution";
        const string FullscreenName = "FullScreen";



        [ReadOnly]
        private int qualityLevel;
        public int QualityLevel
        {
            get { return qualityLevel; }
            set { qualityLevel = Mathf.Clamp(value, 0, QualitySettings.names.Length-1); }
        }

        [ReadOnly]
        private int resolution;
        public int Resolution
        {
            get { return resolution; }
            set { resolution = Mathf.Clamp(value, 0, Screen.resolutions.Length-1); }
        }

        [ReadOnly]
        private int fullscreen;
        public int Fullscreen
        {
            get { return fullscreen; }
            set { fullscreen = (value == 0) ? 0 : 1; }
        }



        public string GetSaveID()
        {
            return this.name;
        }

        public List<string> GetAllSavesID()
        {
            List<string> ids = new List<string>();
            ids.Add(QualityLevelName);
            ids.Add(ResolutionName);
            ids.Add(FullscreenName);
            return ids;
        }

        public List<SaveGameVariable> Save()
        {
            List<SaveGameVariable> save = new List<SaveGameVariable>(1);
            save.Add(new SaveGameVariable(QualityLevelName, qualityLevel));
            save.Add(new SaveGameVariable(ResolutionName, resolution));
            save.Add(new SaveGameVariable(FullscreenName, fullscreen));
            return save;
        }

        public void Load(List<SaveGameVariable> gameVariables)
        {
            for (int i = 0; i < gameVariables.Count; i++)
            {
                if (QualityLevelName.Equals(gameVariables[i].variableName))
                {
                    QualityLevel = gameVariables[i].variableValue;
                    continue;
                }
                else if (ResolutionName.Equals(gameVariables[i].variableName))
                {
                    Resolution = gameVariables[i].variableValue;
                    continue;
                }
                else if (FullscreenName.Equals(gameVariables[i].variableName))
                {
                    Fullscreen = gameVariables[i].variableValue;
                    continue;
                }
            }
            ChangeQualitySettings();
        }



        public void ChangeQualitySettings()
        {
            QualitySettings.SetQualityLevel(qualityLevel);
            Screen.fullScreen = (fullscreen == 0) ? true : false;
            Screen.SetResolution(Screen.resolutions[resolution].width, Screen.resolutions[resolution].height, Screen.fullScreen);
        }
    }

}