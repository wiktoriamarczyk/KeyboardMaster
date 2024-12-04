using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;

    public static SceneController Instance;
    public bool win { get; private set; }
    public bool pause { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause(true);
        }
    }

    public void Pause(bool pause)
    {
        this.pause = pause;
        Time.timeScale = pause ? 0 : 1;

        if (pausePanel == null)
        {
            pausePanel = Data.FindInactiveObjectWithTag(Data.pausePanelTag);
        }

        pausePanel.SetActive(pause);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetGameResult(bool value)
    {
        win = value;
    }
}
