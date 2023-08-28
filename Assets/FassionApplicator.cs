using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class FassionApplicator : MonoBehaviour
{
    [SerializeField] GameObject hatFlippedFalse;
    [SerializeField] GameObject hatFlippedTrue;

    [SerializeField] Sprite turqoiseHat;
    [SerializeField] Sprite boringHat;
    [SerializeField] Sprite redHat;

    SpriteRenderer srHatFlippedFalse;
    SpriteRenderer srHatFlippedTrue;

    private void Start()
    {
        srHatFlippedFalse= hatFlippedFalse.GetComponent<SpriteRenderer>();
        srHatFlippedTrue = hatFlippedTrue.GetComponent<SpriteRenderer>();
    }
    public void SetTurqoiseHat()
    {
        srHatFlippedFalse.sprite = turqoiseHat;
        srHatFlippedTrue.sprite= turqoiseHat;
    }
    public void SetBoringHat()
    {
        srHatFlippedFalse.sprite = boringHat;
        srHatFlippedTrue.sprite = boringHat;
    }
    public void SetRedHat()
    {
        srHatFlippedFalse.sprite = redHat;
        srHatFlippedTrue.sprite = redHat;
    }
}
