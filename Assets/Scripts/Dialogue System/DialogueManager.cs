using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class DialogueManager : MonoBehaviour
{
    #region JSON Variables

    [System.Serializable]
    public class Dialogue
    {
        public string character;
        public string name;
        public string emotion;
        public string text;
    }

    [System.Serializable]
    public class DialogueList
    {
        public Dialogue[] dialogue;
    }
    #endregion

    #region dont touch this
    private static DialogueManager _instance;
    public static DialogueManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("DialogueManager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    public TextAsset currentDialogue;
    public AudioSource titleMenuMusic;
    public AudioSource dialogueMusic;

    public Transform mainTextBox;
    public Transform secondaryTextBox;

    public TextMeshProUGUI _speakerName;
    public TextMeshProUGUI _dialogue;
    public TextMeshProUGUI _previousSpeakerName;
    public TextMeshProUGUI _previousSpeakerText;

    public Image characterImage;
    public Image secondCharacterImage;
    public int index;

    public DialogueList myDialogue = new DialogueList();

    public GameObject talkingDialogueBox;
    public GameObject descriptiveDialogueBox;
    public GameObject previousTalkingDialogueBox;
    public Color fadedColor;

    public Sprite characterSprite;
    public RuntimeAnimatorController characterSpriteAnimator;
    public Sprite previousCharacter;
    public RuntimeAnimatorController previousCharacterAnimator;
    public string currentCharacterName;
    public string previousCharacterLabel;
    public string previousCharacterName;
    private string previousEmotion;
    public bool previousCharacterTalking;

    public bool dialogueFinished = false;
    public GameObject dialogueSystemParent;

    public float textSpeed = 0.05f;
    public float defaultTextSpeed = 0.05f;

    public Animator cameraAnimator;

    [SerializeField] private Animator textBoxAnimator;
    // HI LUCY. I PUT ALL OF THE ANIMATION TRIGGER CODE HERE SO HOPEFULLY YOU CAN USE IT FOR IMPLEMENTATION. <3
    /*
    textBoxAnimator.SetTrigger("Left Speaker");
    textBoxAnimator.SetTrigger("Right Speaker");
    textBoxAnimator.SetTrigger("Middle Speaker");
    textBoxAnimator.ResetTrigger("Left Speaker");
    textBoxAnimator.ResetTrigger("Right Speaker");
    textBoxAnimator.ResetTrigger("Middle Speaker");
    */
       
    // Contains a reference to the object last selected before opening a dialogue sequence
    public GameObject lastActiveObject;

    private EventSystem eventSystem;

    Coroutine typing;

    [Header("Log")]
    public GameObject logEntry;
    public Transform logParent;
    public GameObject log;
    public bool pauseDialogue = false;

    
    public AudioSource audioSource;

    public int totalVisibleCharacters;
    public int visibleCount;

    private int previousCharacterAudioCue;

    [Header("project overture")]
    [SerializeField] private Animator animator;
    private bool projectOvertureMention = false;
    [SerializeField] private AudioSource overtureAudio;

    [Header("Preloaded Assets")]
    private Dictionary<string, Sprite> loadedSprites = new Dictionary<string, Sprite>();
    private Dictionary<string, RuntimeAnimatorController> loadedControllers = new Dictionary<string, RuntimeAnimatorController>();
    private Dictionary<string, AudioClip> loadedAudioClips = new Dictionary<string, AudioClip>();

    public GameObject dialogueCanvas;
    public DialogueInputHandler dialogueInputHandler;


    // Start is called before the first frame update
    void Start()
    {
        eventSystem = EventSystem.current;
        dialogueInputHandler = GetComponent<DialogueInputHandler>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void Clear()
    {
        _speakerName.text = string.Empty;
        _dialogue.text = string.Empty;
        _previousSpeakerName.text = string.Empty;
        _previousSpeakerText.text = string.Empty;

        previousCharacterName = string.Empty;
        currentCharacterName = string.Empty;
        previousCharacterLabel = string.Empty;
        previousEmotion = string.Empty;

        secondCharacterImage.sprite = null;
        secondCharacterImage.color = Color.clear;
        previousCharacterTalking = false;

        loadedSprites.Clear();
        loadedControllers.Clear();
        loadedAudioClips.Clear();
    }


    public void LoadDialogue(TextAsset desiredDialogue)
    {
        //clear all previous dialogue & assets
        Clear();

        //set log to inactive so it doesnt show when dialogue is loaded
        log.SetActive(false);

        currentDialogue = desiredDialogue;
        myDialogue = JsonUtility.FromJson<DialogueList>(currentDialogue.text);
        index = 0;
        
        //disable player input
        dialogueInputHandler.enabled = false;
        dialogueCanvas.SetActive(false);

        StartCoroutine(PreloadAssets());
    }

    IEnumerator PreloadAssets()
    {
        Debug.Log("Start Dialogue Loading");
        
        foreach (Dialogue dialogue in myDialogue.dialogue)
        {
            Sprite characterSprite = Resources.Load<Sprite>($"Characters/{dialogue.character}/SPR-DS_{dialogue.character}-{dialogue.emotion}");
            RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>($"Characters/{dialogue.character}/{dialogue.character}_{dialogue.emotion}");

            if(characterSprite != null && !loadedSprites.TryGetValue(characterSprite.name, out Sprite value))
            {
                Debug.Log($"Sprite Loaded {characterSprite.name}");
                loadedSprites.Add(characterSprite.name, characterSprite);
            }
            if(controller != null && !loadedControllers.ContainsKey(controller.name))
            {
                loadedControllers.Add(controller.name, controller);
            }
        }
        Debug.Log($"Number of sprites loaded {loadedSprites.Count}");
        Debug.Log($"Number of controllers loaded {loadedControllers.Count}");
        yield return new WaitForSeconds(1);

        titleMenuMusic.Stop();
        dialogueMusic.Play();
        dialogueCanvas.SetActive(true);
        typing = StartCoroutine(TypeLine());
        LoadingScreenManager.Instance.EndLoading();
        
        //enable player input
        dialogueInputHandler.enabled = true;
    }

    IEnumerator TypeLine()
    {
        string dialogueText = myDialogue.dialogue[index].text;

        if(dialogueText.Contains("Project Overture"))
        {
            ProjectOverture();
        }

        currentCharacterName = myDialogue.dialogue[index].name;
        LoadCharacterSprite();

        TMP_TextInfo textInfo;
        TextMeshProUGUI currentTextBox;

        if (previousCharacterTalking)
        {
            secondaryTextBox.SetAsLastSibling();

            _previousSpeakerName.text = myDialogue.dialogue[index].name;
            _previousSpeakerText.text = dialogueText;

            _previousSpeakerText.ForceMeshUpdate();

            textInfo = _previousSpeakerText.textInfo;
            currentTextBox = _previousSpeakerText;

            _dialogue.text = string.Empty;
        }
        else
        {
            mainTextBox.SetAsLastSibling();

            _speakerName.text = myDialogue.dialogue[index].name;

            if(!(previousCharacterLabel == " "))
            {
                _previousSpeakerName.text = previousCharacterName;

            }


            _dialogue.text = dialogueText;

            _dialogue.ForceMeshUpdate();

            textInfo = _dialogue.textInfo;
            currentTextBox = _dialogue;

            _previousSpeakerText.text = string.Empty;
        }

        bool typeText = true;

        totalVisibleCharacters = textInfo.characterCount;
        visibleCount = 0;

        while (typeText)
        {
            while (pauseDialogue)
            {
                yield return new WaitForSeconds(GameManager.Instance.textSpeed);
            }

            if (visibleCount > totalVisibleCharacters)
            {
                yield return new WaitForSeconds(GameManager.Instance.textSpeed);
                //visibleCount = 0;
                typeText = false;
            }

            currentTextBox.maxVisibleCharacters = visibleCount;

            //PlayCharacterAudio();
            visibleCount += 1; 
            yield return new WaitForSeconds(GameManager.Instance.textSpeed);
        }
    }

    public void ProjectOverture() //add new line and animator line up
    {
        projectOvertureMention = true;
    }
    public void PlayOverture()
    {
        overtureAudio.Play();
    }

    public void NextLine()
    {
        GameObject _newLog = Instantiate(logEntry, logParent);
        _newLog.GetComponent<DialogueLogEntry>().characterName.text = _speakerName.text;
        _newLog.GetComponent<DialogueLogEntry>().dialogue.text = _dialogue.text;

        if(projectOvertureMention) //load project overture animation
        {
            Clear();
            descriptiveDialogueBox.SetActive(false);
            talkingDialogueBox.SetActive(false);
            previousTalkingDialogueBox.SetActive(false);

            _previousSpeakerName.text = string.Empty;
            _speakerName.text = string.Empty;
            previousCharacterName = string.Empty;
            previousCharacterLabel = string.Empty;
            previousCharacterTalking = false;

            characterImage.sprite = null;
            characterImage.color = Color.clear;

            secondCharacterImage.sprite = null;
            secondCharacterImage.color = Color.clear;

            animator.SetTrigger("Trigger Overture");
            projectOvertureMention = false;
            
            return;
        }

        if (index < myDialogue.dialogue.Length - 1)//start next line
        {
            index++;
            _dialogue.text = string.Empty;
            typing = StartCoroutine(TypeLine());
        }
        else //dialogue finished
        {
            EndDialogue();

            return;
        }
    }
    public void SkipDialogue()
    {
        //end dialogue
        StopCoroutine(typing);

        for (int i = index; i < myDialogue.dialogue.Length; i++)
        {
            GameObject _newLog = Instantiate(logEntry, logParent);
            _newLog.GetComponent<DialogueLogEntry>().characterName.text = myDialogue.dialogue[i].name;
            _newLog.GetComponent<DialogueLogEntry>().dialogue.text = myDialogue.dialogue[i].text;
        }

        EndDialogue();
    }

    public Sprite RetrieveSprite(string key)
    {
        Sprite sprite = null;
        //Debug.Log($"Try to load {key} sprite");
        if (loadedSprites.TryGetValue(key, out sprite))
        {
            return sprite = loadedSprites[key];
        }
        else
        {
            //Debug.Log("Could not retrieve sprite");
            return null;
        }
    }

    public RuntimeAnimatorController RetrieveController(string key)
    {
        RuntimeAnimatorController controller = null;
        //Debug.Log($"Try to load {key} controller");
        if (loadedControllers.TryGetValue(key, out controller))
        {
            return controller = loadedControllers[key];
        }
        else
        {
            //Debug.Log("Could not retrieve controller");
            return null;
        }

    }

    public string ToPascalCase(string original)
    {
        Regex invalidCharsRgx = new Regex("[^_a-zA-Z0-9]");
        Regex whiteSpace = new Regex(@"(?<=\s)");
        Regex startsWithLowerCaseChar = new Regex("^[a-z]");
        Regex firstCharFollowedByUpperCasesOnly = new Regex("(?<=[A-Z])[A-Z0-9]+$");
        Regex lowerCaseNextToNumber = new Regex("(?<=[0-9])[a-z]");
        Regex upperCaseInside = new Regex("(?<=[A-Z])[A-Z]+?((?=[A-Z][a-z])|(?=[0-9]))");

        // replace white spaces with undescore, then replace all invalid chars with empty string
        var pascalCase = invalidCharsRgx.Replace(whiteSpace.Replace(original, "_"), string.Empty)
            // split by underscores
            .Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
            // set first letter to uppercase
            .Select(w => startsWithLowerCaseChar.Replace(w, m => m.Value.ToUpper()))
            // replace second and all following upper case letters to lower if there is no next lower (ABC -> Abc)
            .Select(w => firstCharFollowedByUpperCasesOnly.Replace(w, m => m.Value.ToLower()))
            // set upper case the first lower case following a number (Ab9cd -> Ab9Cd)
            .Select(w => lowerCaseNextToNumber.Replace(w, m => m.Value.ToUpper()))
            // lower second and next upper case letters except the last if it follows by any lower (ABcDEf -> AbcDef)
            .Select(w => upperCaseInside.Replace(w, m => m.Value.ToLower()));

        return string.Concat(pascalCase);
    }


    public void LoadCharacterSprite()
    {
        // Loads the character sprite from the JSON using their name and emotion tags
        //characterSprite = Resources.Load<Sprite>($"Characters/{myDialogue.dialogue[index].character}/SPR-DS_{myDialogue.dialogue[index].character}-{myDialogue.dialogue[index].emotion}");
        //characterSpriteAnimator = Resources.Load<RuntimeAnimatorController>($"Characters/{myDialogue.dialogue[index].character}/{myDialogue.dialogue[index].character}_{myDialogue.dialogue[index].emotion}");

        characterSprite = RetrieveSprite($"SPR-DS_{ToPascalCase(myDialogue.dialogue[index].character)}-{myDialogue.dialogue[index].emotion}");
        characterSpriteAnimator = RetrieveController($"{ToPascalCase(myDialogue.dialogue[index].character)}_{myDialogue.dialogue[index].emotion}");

        //if no character sprite is loaded
        if (characterSpriteAnimator == null && characterSprite == null)
        {
            descriptiveDialogueBox.SetActive(true);
            talkingDialogueBox.SetActive(false);
            previousTalkingDialogueBox.SetActive(false);

            _previousSpeakerName.text = string.Empty;
            _speakerName.text = string.Empty;
            previousCharacterName = string.Empty;
            previousCharacterLabel = string.Empty;
            previousCharacterTalking = false;

            characterImage.GetComponent<Animator>().runtimeAnimatorController = null;
            characterImage.sprite = null;
            characterImage.color = Color.clear;

            secondCharacterImage.GetComponent<Animator>().runtimeAnimatorController = null;
            secondCharacterImage.sprite = null;
            secondCharacterImage.color = Color.clear;
        }
        
        //if a character sprite is loaded
        else
        {
            descriptiveDialogueBox.SetActive(false);
            talkingDialogueBox.SetActive(true);

            if(previousCharacterLabel == myDialogue.dialogue[index].character) //if the previous character is talking
            {
                previousCharacterTalking = true;
                characterImage.color = fadedColor;
                secondCharacterImage.color = Color.white;

                previousTalkingDialogueBox.SetActive(true);
                talkingDialogueBox.SetActive(false);
                _previousSpeakerName.text = previousCharacterName;

                //previousCharacter = Resources.Load<Sprite>($"Characters/{previousCharacterLabel}/SPR-DS_{previousCharacterLabel}-{previousEmotion}");
                //previousCharacterAnimator = Resources.Load<RuntimeAnimatorController>($"Characters/{previousCharacterLabel}/{previousCharacterLabel}_{previousEmotion}");

                //loadedSprites.TryGetValue($"SPR-DS_{previousCharacterLabel}-{previousEmotion}", out previousCharacter);
                //loadedControllers.TryGetValue($"{previousCharacterLabel}_{previousEmotion}", out previousCharacterAnimator);

                previousCharacter = RetrieveSprite($"SPR-DS_{ToPascalCase(previousCharacterLabel)}-{previousEmotion}");
                previousCharacterAnimator = RetrieveController($"{ToPascalCase(previousCharacterLabel)}_{previousEmotion}");


                //load either animator or sprite
                if (characterSpriteAnimator == null)
                {
                    secondCharacterImage.GetComponent<Animator>().runtimeAnimatorController = null;
                    secondCharacterImage.sprite = characterSprite;

                    secondCharacterImage.transform.localScale = new Vector3(1f, secondCharacterImage.transform.localScale.y, secondCharacterImage.transform.localScale.z);

                }
                else
                {
                    //StartCoroutine(SwitchAnimController(characterSpriteAnimator, secondCharacterImage.GetComponent<Animator>()));
                    secondCharacterImage.GetComponent<Animator>().runtimeAnimatorController = characterSpriteAnimator;

                    secondCharacterImage.transform.localScale = new Vector3(-1f, secondCharacterImage.transform.localScale.y, secondCharacterImage.transform.localScale.z);
                }

                return;
            }

            if(index != 0 && myDialogue.dialogue[index].character != myDialogue.dialogue[index - 1].character && myDialogue.dialogue[index].character != string.Empty) //if the previous character is not talking and a new character is
            {
                previousCharacterTalking = false;

                //load either animator or sprite
                if (previousCharacterAnimator == null)
                {
                    secondCharacterImage.GetComponent<Animator>().runtimeAnimatorController = null;
                    secondCharacterImage.sprite = previousCharacter;

                    secondCharacterImage.transform.localScale = new Vector3(1f, secondCharacterImage.transform.localScale.y, secondCharacterImage.transform.localScale.z);

                }
                else
                {

                    //StartCoroutine(SwitchAnimController(previousCharacterAnimator, secondCharacterImage.GetComponent<Animator>()));
                    secondCharacterImage.GetComponent<Animator>().runtimeAnimatorController = previousCharacterAnimator;

                    secondCharacterImage.transform.localScale = new Vector3(-1f, secondCharacterImage.transform.localScale.y, secondCharacterImage.transform.localScale.z);
                }


                secondCharacterImage.color = fadedColor;
                previousCharacterName = myDialogue.dialogue[index - 1].name;
                previousCharacterLabel = myDialogue.dialogue[index - 1].character;
                previousEmotion = myDialogue.dialogue[index - 1].emotion;

            }
            previousCharacterTalking = false;
            previousTalkingDialogueBox.SetActive(false);
            characterImage.color = Color.white;

            //load either animator or sprite
            if (characterSpriteAnimator == null)
            {
                characterImage.GetComponent<Animator>().runtimeAnimatorController = null;
                characterImage.sprite = characterSprite;

                characterImage.transform.localScale = new Vector3(-1f, secondCharacterImage.transform.localScale.y, characterImage.transform.localScale.z);

            }
            else
            {
                //StartCoroutine(SwitchAnimController(characterSpriteAnimator, characterImage.GetComponent<Animator>()));
                characterImage.GetComponent<Animator>().runtimeAnimatorController = characterSpriteAnimator;

                characterImage.transform.localScale = new Vector3(1f, characterImage.transform.localScale.y, characterImage.transform.localScale.z);

            }

        }
        previousCharacter = characterSprite;
        previousCharacterAnimator = characterSpriteAnimator;
    }

    private void PlayCharacterAudio()
    {
        if (audioSource.isPlaying)
            return;
        int randNum = UnityEngine.Random.Range(1, 6); //gets random number between 1 and 5

        while (previousCharacterAudioCue == randNum)
        {
            randNum = UnityEngine.Random.Range(1, 6);
        }
        //if(previousCharacterAudioCue == randNum)//respins random character audio cue
        //{
        //    randNum = Random.Range(1, 6);
        //}

        previousCharacterAudioCue = randNum;

        var _characterSpeaking = Resources.Load<AudioClip>($"audio/{myDialogue.dialogue[index].character}/{myDialogue.dialogue[index].character}{randNum}");

        if (_characterSpeaking == null)
            return;
        else
        {
            GetComponent<AudioSource>().clip = _characterSpeaking;

            //Below is theoretical code the increase the sound speed of the dialogue depending on the text speed. But it doesn't sound great / work right now.
            /*if (GameManager.Instance.textSpeed == 0.01f) //Slow
            {
                audioSource.pitch = Random.Range(0.3f, 0.5f);
            }
            else if (GameManager.Instance.textSpeed == 0.05f) //Med
            {
                audioSource.pitch = Random.Range(0.9f, 1.1f);
            }
            else if (GameManager.Instance.textSpeed == 0.001f) //Fast
            {
                audioSource.pitch = Random.Range(1.5f, 1.7f);
            }
            else //Broken :(
            {
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                Debug.LogError("[DialogueManager] Dialogue Audio Speed is Broken :(");
            } */

            audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f); //Get rid of this line if you wanna use the code above.
            audioSource.Play();
        }
    }


    public void FinishLine()
    {
        if(visibleCount >= totalVisibleCharacters)
        {
            NextLine();
        }
        else
        {
            StopCoroutine(typing);
            visibleCount = totalVisibleCharacters;
            _dialogue.maxVisibleCharacters = visibleCount;
            _previousSpeakerText.maxVisibleCharacters = visibleCount;
        }
    }

    public void EndDialogue()
    {
        //end dialogue
        StopCoroutine(typing);

        dialogueMusic.Stop();

        //dialogue if its going into a combat
        if (GameManager.Instance.encounterRunning)
        {
            /*
            if (GameManager.Instance.tutorialRunning)
            {
                //GameManager.Instance.dialogueRoot.SetActive(false);
                GameManager.Instance.LoadTutorial();
                return;
            }
            */
            
            MenuEventManager.Instance.DialogueClose();
            MenuEventManager.Instance.OpenLoadoutMenu();

            return;
        }
        //dialogue after combat
        if (GameManager.Instance.winState)
        {
            /*
            if(GameManager.Instance.currentEncounter.isShowcase)
            {
                GameManager.Instance.showcaseCredits.SetActive(true);
                //MenuEventManager.Instance.OpenShowcaseCredits();
                GameManager.Instance.dialogueRoot.SetActive(false);
                GameManager.Instance.combatRoot.SetActive(false);
                return;
            }
            */

            GameManager.Instance.StartWinLevelProcess();

            GameManager.Instance.dialogueRoot.SetActive(false);
        }
        // Sets the active object to the object last active before dialogue started
        else if (GameManager.Instance.menuRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(lastActiveObject);
        }

        dialogueSystemParent.SetActive(false);
    }

    public void SetLastActiveObject(GameObject currentlyActiveObject)
    {
        lastActiveObject = currentlyActiveObject;
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void OnDisable()
    {
        dialogueMusic.Stop();
    }

    string GetCompleteRichTextTag(ref int _index)
    {
        string completeTag = string.Empty;

        while(_index < myDialogue.dialogue[index].text.Length)
        {
            completeTag += myDialogue.dialogue[index].text[_index];
            if (myDialogue.dialogue[index].text[_index] == '>')
                return completeTag;

            _index++;
        }
        return string.Empty;
    }

    public void OpenLog()
    {
        log.SetActive(true);
        pauseDialogue = true;
        MenuEventManager.Instance.OpenLog();
        DialogueInputHandler.Instance.enabled = false;
    }

    public void CloseLog()
    {
        log.SetActive(false);
        pauseDialogue = false;
        DialogueInputHandler.Instance.enabled = true;
    }

}