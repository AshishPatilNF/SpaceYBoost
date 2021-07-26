using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;

    public void UpdateFuel(int fuel)
    {
        text.text = "Fuel: " + fuel;
    }
}
