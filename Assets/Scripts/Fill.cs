using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Fill : MonoBehaviour
{

    public int value;
    [SerializeField] TextMeshProUGUI valueDisplay;
    [SerializeField] float speed;

    bool hasCombined;

    public void FillValueUpdate(int valueIn)
    {
        value = valueIn;
        valueDisplay.text = value.ToString();

    }

    private void Update()
    {
        if(transform.localPosition != Vector3.zero)
        {
            hasCombined = false;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, speed * Time.deltaTime);
        }
        else if (!hasCombined)
        {
            if(transform.parent.GetChild(0) != this.transform)
            {
                Destroy(transform.parent.GetChild(0).gameObject);
                
            }
            hasCombined = true;
        }
    }

    public void Double()
    {
        value *= 2;
        if(value > GameControllerManager.instance.currMaxValue)
        {
            GameControllerManager.instance.currMaxValue = value;
        }
        valueDisplay.text = value.ToString();
        

    }

    
}
