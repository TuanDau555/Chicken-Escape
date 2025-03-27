using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] private float conveyorSpeed;

    [SerializeField] private bool _moveRight; // to dedicate which direction is the conveyor is moving
    
    private void OnTriggerStay2D(Collider2D other)
    {
        Vector3 conveyorMovement = (_moveRight ? Vector3.right : Vector3.left) * conveyorSpeed * Time.deltaTime;
        other.transform.Translate(conveyorMovement);
    }
    
}
