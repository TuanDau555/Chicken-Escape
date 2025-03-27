using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box_Pooler : MonoBehaviour
{
    #region Components
    public static Box_Pooler Instance {  get; private set; }

    [SerializeField] private GameObject boxToPool;
    [SerializeField] private int amountToPool;
    [SerializeField] private List<GameObject> pooledBox;
    #endregion

    #region MonoBehaviour
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CreateBox();
    }
    #endregion

    #region Pooled
    private void CreateBox()
    {
        pooledBox = new List<GameObject>();
        // Loop list, deactive them and add when Instantiate 
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject box = (GameObject)Instantiate(boxToPool);
            box.SetActive(false); // make them unactive at first
            pooledBox.Add(box); // Add to pool if pool don't have box
            box.transform.SetParent(this.transform); // make it a child of dispenser
        }
    }

    public GameObject GetPoolerBox()
    {
        for(int i  = 0; i < pooledBox.Count; i++)
        {
            if (!pooledBox[i].activeInHierarchy) // if not active...
            {
                //... avtive it
                return pooledBox[i];
            }
        }

        //... else
        return null;
    }
    #endregion
}
