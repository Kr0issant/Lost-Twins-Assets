using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hazard"))
        {
            GMScript.Instance.PlayerDied();
        }
        else if (other.CompareTag("Finish"))
        {
            GMScript.Instance.LevelComplete();
        }
    }
}
