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
        AudioManager.instance.randomPitch = false;
        AudioManager.instance.PlaySFX("Poof");
        vfx.StartVFXCoroutine();
    }

    void OnDisable()
    {
        AudioManager.instance.randomPitch = false;
        AudioManager.instance.PlaySFX("Poof");
        vfx.StartVFXCoroutine();
    }

    public void DrawPond()
    {
        GameControllerManager.instance.canDrawPond = true;
    }

    public void DrawWall()
    {
        GameControllerManager.instance.canDrawWall = true;
    }

    public void DrawHouse()
    {
        GameControllerManager.instance.canDrawHouse = true;
    }

    public void DrawRocks()
    {
        GameControllerManager.instance.canDrawRocks = true;
    }

    public void DrawTree()
    {
        GameControllerManager.instance.canDrawTree = true;
    }

    public void SmallStroke()
    {
        AudioManager.instance.randomPitch = true;
        AudioManager.instance.PlaySFX("SmallStroke");
    }

    public void MediumStroke()
    {
        AudioManager.instance.randomPitch = true;
        AudioManager.instance.PlaySFX("MediumStroke");
    }


}
