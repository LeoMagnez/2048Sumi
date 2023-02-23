using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSceneAnimationEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndAnim()
    {
        GameControllerManager.instance.endSceneAnim = true;
    }

    public void ChoirSound()
    {
        AudioManager.instance.sfxSource.pitch = 1f;
        AudioManager.instance.PlaySFX("Choir");
    }

    public void WhooshSound()
    {
        AudioManager.instance.PlaySFX("Whoosh");
    }

    public void BirdSounds()
    {
        AudioManager.instance.PlaySFX("Birds");
    }
}
