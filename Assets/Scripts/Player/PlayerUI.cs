using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI promtText;

    private string currentPromt;

    public void UpdateText(string promtMessage)
    {
        if (currentPromt == promtMessage)
            return;

        currentPromt = promtMessage;
        promtText.text = promtMessage;
    }
}
