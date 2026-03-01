using System.Collections;
using UnityEngine;
using TMPro; 

public class GMScript : MonoBehaviour
{
    public static GMScript Instance;

    [Header("Player Setup")]
    public GameObject player;
    private Rigidbody2D playerRb;

    [Header("Level Progression")]
    public Transform[] levelStartPoints;
    private int currentLevelIndex = 0;

    [Header("Transitions & Plot")]
    public CanvasGroup transitionCanvasGroup;
    public TextMeshProUGUI transitionText;
    public float fadeDuration = 1f;
    public float messageDisplayTime = 2f;
    
    [Tooltip("Messages shown AFTER completing each level. Index 0 shows after Level 0, etc.")]
    public string[] levelMessages;
    
    [Header("Troll Level Setup")]
    [Tooltip("The index of the troll level where death equals progression.")]
    public int trollLevelIndex = 1; 

    private bool isTransitioning = false;

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody2D>();
            // Start the game by just fading in from black without a message
            StartCoroutine(FadeTransition(""));
        }
    }

    public void PlayerDied()
    {
        if (isTransitioning) return; 

        if (currentLevelIndex == trollLevelIndex)
        {
            string message = (currentLevelIndex < levelMessages.Length) ? levelMessages[currentLevelIndex] : "";
            
            currentLevelIndex++;
            
            StartCoroutine(FadeTransition(message));
        }
        else
        {
            TeleportToCurrentStart();
        }
    }

    public void LevelComplete()
    {
        if (isTransitioning) return;

        string message = (currentLevelIndex < levelMessages.Length) ? levelMessages[currentLevelIndex] : "";

        currentLevelIndex++;

        if (currentLevelIndex < levelStartPoints.Length)
        {
            StartCoroutine(FadeTransition(message));
        }
    }

    private IEnumerator FadeTransition(string message)
    {
        isTransitioning = true;
        
        if (playerRb != null) playerRb.simulated = false; 

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            transitionCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }

        transitionText.text = message;
        
        if (levelStartPoints.Length > 0 && currentLevelIndex < levelStartPoints.Length)
        {
            TeleportToCurrentStart();
        }

        if (!string.IsNullOrEmpty(message))
        {
            yield return new WaitForSeconds(messageDisplayTime);
        }

        transitionText.text = "";
        
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            transitionCanvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            yield return null;
        }

        if (playerRb != null && currentLevelIndex < levelStartPoints.Length) 
        {
            playerRb.simulated = true; 
        }
        isTransitioning = false;
    }

    private void TeleportToCurrentStart()
    {
        player.transform.position = levelStartPoints[currentLevelIndex].position;

        if (playerRb != null)
        {
            playerRb.velocity = Vector2.zero;
            playerRb.angularVelocity = 0f;
        }
        
        GrappleController grapple = player.GetComponent<GrappleController>();
        if (grapple != null)
        {
            grapple.ForceRelease();
        }
        
        if (AudioManager.Instance != null)
        {
            if (currentLevelIndex == trollLevelIndex)
            {
                AudioManager.Instance.PlayBGM(AudioManager.Instance.trollBGM);
            }
            else
            {
                AudioManager.Instance.PlayBGM(AudioManager.Instance.normalBGM);
            }
        }
        
        if (BackgroundManager.Instance != null)
        {
            BackgroundManager.Instance.LoadCavernForLevel(currentLevelIndex);
        }
    }
}