using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockerManager : MonoBehaviour
{
    [Header("Level Blockers")]
    public GameObject Blocker2G;
    public GameObject BlockerWifi;
    public GameObject Blocker3G;
    public GameObject Blocker4G;
    public GameObject Blocker5G;
    public GameObject Blocker6G;

    public void EnableAll()
    {
        Blocker2G.SetActive(true);
        BlockerWifi.SetActive(true);
        Blocker3G.SetActive(true);
        Blocker4G.SetActive(true);
        Blocker5G.SetActive(true);
        Blocker6G.SetActive(true);
    }
    // Useless method since we can modify object directly, but using for readability purposes.
    public void Disable(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}
