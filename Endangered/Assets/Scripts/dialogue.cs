using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class dialogue : MonoBehaviour
{
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
    private Coroutine typingCoroutine;
    private bool justEntered = false;

    [Header("Typing Settings")]
    [SerializeField] private float typeSpeed = 0.18f;          // slower time between letters
    [SerializeField] private float punctuationPause = 0.6f;    // longer pause after punctuation
    [SerializeField] private int soundFrequency = 4;           // play sound every 4 letters

    [Header("Typing Sounds")]
    [SerializeField] private AudioClip Dialogue1;
    [SerializeField] private AudioClip Dialogue2;
    private AudioSource audioSource;
    private bool toggleSound = false;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (dialogueActivated && Input.GetKeyDown(KeyCode.T) && !justEntered)
        {
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
        if (step >= speaker.Length)
        {
            dialogueCanvas.SetActive(false);
            step = 0;
            return;
        }

        dialogueCanvas.SetActive(true);
        speakerText.text = speaker[step];
        portraitImage.sprite = portrait[step];

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(dialogueWords[step]));
        step++;
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        int letterIndex = 0;

        foreach (char c in text)
        {
            dialogueText.text += c;

            if (!char.IsWhiteSpace(c) && letterIndex % soundFrequency == 0)
            {
                PlayLetterSound();
            }

            letterIndex++;

            if (".,!?".Contains(c.ToString()))
            {
                yield return new WaitForSeconds(punctuationPause);
            }
            else
            {
                yield return new WaitForSeconds(typeSpeed);
            }
        }

        isTyping = false;
    }

    void PlayLetterSound()
    {
        audioSource.PlayOneShot(toggleSound ? Dialogue1 : Dialogue2);
        toggleSound = !toggleSound;
    }

    void CompleteTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogueText.text = dialogueWords[step - 1];
        isTyping = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
        yield return new WaitForSeconds(0.2f);
        justEntered = false;
    }
}
