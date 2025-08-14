using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatDialogueManager : MonoBehaviour
{
    #region dont touch this
    private static CombatDialogueManager _instance;
    public static CombatDialogueManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("CombatDialogueManager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    //having this here to check if dialogue is playing
    public bool combatDialogueActive = false;

    //dialogue gameobjects
    [SerializeField] public GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueTextMesh;
    [SerializeField] private Image dialogueImage;

    //dialogue variables
    private int dialogueIndex;
    private CombatDialogue[] dialogue;
    Coroutine _typing;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayDialogue(CombatDialogue[] dialogueToPlay)
    {
        Clear();

        dialogueBox.SetActive(true);

        //reset dialogue variables
        combatDialogueActive = true;
        dialogueIndex = 0;
        dialogue = dialogueToPlay;
        _typing = StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        string dialogueText = dialogue[dialogueIndex].text;
        dialogueImage.sprite = dialogue[dialogueIndex].characterSprite;

        TMP_TextInfo textInfo = new TMP_TextInfo();

        dialogueTextMesh.text = dialogueText;
        dialogueTextMesh.ForceMeshUpdate();
        textInfo = dialogueTextMesh.textInfo;

        bool typeText = true;
        int totalVisibleCharacters = textInfo.characterCount;
        int visibleCount = 0;

        while(typeText)
        {
            //all text has been typed
            if(visibleCount > totalVisibleCharacters)
            {
                yield return new WaitForSeconds(2);
                typeText = false;
                NextLine();
                yield return null;

            }

            //type text
            dialogueTextMesh.maxVisibleCharacters = visibleCount;

            visibleCount += 1;
            yield return new WaitForSeconds(GameManager.Instance.textSpeed);

        }


    }

    public void NextLine()
    {
        StopCoroutine(_typing);
        if(dialogueIndex < dialogue.Length - 1)
        {
            dialogueIndex += 1;
            dialogueTextMesh.text = string.Empty;
            _typing = StartCoroutine(TypeLine());
        }
        else//dialogue finished
        {
            combatDialogueActive = false;
            Clear();
            dialogueBox.SetActive(false);
        }
    }

    public void Clear()
    {
        dialogue = null;
        dialogueIndex = 0;
        dialogueTextMesh.text = string.Empty;
    }
}

[System.Serializable]
public class CombatDialogue
{
    public string text;
    public Sprite characterSprite;
}
