using System.Collections.Generic;
using UnityEngine;

public class GameObjectPooling : MonoBehaviour
{
    private List<GameObject> lstPoolingObj = new List<GameObject>();
    public GameObject objPrefabs;
    public GameObject GetPoolingTile(){
        foreach(var obj in lstPoolingObj){
            if(!obj.activeInHierarchy){
                return obj;
            }
        }
        return AddAndGetTile();
    }

    private GameObject AddAndGetTile(){
        GameObject obj = Instantiate(objPrefabs, gameObject.transform);
        obj.SetActive(false);
        lstPoolingObj.Add(obj);
        return obj;
    }
    public void ClearPoolingTile(){
        foreach(var obj in lstPoolingObj){
            obj.SetActive(false);
        }
    }
}
