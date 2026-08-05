using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class UIInputHandler : MonoBehaviour
{
    [Header("<b><size=15>Input Action Asset<b><size=15>")]
    [Line(255,255,255)]
    [SerializeField] private InputActionAsset uiControls;

    [Space(20)][Header("<b><size=15>Action Map Name Reference<b><size=15>")]
    [Line(255,255,255)]
    [SerializeField] private string actionMapName = "UI";

    [Space(20)][Header("<b><size=15>Action Name References<b><size=15>")]
    [Line(255,255,255)]
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
