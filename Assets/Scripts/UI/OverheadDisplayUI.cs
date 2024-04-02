using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OverheadDisplayUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;

    public void Construct(string text)
    {
        _text.text = text;
    }

    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
