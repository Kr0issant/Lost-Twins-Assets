using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    public Transform transform;
    public float rotationSpeed = 100f;
    void FixedUpdate()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
