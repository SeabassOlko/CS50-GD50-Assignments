using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSpawner : MonoBehaviour
{

    public GameObject[] prefab;

    // Start is called before the first frame update
    void Start()
    {
        
        StartCoroutine(SpawnGem());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnGem() {
		while (true) {

			// instantiate gem at random height
            Instantiate(prefab[Random.Range(0, prefab.Length)], new Vector3(26, Random.Range(-5, 10), 10), Quaternion.identity);
			

			//pause 1-10 seconds until the next gem spawns
			yield return new WaitForSeconds(Random.Range(1, 10));
		}
	}

}
