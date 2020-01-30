using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public float destTime;
    void Start()
    {
        Destroy(gameObject, destTime);
    }

  
}
