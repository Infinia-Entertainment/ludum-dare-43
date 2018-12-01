using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using System.Linq;

//SystemConfigurationMenu manager, helds transitions and all settings
public class SystemMenu : MonoBehaviour
{
    //Declaration of class-wide variables
    #region VARIABLES
    [SerializeField]
    TMPro.TMP_Dropdown resolutionDropdown;
    [SerializeField]
    TMPro.TMP_Dropdown qualityDropdown;
    [SerializeField]
    AudioMixer audioMixer;
    [SerializeField]
    Toggle fullScreenToggle;

    Resolution[] resolutions;
    #endregion

    //Public methods for OnValueChanged events (Audio settings)
    #region AUDIO
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }
    public void SetSoundVolume(float volume)
    {
        audioMixer.SetFloat("SoundVolume", volume);
    }
    #endregion

    //Public methods for OnValueChanged events (Video settings)
    #region VIDEO
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)  
    {
        Resolution resolution = resolutions[resolutionIndex]; //Assign the chosen resolution to the variable
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen); //Apply the resolution
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        Debug.Log(QualitySettings.names[QualitySettings.GetQualityLevel()]);
    }
    #endregion

    //Additional methods required
    #region ADDITIONAL
    void Start()
    {
        #region Resolutions
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();


        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
        }

        resolutionDropdown.AddOptions(options);             //Add options to the dropdown menu
        resolutionDropdown.value = Screen.resolutions.ToList().IndexOf(Screen.currentResolution);  //Set the value to the current one you have
        resolutionDropdown.RefreshShownValue();             //Refresh the displayed value

        fullScreenToggle.isOn = Screen.fullScreen;
        #endregion

        #region Quality_Control

        string[] qualities = QualitySettings.names;

        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(qualities.ToList());
        qualityDropdown.value = QualitySettings.GetQualityLevel();  //Set the value to the current one you have
        qualityDropdown.RefreshShownValue();                        //Refresh the displayed value
        #endregion
    }

    #endregion
}



