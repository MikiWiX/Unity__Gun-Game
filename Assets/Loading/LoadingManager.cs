using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public GameObject loadingScreen;
    public CanvasGroup displayCanvas;

    public Slider slider;
    public TMPro.TMP_Text textMesh;
    public Button button;

    public static LoadingManager Instance;

    //flags
    private bool LOADED = false;
    private bool CLICKED = false;

    //statics
    private static readonly string loadingScene = "LoadingScreen";
    private static readonly string loadingText = "Loading...";
    private static readonly string loadFinishedText = "Press anywhere to continue";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
            Destroy(gameObject);
    }

    public static void Load()
    {
        if (Instance == null)
        {
            SceneManager.LoadScene(loadingScene, LoadSceneMode.Additive);
        } 
        else
        {
            Instance.loadingScreen.SetActive(true);
            Instance.LoadProcess();
        }
    }

    public static void Load(string sceneName)
    {
        LoadingData.SceneName = sceneName;
        Load();
    }

    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(loadingScreen);
        //DontDestroyOnLoad(displayCanvas);
        LoadProcess();
    }

    private void LoadProcess()
    {
        Debug.Log("Loading Scene: "+LoadingData.SceneName);
        StartCoroutine(StartLoad());
    }

    public void ConfirmLoadingComplete()
    {
        if (LOADED)
        {
            CLICKED = true;
        }
    }

    IEnumerator StartLoad()
    {
        // init screen state
        Instance.displayCanvas.alpha = 0;
        textMesh.text = loadingText;

        // fade in
        loadingScreen.SetActive(true);
        yield return StartCoroutine(FadeLoadingScreen(1, 0.4f));
        LevelManager.gameStop();

        // actual loading
        AsyncOperation operation = SceneManager.LoadSceneAsync(LoadingData.SceneName);
        operation.allowSceneActivation = false;
        while (operation.progress < 0.9f)
        {
            slider.value = operation.progress / 0.9f;
            yield return null;
        }

        slider.value = 1;

        // loading finished
        Debug.Log("Loading Finished");
        LOADED = true;
        textMesh.text = loadFinishedText;
        while (!CLICKED)
        {
            yield return null;
        }
        LOADED = false;
        CLICKED = false;

        operation.allowSceneActivation = true;

        // fade out
        LevelManager.gameStart();
        yield return StartCoroutine(FadeLoadingScreen(0, 1));
        loadingScreen.SetActive(false);
    }

    IEnumerator FadeLoadingScreen(float targetValue, float duration)
    {
        float startValue = displayCanvas.alpha;
        float time = 0;

        while (time < duration)
        {
            displayCanvas.alpha = Mathf.Lerp(startValue, targetValue, time / duration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        displayCanvas.alpha = targetValue;
    }

}
