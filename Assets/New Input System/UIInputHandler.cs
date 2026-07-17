using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class UIInputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset uiControls;

    [Header("Action Map Name Reference")]
    [SerializeField] private string actionMapName = "UI";

    [Header("Action Name References")]
    [SerializeField] private string modifierMenu = "Modifier Menu Toggle";

    private InputAction modifierAction;

    public bool ModifierTrigger { get; private set; }

    public static UIInputHandler Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        modifierAction = uiControls.FindActionMap(actionMapName).FindAction(modifierMenu);
        
        RegisterInputActions();
    }

    void RegisterInputActions()
    {
        modifierAction.performed += context => {
            if (context.interaction is TapInteraction)
                MenuEventManager.Instance.HandleModifierMenuInput();
        };
        modifierAction.canceled += context => ModifierTrigger = false;
    }

    private void OnEnable()
    {
        modifierAction.Enable();
    }

    private void OnDisable()
    {
        modifierAction.Disable();
        Debug.Log("Player Input Handler Disabled");
    }

}
