using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class dialogue : MonoBehaviour
{
    // This script handles dialogue interactions

    // Create variables for dialogue management
    [Header("UI References")]
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private TMP_Text speakerText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Image portraitImage;

    [Header("Dialogue Content")]
    [SerializeField] private string[] speaker;
    [SerializeField][TextArea] private string[] dialogueWords;
    [SerializeField] private Sprite[] portrait;

    private bool dialogueActivated;
    private int step;
    private bool isTyping;
    private Coroutine typingCoroutine; // Coroutine for typewriter effect
    private bool justEntered = false; // Prevents dialogue from activating twice

    [Header("Typing Settings")]
    [SerializeField] private float typeSpeed = 0.03f;

    void Update()
    {
        // Check if the dialogue is activated and the player presses the 'T' key
        if (dialogueActivated && Input.GetKeyDown(KeyCode.T) && !justEntered)
        {

            // If currently typing, complete the typing effect, or else show next line
            if (isTyping)
            {
                CompleteTyping();
            }
            else
            {
                ShowNextDialogueLine();
            }
        }
    }

    void ShowNextDialogueLine()
    {
        // Check if player has reached the end of the dialogue
        if (step >= speaker.Length)
        {
            dialogueCanvas.SetActive(false);
            step = 0;
            return;
        }

        // Activate the dialogue canvas and update the text and portrait
        dialogueCanvas.SetActive(true);
        speakerText.text = speaker[step];
        portraitImage.sprite = portrait[step];

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(dialogueWords[step]));
        step += 1;
    }

    IEnumerator TypeText(string text)
    {
        // Typewriter effect
        isTyping = true;
        dialogueText.text = "";

        // Clear previous text
        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
    }

    void CompleteTyping()
    {
        // If currently typing, complete the typing effect immediately
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogueText.text = dialogueWords[step - 1];
        isTyping = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player enters the dialogue trigger
        if (collision.CompareTag("Player"))
        {
            dialogueActivated = true;
            step = 0;
            justEntered = true;
            ShowNextDialogueLine();
            StartCoroutine(ResetJustEntered());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the player exits the dialogue trigger
        if (collision.CompareTag("Player"))
        {
            dialogueActivated = false;
            dialogueCanvas.SetActive(false);
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            isTyping = false;
            step = 0;
        }
    }

    IEnumerator ResetJustEntered()
    {
        // Wait for a short duration before allowing dialogue to be activated again
        yield return new WaitForSeconds(0.2f);
        justEntered = false;
    }
}