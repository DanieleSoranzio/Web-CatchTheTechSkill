using UnityEngine;

public class RotateObj : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    void Update()
    {
        transform.Rotate(Vector3.back * (rotationSpeed * Time.deltaTime));
    }
}
