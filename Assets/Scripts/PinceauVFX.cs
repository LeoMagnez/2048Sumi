using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PinceauVFX : MonoBehaviour
{
    [SerializeField] VisualEffect smokeVFX;
    [SerializeField] Transform smokePosition;

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(SpawnParticles());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnParticles()
    {
        VisualEffect newSmokeEffect = Instantiate(smokeVFX, smokePosition);

        newSmokeEffect.Play();
        yield return new WaitForSeconds(0.05f);
        newSmokeEffect.Stop();

        //Destroy(newSmokeEffect.gameObject, 2f);
        
    }
}
