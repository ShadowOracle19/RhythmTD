using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class OverworldPlayerController : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name Reference")]
    [SerializeField] private string actionMapName = "Overworld";

    [Header("Action Name References")]
    [SerializeField] private string move = "Move";
    [SerializeField] private string options = "Options";
    [SerializeField] private string interact = "Interact";


    private InputAction moveAction;
    private InputAction optionsAction;
    private InputAction interactAction;

    public Vector2 MoveInput { get; private set; }
    public bool OptionsTrigger { get; private set; }
    public bool InteractTrigger { get; private set; }

    [Header("Player Controller Stats")]
    Rigidbody2D body;
    Vector2 axis;
    public float speed;
    public SpriteRenderer sprite;

    public static OverworldPlayerController Instance { get; private set; }

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

        moveAction = playerControls.FindActionMap(actionMapName).FindAction(move);
        optionsAction = playerControls.FindActionMap(actionMapName).FindAction(options);
        interactAction = playerControls.FindActionMap(actionMapName).FindAction(interact);

        RegisterInputActions();
    }

    void RegisterInputActions()
    {
        //moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        moveAction.performed += context => Move(context.ReadValue<Vector2>());
        moveAction.canceled += context => Move(Vector2.zero);

        

        optionsAction.performed += context => {
            if (context.interaction is TapInteraction)
                GameManager.Instance.HandlePauseMenuInput();
        };
        optionsAction.canceled += context => OptionsTrigger = false;

        interactAction.performed += context => {
            if (context.interaction is TapInteraction)
            {
                //Interact functionality
            }
        };
        interactAction.canceled += context => InteractTrigger = false;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        optionsAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        optionsAction.Disable();
    }

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 _axis)
    {
        axis = _axis;
        axis.Normalize();
        sprite.flipX = !(axis.x < 0);
        //transform.Translate(axis);
    }


    private void FixedUpdate()
    {
        body.velocity = axis * speed;
    }


}

