using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectPoolTest : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(idk());
    }

    IEnumerator idk()
    {
        yield return new WaitForSeconds(Random.Range(0.9f, 10f));
        gameObject.SetActive(false);
    }
}
