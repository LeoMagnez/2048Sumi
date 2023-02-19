using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PinceauVFX : MonoBehaviour
{
    [SerializeField] SmokeVFX vfx;

    // Start is called before the first frame update
    void OnEnable()
    {
        vfx.StartVFXCoroutine();
    }

    void OnDisable()
    {
        vfx.StartVFXCoroutine();
    }


}
