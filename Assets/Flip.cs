using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : MonoBehaviour
{
    // Start is called before the first frame update
    SpriteRenderer sp; 
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        sp.flipX=transform.parent.GetComponent<SpriteRenderer>().flipX;
    }
}
