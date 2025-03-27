using System.Collections;
using UnityEngine;

public class Dispenser : MonoBehaviour
{
    #region Component
    private Coroutine dispenseCoroutine;

    [SerializeField] private float delayTime;
    [SerializeField] private float dispenseTime;
    [SerializeField] private float boxLifeTime;
    #endregion

    private void Awake()
    {
        StartDispense();
    }

    #region Dispense
    private void StartDispense()
    {
        if(dispenseCoroutine == null)
        {
            dispenseCoroutine = StartCoroutine(Dispensing());
        }
    }

    private void StopDispense()
    {
        if (dispenseCoroutine != null)
        {
            StopCoroutine(dispenseCoroutine);
            dispenseCoroutine = null;
            Debug.Log("Stopped dispensing");
        }
    } // use in OnDisable func (Optional)

    private IEnumerator Dispensing()
    {
        while (true)
        {
            yield return new WaitForSeconds(delayTime);

            GameObject box = Box_Pooler.Instance.GetPoolerBox();
            if (box != null)
            {
                box.transform.position = transform.position;
                box.SetActive(true);
                StartCoroutine(BoxLifeTime(box));
            }
            else
            {
                break;
            }
        }

        yield return new WaitForSeconds(dispenseTime);
    }

    private IEnumerator BoxLifeTime(GameObject box)
    {
        yield return new WaitForSeconds(boxLifeTime);
        
        if(box.activeInHierarchy)
        {
            box.SetActive(false);
        }
    }
    #endregion
}
