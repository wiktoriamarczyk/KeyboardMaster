using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Data;

public class TextCrawler : MonoBehaviour
{
    [SerializeField] float _scrollSpeed = 20f;
    [SerializeField] GameObject objectBeginning;
    [SerializeField] GameObject objectStory;
    [SerializeField] GameObject objectTitle;
    [SerializeField] float deactivateDistance = 100f;

    private Vector3 lastPosition;

    void Start()
    {
        if (objectBeginning != null && objectTitle != null)
        {
            lastPosition = transform.position;
            StartCoroutine(SwitchObjectsAfterDelay());
        }
        StartCoroutine(LoadNextScene());
    }

    void Update()
    {
        if (objectBeginning != null && objectTitle != null)
        {
            if (IsObjectActive(objectTitle))
            {
                transform.Translate(Camera.main.transform.forward * _scrollSpeed * Time.deltaTime);

                float distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);

                if (distanceToCamera > deactivateDistance)
                {
                    objectTitle.SetActive(false);
                    Debug.Log(objectTitle.name + " zosta� dezaktywowany z powodu odleg�o�ci.");
                    RestorePosition();
                    objectStory.SetActive(true);
                }
            }
        }
        else
        {
            objectStory.SetActive(true);
        }

        if (IsObjectActive(objectStory))
        {
            transform.Translate(Camera.main.transform.up * _scrollSpeed * Time.deltaTime);
        }
    }

    public bool IsObjectActive(GameObject nameOfObject)
    {
        return nameOfObject.activeSelf;
    }

    public void Skip()
    {
        StopAllCoroutines();
        SceneController.Instance.LoadScene(Data.gameSceneName);
    }

    IEnumerator SwitchObjectsAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        if (objectBeginning != null)
        {
            objectBeginning.SetActive(true);
        }

        yield return new WaitForSeconds(2f);

        if (objectBeginning != null)
        {
            objectBeginning.SetActive(false);
        }

        if (objectTitle != null)
        {
            objectTitle.SetActive(true);
        }
    }

    private void RestorePosition()
    {
        transform.position = lastPosition;
        Debug.Log(gameObject.name + " przywr�cono pozycj�.");
    }

    private IEnumerator LoadNextScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        float delay;
        string nextSceneName;

        if (currentSceneName == Data.introSceneName)
        {
            delay = 33f;
            nextSceneName = Data.gameSceneName;
        }
        else if (currentSceneName == Data.winSceneName)
        {
            delay = 17f;
            nextSceneName = Data.lossSceneName;
        }
        else
        {
            delay = 33f;
            nextSceneName = Data.gameSceneName;
        }

        yield return new WaitForSeconds(delay);
        SceneController.Instance.LoadScene(nextSceneName);
    }

}
