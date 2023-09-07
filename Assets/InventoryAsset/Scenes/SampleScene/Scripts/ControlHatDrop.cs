using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class ControlHatDrop : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    List<GameObject> hats;

    [SerializeField]
    Transform pos1;
    [SerializeField]
    Transform pos2;
    private GameObject curObject;

    // Update is called once per frame
    void Update()
    {
        if(curObject == null)
        {
            curObject = Instantiate(hats[Random.Range(0, hats.Count)],new Vector3(Random.Range(pos1.position.x,pos2.position.x),pos1.position.y,pos1.position.z), Quaternion.identity);
        }
    }
}
