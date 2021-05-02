using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateTextUI : MonoBehaviour
{

    private TMP_Text tmpText;

    // Start is called before the first frame update
    void Start()
    {
        tmpText = GetComponent<TMP_Text>();
    }

    public void UpdateText(HurtableObject obj)
    {
        tmpText.SetText(obj.CurrentHealth.ToString());
    }

    public void UpdateText(int value)
    {
        tmpText.SetText(value.ToString());
    }

    public void UpdateText(float value)
    {
        tmpText.SetText((Mathf.Ceil(value)).ToString());
    }
}
