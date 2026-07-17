using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name Reference")]
    [SerializeField] private string actionMapName = "Player";

    [Header("Action Name References")]
    [SerializeField] private string move = "Move";
    [SerializeField] private string radialMenu = "Radial Menu Toggle";
    [SerializeField] private string swapTower = "Swap Tower";
    [SerializeField] private string destructMode = "Destruct Mode Toggle";
    [SerializeField] private string destructTower = "Destruct Tower";
    [SerializeField] private string buff = "Buff";
    /*
    [SerializeField] private string buff1 = "Buff 1";
    [SerializeField] private string buff2 = "Buff 2";
    [SerializeField] private string buff3 = "Buff 3";
    [SerializeField] private string buff4 = "Buff 4";
    */
    [SerializeField] private string feverMode = "Fever Mode";
    [SerializeField] private string options = "Options";


    private InputAction moveAction;
    private InputAction radialAction;
    private InputAction swapAction;
    private InputAction destructToggleAction;
    private InputAction destructAction;
    private InputAction buffAction;
    private InputAction feverAction;
    private InputAction optionsAction;

    public Vector2 MoveInput { get; private set; }
    public bool RadialTrigger { get; private set; }
    public bool SwapTrigger { get; private set; }
    public bool DestructToggleTrigger { get; private set; }
    public bool DestructTrigger { get; private set; }
    public bool BuffTrigger { get; private set; }
    public bool FeverTrigger { get; private set; }
    public bool OptionsTrigger { get; private set; }

    public static PlayerInputHandler Instance { get; private set; }

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

        moveAction = playerControls.FindActionMap(actionMapName).FindAction(move);
        radialAction = playerControls.FindActionMap(actionMapName).FindAction(radialMenu);
        swapAction = playerControls.FindActionMap(actionMapName).FindAction(swapTower);
        destructToggleAction = playerControls.FindActionMap(actionMapName).FindAction(destructMode);
        destructAction = playerControls.FindActionMap(actionMapName).FindAction(destructTower);
        buffAction = playerControls.FindActionMap(actionMapName).FindAction(buff);
        feverAction = playerControls.FindActionMap(actionMapName).FindAction(feverMode);
        optionsAction = playerControls.FindActionMap(actionMapName).FindAction(options);
        
        RegisterInputActions();
    }

    void RegisterInputActions()
    {
        //moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        //moveAction.performed += context => CursorTD.Instance.MoveCursor(context.ReadValue<Vector2>());
        moveAction.performed += context =>
        {
            if(context.interaction is PressInteraction)
                CursorTD.Instance.MoveCursor(context.ReadValue<Vector2>());
        };
        moveAction.canceled += context => MoveInput = Vector2.zero;

        radialAction.performed += context =>
        {
            if (context.interaction is HoldInteraction)
                CursorTD.Instance.TogglePlacementMenu();
        };
        radialAction.canceled += context => {
            if (context.interaction is HoldInteraction)
                CursorTD.Instance.ClosePlacementMenu();
        };

        swapAction.performed += context => {
            if (context.interaction is TapInteraction)
                CursorTD.Instance.SwapTowers();
        }; 
        swapAction.canceled += context => SwapTrigger = false;

        destructToggleAction.performed += context => {
            if (context.interaction is TapInteraction)
                CursorTD.Instance.DestroyMode();
        };
        destructToggleAction.canceled += context => DestructToggleTrigger = false;

        destructAction.performed += context => {
            if (context.interaction is HoldInteraction)
                CursorTD.Instance.DestroyTower();
        };
        destructAction.canceled += context => DestructTrigger = false;

        buffAction.performed += context => {
            if (context.interaction is TapInteraction)
                CursorTD.Instance.BuffTrigger();
        };
        buffAction.canceled += context => BuffTrigger = false;

        feverAction.performed += context => {
            if (context.interaction is TapInteraction)
                CursorTD.Instance.HandleFeverModeInput();
        };
        feverAction.canceled += context => FeverTrigger = false;

        optionsAction.performed += context => {
            if (context.interaction is TapInteraction)
                GameManager.Instance.HandlePauseMenuInput();
        };
        optionsAction.canceled += context => OptionsTrigger = false;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        radialAction.Enable();
        swapAction.Enable();
        destructToggleAction.Enable();
        destructAction.Enable();
        buffAction.Enable();
        feverAction.Enable();
        optionsAction.Enable(); 
    }

    private void OnDisable()
    {
        moveAction.Disable();
        radialAction.Disable();
        swapAction.Disable();
        destructToggleAction.Disable();
        destructAction.Disable();
        buffAction.Disable();
        feverAction.Disable();
        //optionsAction.Disable();
        Debug.Log("Player Input Handler Disabled");
    }

}

