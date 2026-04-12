using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Dialogue
{
    [SerializeField] public TMP_Text textDisplay;
    [SerializeField] public string customString = "Your custom text here!";
    [SerializeField] public GameObject npcArt;
    [SerializeField] public float letterDelay = 0.05f;
}

public class Converashons : MonoBehaviour
{
    [SerializeField] private Interaction interaction;

    [Space]

    [SerializeField] private string levelToLoad;
    public bool loadNewScean = false;

    [Space]

    [Header("Dialogue Settings")]
    public List<Dialogue> textTriggers = new List<Dialogue>();
    [SerializeField] private GameObject npcChattingWindow;
    [SerializeField] private Transform npcArtSpawnParent;

    [Space]

    private bool isChatting = false;
    private int currentDialogueIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private GameObject currentNpcArtInstance;

    public SpriteRenderer spriteRendererIntIcon;
    public UnityEngine.Color colorIntIcon;
    public GameObject interactive_prompt;

    [SerializeField] private PlayerMovementScript pm;

    void Awake()
    {
        if (interactive_prompt != null)
        {
            spriteRendererIntIcon = interactive_prompt.GetComponent<SpriteRenderer>();
            if (spriteRendererIntIcon != null)
            {
                colorIntIcon.a = 0f;
                colorIntIcon.r = 1f;
                colorIntIcon.g = 1f;
                colorIntIcon.b = 1f;
                spriteRendererIntIcon.color = colorIntIcon;
            }
        }

        npcChattingWindow.SetActive(false);

        if (interaction == null)
            interaction = GetComponent<Interaction>();

        if (pm == null)
            pm = Object.FindObjectOfType<PlayerMovementScript>();
    }

    void Update()
    {
        // show prompt icon when player is facing an NPC
        if (!isChatting && interaction.EventCaller() == "npc")
            interactiveAni(1);
        else if (!isChatting)
            interactiveAni(2);

        // start dialogue
        if (!isChatting && interaction.EventCaller() == "npc" && Keyboard.current.eKey.wasPressedThisFrame || interaction.EventCaller() == "eventTrigger")
        {
            StartDialogue();
            return;
        }

        // advance or finish dialogue
        if (isChatting && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (isTyping)
                FinishCurrentSentence();
            else
                NextDialogue();
        }
    }

    void StartDialogue()
    {
        npcChattingWindow.SetActive(true);
        pm.not_In_Screen = true;
        isChatting = true;
        currentDialogueIndex = 0;
        DisplayDialogue();
    }

    void interactiveAni(int fashing)
    {
        if (spriteRendererIntIcon == null) return;

        if (colorIntIcon.a != 1f && fashing == 1)
        {
            colorIntIcon.a += 0.01f;
            spriteRendererIntIcon.color = colorIntIcon;
        }

        if (colorIntIcon.a != 0f && fashing == 2)
        {
            colorIntIcon.a -= 0.01f;
            spriteRendererIntIcon.color = colorIntIcon;
        }

        if (colorIntIcon.a < 0) colorIntIcon.a = 0;
        if (colorIntIcon.a > 1) colorIntIcon.a = 1;
    }

    void DisplayDialogue()
    {
        if (currentDialogueIndex < textTriggers.Count)
        {
            Dialogue dialogue = textTriggers[currentDialogueIndex];
            SpawnNpcArt(dialogue.npcArt);

            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypeSentence(dialogue.customString, dialogue.textDisplay, dialogue.letterDelay));
        }
        else
            EndDialogue();
    }

    void SpawnNpcArt(GameObject npcArtPrefab)
    {
        if (currentNpcArtInstance != null)
            Destroy(currentNpcArtInstance);

        if (npcArtPrefab != null && npcArtSpawnParent != null)
        {
            currentNpcArtInstance = Instantiate(npcArtPrefab, npcArtSpawnParent);
            RectTransform rt = currentNpcArtInstance.GetComponent<RectTransform>();

            if (rt != null)
            {
                rt.localPosition = Vector3.zero;
                rt.localScale = Vector3.one;
                rt.anchoredPosition = Vector2.zero;
            }
        }
    }

    IEnumerator TypeSentence(string sentence, TMP_Text textDisplay, float letterDelay)
    {
        isTyping = true;
        textDisplay.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(letterDelay);
        }

        isTyping = false;
    }

    void FinishCurrentSentence()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        Dialogue dialogue = textTriggers[currentDialogueIndex];
        dialogue.textDisplay.text = dialogue.customString;
        isTyping = false;
    }

    void NextDialogue()
    {
        currentDialogueIndex++;

        if (currentDialogueIndex < textTriggers.Count)
            DisplayDialogue();
        else
            EndDialogue();
    }

    void EndDialogue()
    {
        StartCoroutine(CloseWindowAfterDelay());
    }

    IEnumerator CloseWindowAfterDelay()
    {
        yield return new WaitForSeconds(0.34f);

        if (loadNewScean) {
            Debug.Log($"[LevelTransitionTrigger] Loading: {levelToLoad}");
            SceneManager.LoadScene(levelToLoad);
        }

        npcChattingWindow.SetActive(false);
        pm.not_In_Screen = false;
        pm.int_NPC = false;

        if (currentNpcArtInstance != null)
        {
            Destroy(currentNpcArtInstance);
            currentNpcArtInstance = null;
        }

        isChatting = false;
        currentDialogueIndex = 0;
    }
}