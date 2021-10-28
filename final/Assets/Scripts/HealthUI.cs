using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthUI : MonoBehaviour
{
    public TextMeshProUGUI text;
    public void UpdateHealthUI(int count)
    {
        text.text = "Health: " + count;
    }
}
