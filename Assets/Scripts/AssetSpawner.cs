using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssetSpawner : MonoBehaviour
{
    public GameObject[] m_allAssets;

    //public Text[] m_nameTags;

    public Vector3[] assetPosOffset;

    public Vector3[] assetSizeOffset;

    public GameObject spawnAsset(int i)
    {
        GameObject asset = Instantiate(m_allAssets[i], transform.position, Quaternion.identity)as GameObject;
        asset.transform.position = asset.transform.position + assetPosOffset[i];
        asset.transform.localScale = asset.transform.localScale + assetSizeOffset[i];
        if (asset)
        {
            return asset;
        }
        else
        {
            Debug.Log("Warning in spawnAsset");
            return null;
        }
    }
    /*public Text spawnTag(int i)
    {
        Text assetTag = Instantiate(m_nameTags[i], transform.position, Quaternion.identity) as Text;
        if (assetTag)
        {
            return assetTag;
        }
        else
        {
            Debug.Log("Warning in spawnAsset");
            return null;
        }
    }*/
}
