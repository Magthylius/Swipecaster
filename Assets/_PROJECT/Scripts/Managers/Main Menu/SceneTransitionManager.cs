using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LerpFunctions;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager instance;

    public float transitionSpeed = 10f;
    public float sceneDelay = 2f;
    public List<GameObject> loadingObjects;

    CanvasGroupFader canvasCG;
    string targetScene;

    void OnEnable()
    {
        
    }

    void Awake()
    {
        if (instance != null)
            Destroy(this);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        canvasCG = new CanvasGroupFader(gameObject.GetComponent<CanvasGroup>(), true, true);
        canvasCG.fadeEndedEvent.AddListener(ShowLoaders);

        SceneManager.sceneLoaded += OnSceneLoad;
        DeactivateTransition();
    }

    void Update()
    {
        canvasCG.Step(transitionSpeed * Time.unscaledDeltaTime);
        //if (Time.unscaledDeltaTime >= 0.01f) print(Time.unscaledDeltaTime); 
    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(SceneLoadDelay(sceneDelay));
    }

    public void ActivateTransition(string transitionTarget)
    {
        canvasCG.StartFadeIn();
        targetScene = transitionTarget;
    }

    void DeactivateTransition()
    {
        SetObjectActives(false);
        canvasCG.StartFadeOut();
    }

    void SetObjectActives(bool activation)
    {
        foreach (GameObject obj in loadingObjects) obj.SetActive(activation);
    }

    void ShowLoaders()
    {
        if (canvasCG.State == CanvasGroupFader.CanvasState.FADE_IN)
        {
            SetObjectActives(true);
            StartCoroutine(LoadAsyncScene());
        }
    }

    IEnumerator SceneLoadDelay(float time)
    {  
        yield return new WaitForSecondsRealtime(time);
        print("activation2");
        DeactivateTransition();
    }
    
    IEnumerator LoadAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene);

        while(!asyncLoad.isDone)
        {
            yield return null;
        }

        //print("activation1");
        //DeactivateTransition();
    }
}
