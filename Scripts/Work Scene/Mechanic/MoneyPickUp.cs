using UnityEngine;

public class MoneyPickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            playerInventory.moneyCount++;
            AudioManager.Instance.PlaySFX(AudioManager.Instance.collectMoney); //collect sound
        }
    }
}
