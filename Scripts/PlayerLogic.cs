using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hazard"))
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.deathSFX);
            GMScript.Instance.PlayerDied();
        }
        else if (other.CompareTag("Finish"))
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.finishSFX);
            GMScript.Instance.LevelComplete();
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.bounceSFX);
        }
    }
}
