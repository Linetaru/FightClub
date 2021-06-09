using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu
{

    // Si ça devient trop chiant tout regrouper dans une classe static et modifier le save manager puisqu'on peut pas serialize des class static
    [CreateAssetMenu(fileName = "SoundSettings_", menuName = "Settings/SoundSettings", order = 1)]
    public class SaveSoundVolume : ScriptableObject, ISavable
    {
        const string MasterVolumeName = "Volume_Master_RTPC";
        const string MusicVolumeName = "Volume_Music_RTPC";
        const string SFXVolumeName = "Volume_SFX_RTPC";
        const string VoiceVolumeName = "Volume_Voice_RTPC";

        [SerializeField]
        private int maxValue = 0;

        [ReadOnly]
        private int masterVolume;
        public int MasterVolume
        {
            get { return masterVolume; }
            set { masterVolume = Mathf.Clamp(value, 0, maxValue); }
        }

        [ReadOnly]
        private int musicVolume;
        public int MusicVolume
        {
            get { return musicVolume; }
            set { musicVolume = Mathf.Clamp(value, 0, maxValue); }
        }

        [ReadOnly]
        private int sfxVolume;
        public int SfxVolume
        {
            get { return sfxVolume; }
            set { sfxVolume = Mathf.Clamp(value, 0, maxValue); }
        }

        [ReadOnly]
        private int voiceVolume;
        public int VoiceVolume
        {
            get { return voiceVolume; }
            set { voiceVolume = Mathf.Clamp(value, 0, maxValue); }
        }


        public string GetSaveID()
        {
            return this.name;
        }

        public List<string> GetAllSavesID()
        {
            List<string> ids = new List<string>();
            ids.Add(MasterVolumeName);
            ids.Add(MusicVolumeName);
            ids.Add(SFXVolumeName);
            ids.Add(VoiceVolumeName);
            return ids;
        }

        public List<SaveGameVariable> Save()
        {
            List<SaveGameVariable> save = new List<SaveGameVariable>(1);
            save.Add(new SaveGameVariable(MasterVolumeName, masterVolume));
            save.Add(new SaveGameVariable(MusicVolumeName, musicVolume));
            save.Add(new SaveGameVariable(SFXVolumeName, sfxVolume));
            save.Add(new SaveGameVariable(VoiceVolumeName, voiceVolume));
            return save;
        }

        public void Load(List<SaveGameVariable> gameVariables)
        {
            for (int i = 0; i < gameVariables.Count; i++)
            {
                if (MasterVolumeName.Equals(gameVariables[i].variableName))
                {
                    masterVolume = gameVariables[i].variableValue;
                    continue;
                }
                else if (MusicVolumeName.Equals(gameVariables[i].variableName))
                {
                    musicVolume = gameVariables[i].variableValue;
                    continue;
                }
                else if (SFXVolumeName.Equals(gameVariables[i].variableName))
                {
                    sfxVolume = gameVariables[i].variableValue;
                    continue;
                }
                else if (VoiceVolumeName.Equals(gameVariables[i].variableName))
                {
                    voiceVolume = gameVariables[i].variableValue;
                    continue;
                }
            }
            ChangeRTPCValue();
        }



        public void ChangeRTPCValue()
        {
            AkSoundEngine.SetRTPCValue(MasterVolumeName, masterVolume / maxValue);
            AkSoundEngine.SetRTPCValue(MusicVolumeName, musicVolume / maxValue);
            AkSoundEngine.SetRTPCValue(SFXVolumeName, sfxVolume / maxValue);
            AkSoundEngine.SetRTPCValue(VoiceVolumeName, voiceVolume / maxValue);
        }
    }
}

