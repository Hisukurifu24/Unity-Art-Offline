using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CloudSystem : MonoBehaviour {
    public GameObject[] clouds;
    public Transform start, end;

    private List<GameObject> activeClouds;

    private void Start() {
        activeClouds = new List<GameObject>();
        StartCoroutine(SpawnCloud());
    }

    private void Update() {
        foreach (GameObject cloud in activeClouds.ToList()) {
            if (cloud.transform.position.x < end.position.x) {
                activeClouds.Remove(cloud);
                Destroy(cloud);
            }
        }
    }

    private IEnumerator SpawnCloud() {
        while (true) {
            int index = Random.Range(0, clouds.Length);
            Vector3 pos = new Vector3(start.position.x, Random.Range(start.position.y, end.position.y), 0);
            GameObject newCloud = Instantiate(clouds[index], pos, clouds[index].transform.rotation);
            newCloud.GetComponent<CloudMove>().speed = Random.Range(1f, 5f);
            activeClouds.Add(newCloud);
            yield return new WaitForSeconds(Random.Range(1f, 10f));
        }
    }
}
