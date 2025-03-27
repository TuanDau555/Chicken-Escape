using UnityEngine;

public class BackgroundSlide : MonoBehaviour
{
    [SerializeField] private float sildeSpeed;
    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * sildeSpeed * Time.fixedDeltaTime);
    }
}
