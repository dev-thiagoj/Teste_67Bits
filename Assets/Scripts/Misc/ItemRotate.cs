using UnityEngine;

public class ItemRotate : MonoBehaviour
{
    [SerializeField] Vector3 rotationSpeed;


    void Update()
    {
        transform.Rotate(rotationSpeed * 10 * Time.deltaTime);
    }
}
