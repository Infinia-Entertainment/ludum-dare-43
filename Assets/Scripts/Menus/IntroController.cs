using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroController : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private SceneLoader sceneLoader;

    void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
        videoPlayer = FindObjectOfType<VideoPlayer>();
        videoPlayer.loopPointReached += SwitchScenes; //sublscibe to the end of the video event and assign scene change to it
    }

    void SwitchScenes(VideoPlayer vp)
    {
        sceneLoader.LoadScene(sceneIndex:1); // scene 1 is main menu.
    } 
}
