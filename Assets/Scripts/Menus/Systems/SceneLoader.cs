using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{

    public GameObject loadingCanvas;
    public Transform loadingAnimationTrasform;
    public Image loadingBarImage;
    public Animator animator;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject gameplayUI;

    private static SceneLoader instance = null;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        loadingCanvas.SetActive(false);

        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

    }

    public static SceneLoader Instance
    {
        get
        {
            return instance;
        }
    }

    public void LoadScene(string sceneName = "", int sceneIndex = -1)
    {
        if (sceneIndex == -1)
        {
            sceneIndex = SceneManager.GetSceneByName(sceneName).buildIndex;
        }
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    /// <summary>
    /// Starts Asyncronous Scene Loading.
    /// </summary>
    /// <param name="sceneIndex"></param>
    /// <returns></returns>
    public IEnumerator LoadAsynchronously(int sceneIndex)
    {
        loadingBarImage.fillAmount = 0;

        loadingCanvas.SetActive(true);
        animator.SetTrigger("StartLoading");
        yield return new WaitForSeconds(2.2f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress/0.9f);

            if (operation.progress <= 0.9f && operation.progress >= 0.8f)
            {
                animator.SetBool("FinishLoading", true);
                operation.allowSceneActivation = true;
                loadingBarImage.fillAmount = 1;
                yield return new WaitForSeconds(2.2f);
                break;
            }
            loadingBarImage.fillAmount = progress;
            yield return null;
        }



        loadingBarImage.fillAmount = 1;
        loadingCanvas.SetActive(false);

    }


    /// <summary>
    /// Loads the current level again
    /// </summary>
    public void ResetLevel()
    {
        LoadScene(sceneIndex:SceneManager.GetActiveScene().buildIndex);
    }
  
}
