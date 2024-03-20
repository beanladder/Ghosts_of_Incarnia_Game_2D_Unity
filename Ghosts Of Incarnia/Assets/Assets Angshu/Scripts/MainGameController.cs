using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameController : MonoBehaviour
{
    public GameObject[] MapPrefabs;
    public GameObject PlayerPrefab;
    public int random;
    public GameObject ActiveMap;

    // Start is called before the first frame update
    void Start()
    {
        random = Random.Range(0, MapPrefabs.Length);
        GameObject instantiatedWorld = Instantiate(MapPrefabs[random]);
        GameObject playerPrefab = Instantiate(PlayerPrefab);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
