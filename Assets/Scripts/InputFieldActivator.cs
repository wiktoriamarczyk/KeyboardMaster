using TMPro;
using UnityEngine;

public class InputFieldActivator : MonoBehaviour
{
    [SerializeField] TMP_InputField                 inputField;
    [SerializeField] UserInterfaceItemsActivator    itemActivator;
    [SerializeField] bool                           clearOnStateChange = false;

    void Start()
    {
        itemActivator.onActivationStateChange += OnActivationStateChange;
    }

    void OnActivationStateChange(bool activated)
    {
        if (clearOnStateChange)
        {
            inputField.text = string.Empty;
        }

        inputField.interactable = activated;

        if (activated)
        {
            inputField.ActivateInputField();
        }
        else
        {
            inputField.DeactivateInputField();
        }

    }
}
