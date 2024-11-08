using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextCrawler : MonoBehaviour
{
    [SerializeField] private float _scrollSpeed = 20f;
    [SerializeField] private GameObject objectBeginning;
    [SerializeField] private GameObject objectStory;
    [SerializeField] private GameObject objectTitle;
    [SerializeField] private float deactivateDistance = 100f;

    private Vector3 lastPosition;

    void Start()
    {
        if (objectBeginning != null && objectTitle != null) {
            lastPosition = transform.position;
            StartCoroutine(SwitchObjectsAfterDelay());
        }
    }

    // Update is called once per frame
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
                    Debug.Log(objectTitle.name + " zosta³ dezaktywowany z powodu odleg³oœci.");
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

    IEnumerator SwitchObjectsAfterDelay()
    {
        yield return new WaitForSeconds(4.5f);

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
        transform.position = lastPosition; // Przywróæ ostatni¹ zapisan¹ pozycjê
        Debug.Log(gameObject.name + " przywrócono pozycjê.");
    }
}
