using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointScript : MonoBehaviour
{
    public bool isBusy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isBusy = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isBusy = false;
    }
}
