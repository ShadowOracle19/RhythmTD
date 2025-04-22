using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class DialogueInputHandler : MonoBehaviour
{
    public DialogueManager dialogueManager;

    [Header("Input action asset")]
    [SerializeField] private InputActionAsset dialogueControls;

    [Header("Action map name ref")]
    [SerializeField] private string dialogueMapName = "Dialogue";

    [Header("Action name ref")]
    [SerializeField] private string skip = "Skip";
    [SerializeField] private string log = "Log";
    [SerializeField] private string advanceDialog = "AdvanceDialog";

    private InputAction skipAction;
    private InputAction logAction;
    private InputAction advanceAction;

    public bool SkipTrigger { get; private set; }
    public bool LogTrigger { get; private set; }
    public bool AdvanceTrigger { get; private set; }

    public static DialogueInputHandler Instance {  get; private set; }

    private void Awake()
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

        skipAction = dialogueControls.FindActionMap(dialogueMapName).FindAction(skip);
        logAction = dialogueControls.FindActionMap(dialogueMapName).FindAction(log);
        advanceAction = dialogueControls.FindActionMap(dialogueMapName).FindAction(advanceDialog);
        
        RegisterDialogueActions();
    }

    void RegisterDialogueActions()
    {
        skipAction.performed += context =>
        {
            if (context.interaction is TapInteraction)
                dialogueManager.SkipDialogue();
        };
        skipAction.canceled += context => SkipTrigger = false;

        logAction.performed += context =>
        {
            if (context.interaction is TapInteraction)
                dialogueManager.OpenLog();
        };
        logAction.canceled += context => LogTrigger = false;

        advanceAction.performed += context =>
        {
            if (context.interaction is TapInteraction)
                dialogueManager.FinishLine();
        };
        advanceAction.canceled += context => SkipTrigger = false;

    }

    private void OnEnable()
    {
        skipAction.Enable();
        logAction.Enable();
        advanceAction.Enable();
    }

    private void OnDisable()
    {
        skipAction.Disable();
        logAction.Disable();
        advanceAction.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
