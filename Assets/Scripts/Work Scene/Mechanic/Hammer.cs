using System.Collections;
using UnityEngine;

/// <summary>
/// The mechanic of the hammer is to turn on/off collider to block the player
/// If the player in the collider Die() is called
/// Set the hammer Tag is OBSTACLE
/// </summary>
public class Hammer : MonoBehaviour
{
    [SerializeField] private float hammerChopTime;

    [SerializeField] private Collider2D[] hammerCollider;
    private Animator hammerAnim;

    private void Start()
    {
        hammerAnim = GetComponent<Animator>();
        StartCoroutine(AnimationDelay());
    }


    /// <summary>
    /// When the hammer is not chopping down so player could go through
    /// Sprite 0 is diable (fisrt sprite)
    /// </summary>
    private void DisableCollider()
    {
        foreach(Collider2D collider in hammerCollider)
        {
            if (collider != null)
            {
                collider.enabled = false;
            }
        }
    }

    /// <summary>
    /// the collider is turn on when the hammer is chopping down 
    /// Enable collider at sprite 1 to 7
    /// </summary>
    private void EnableCollider()
    {
        foreach (Collider2D collider in hammerCollider)
        {
            if (collider != null)
            {
                collider.enabled = true;
            }
        }
    }

   

    /// <summary>
    /// For Sequential chop time and random chop time
    /// </summary>
    /// <returns></returns>
    private IEnumerator AnimationDelay()
    {
        yield return new WaitForSeconds(hammerChopTime);
        hammerAnim.Play("Hammer_Chop");

    }

}