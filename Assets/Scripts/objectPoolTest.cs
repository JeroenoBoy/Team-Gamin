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
        yield return new WaitForSeconds(7);
        gameObject.SetActive(false);
    }
}
