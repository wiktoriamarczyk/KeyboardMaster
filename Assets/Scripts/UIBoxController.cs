using UnityEngine;
public class UIBoxController : MonoBehaviour
{
    [SerializeField] ItemActivator firstBox;
    [SerializeField] ItemActivator secondBox;

    void Start()
    {
        firstBox.Activate(true);
        secondBox.Activate(false);
    }

    void Update()
    {
        // after clicking left arrow key, first box will be active and second box will be inactive
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            firstBox.Activate(true);
            secondBox.Activate(false);
        }
        // after clicking right arrow key, first box will be inactive and second box will be active
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            firstBox.Activate(false);
            secondBox.Activate(true);
        }

    }
}
