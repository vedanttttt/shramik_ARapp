using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssetController : MonoBehaviour
{
    public GameObject startPanel;
    GameObject m_activeSpawner;
    public Text m_nameTags;
    string[] m_spawnerTags = { "Control Module", "Cooling Tank", "Drill Hand", "Industrial Motor", "Pipes", "Robot Hand" };
    AssetSpawner m_spawner;
    public int i;
    TouchController tc;

    /*public void toggleAsset()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            previous();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            next();
        }
    }*/

    public void previous()
    {
        
        if (i == 0)
        {
            i = m_spawner.m_allAssets.Length-1;
        }
        else
        {
            i = i - 1;
        }
        Destroy(m_activeSpawner);
        //Destroy(m_activeSpawnerTag);
        m_activeSpawner = m_spawner.spawnAsset(i);
        //m_activeSpawnerTag = m_spawner.spawnTag(i);
        m_nameTags.text = m_spawnerTags[i];
    }

    public void next()
    {
        
        if (i == ((m_spawner.m_allAssets.Length)-1))
        {
            i = 0;
        }
        else
        {
            i = i + 1;
        }
        Destroy(m_activeSpawner);
        //Destroy(m_activeSpawnerTag);
        m_activeSpawner = m_spawner.spawnAsset(i);
        m_nameTags.text = m_spawnerTags[i];
    }

    void rotate()
    {
        m_activeSpawner.transform.Rotate(new Vector3(0f, -100f, 0f) * 0.01f);// Time.deltaTime);
        //transform.Rotate(new Vector3(0f, 100f, 0f) * Time.deltaTime);
    }
    void Start()
    {
        m_spawner = GameObject.FindWithTag("AssetSpawner").GetComponent<AssetSpawner>();

        i = m_spawner.m_allAssets.Length-1;
        int r = Random.Range(0, i);
        m_activeSpawner =m_spawner.spawnAsset(r);
        //m_activeSpawnerTag = m_spawner.spawnTag(r);
        m_nameTags.text = m_spawnerTags[r];
    }
    
    void Update()
    {
        //toggleAsset();
        rotate();
        //
        ///
        if (m_didTap)
        {
            startPanel.SetActive(false);
            //m_activeSpawner = m_spawner.spawnAsset(Random.Range(0, i));
            //rotate();
        }
        touchChangeAsset();
        //
        ///
    }

    void touchChangeAsset()
    {
        if (m_swipeDirection == Direction.right && Time.time > m_timeToNextSwipe)
        {
            previous();
            m_timeToNextSwipe = Time.time + m_minTimeToSwipe;
            m_swipeDirection = Direction.none;
        }
        if (m_swipeDirection == Direction.left && Time.time > m_timeToNextSwipe)
        {
            next();
            m_timeToNextSwipe = Time.time + m_minTimeToSwipe;
            m_swipeDirection = Direction.none;
        }
    }

    enum Direction { none, left, right, up, down }
    
    Direction m_swipeDirection = Direction.none;
    
    float m_timeToNextSwipe;

    [Range(0.05f, 1f)]
    public float m_minTimeToSwipe = 0.3f;

    bool m_didTap = false;

    private void OnEnable()
    {
        TouchController.swipeEvent += swipeHandler;
        TouchController.tapEvent += tapHandler;
    }

    private void OnDisable()
    {
        TouchController.swipeEvent -= swipeHandler;
        TouchController.tapEvent -= tapHandler;
    }

    void swipeHandler(Vector2 swipeMovement)
    {
        m_swipeDirection = getDirection(swipeMovement);
    }

    void tapHandler(Vector2 swipeMovement)
    {
        m_didTap = true;
    }

    Direction getDirection(Vector2 swipeMovement)
    {
        Direction swipeDir = Direction.none;

        //hor
        if (Mathf.Abs(swipeMovement.x) > Mathf.Abs(swipeMovement.y))
        {
            swipeDir = (swipeMovement.x >= 0) ? Direction.right : Direction.left;
            Debug.Log("Left Or Right");
        }

        //ver
        else
        {
            swipeDir = (swipeMovement.y >= 0) ? Direction.up : Direction.down;
            Debug.Log("Up or Down");
        }
        return swipeDir;
    }
}
