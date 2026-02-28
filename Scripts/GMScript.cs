using UnityEngine;

public class GMScript : MonoBehaviour
{
    public static GMScript Instance;

    [Header("Player Setup")]
    public GameObject player;
    private Rigidbody2D playerRb;

    [Header("Level Progression")]
    [Tooltip("Drag empty GameObjects here to act as spawn points for each level.")]
    public Transform[] levelStartPoints;
    
    private int currentLevelIndex = 0;

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
            RespawnPlayer();
        }
    }

    public void PlayerDied()
    {
        // Add death particle effects or sound here later
        RespawnPlayer();
    }

    public void LevelComplete()
    {
        Debug.Log(currentLevelIndex);
        currentLevelIndex++;
        Debug.Log(currentLevelIndex);

        if (currentLevelIndex < levelStartPoints.Length)
        {
            RespawnPlayer();
        }
        else
        {
            // Player reached the final twin ball
            Debug.Log("Game Finished! Show Win UI.");
            // Freeze player
            if (playerRb != null) { playerRb.simulated = false; }
        }
    }

    private void RespawnPlayer()
    {
        if (levelStartPoints.Length == 0) return;

        // Move the player to the current level's spawn point
        player.transform.position = levelStartPoints[currentLevelIndex].position;

        // Kill all momentum
        if (playerRb != null)
        {
            playerRb.velocity = Vector2.zero;
            playerRb.angularVelocity = 0f;
        }
    }
}
