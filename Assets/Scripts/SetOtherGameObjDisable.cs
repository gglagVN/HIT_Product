using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetOtherGameObjDisable : MonoBehaviour
{
    public GameObject[] listGameObject;
    public void Disable()
    {
        foreach (GameObject go in listGameObject)
        {
            go.SetActive(false);
        }
    }
}
