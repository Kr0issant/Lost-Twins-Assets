using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager Instance;

    [Header("UI Layers")]
    public RawImage[] backgroundLayers;

    [Header("Parallax Settings")]
    [Tooltip("How fast each layer moves. Index 0 (Back) should be very small (e.g. 0.01), Index 4 (Front) larger (e.g. 0.1).")]
    public float[] parallaxSpeeds;
    
    [Tooltip("A general multiplier to tune the overall parallax speed without editing every value.")]
    public float globalParallaxMultiplier = 0.05f;

    [Header("Level Mapping")]
    public int[] levelToCavernMap;

    private Camera mainCam;

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if (mainCam == null) return;

        // Get the actual world position of the camera
        Vector2 camPos = mainCam.transform.position;

        for (int i = 0; i < backgroundLayers.Length; i++)
        {
            // Skip if arrays are mismatched or missing
            if (backgroundLayers[i] == null || backgroundLayers[i].texture == null || parallaxSpeeds.Length <= i) continue;

            // Calculate the UV offset based on camera position and specific layer speed
            float offsetX = camPos.x * parallaxSpeeds[i] * globalParallaxMultiplier;
            float offsetY = camPos.y * parallaxSpeeds[i] * globalParallaxMultiplier;

            // Apply the offset. The 1f, 1f at the end keeps the image at its original scale.
            backgroundLayers[i].uvRect = new Rect(offsetX, offsetY, 1f, 1f);
        }
    }

    public void LoadCavernForLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levelToCavernMap.Length) return;

        int cavernNumber = levelToCavernMap[levelIndex];

        for (int i = 0; i < backgroundLayers.Length; i++)
        {
            int layerNumber = 5 - i; 
            string path = $"Caverns{cavernNumber}/Layer{layerNumber}";
            
            Texture2D loadedTexture = Resources.Load<Texture2D>(path);

            if (loadedTexture != null)
            {
                backgroundLayers[i].texture = loadedTexture;
            }
            else
            {
                Debug.LogWarning($"Failed to load texture at Resources/{path}");
            }
        }
    }
}