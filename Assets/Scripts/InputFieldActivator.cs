using TMPro;
using UnityEngine;

public class InputFieldActivator : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] ItemActivator itemActivator;

    void Start()
    {
        itemActivator.onActivated += OnActivated;
    }

    void OnActivated(bool activated)
    {
        inputField.text = string.Empty;
        inputField.interactable = activated;
        if (activated)
            inputField.ActivateInputField();
        else
        inputField.DeactivateInputField();
    }

}
