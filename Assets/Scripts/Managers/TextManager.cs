using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    [Header("Base Texts")]
    [SerializeField] public TextMeshProUGUI m_MoneyText;
    [SerializeField] public TextMeshProUGUI m_ClicksDoneText;
    [SerializeField] public TextMeshProUGUI m_ClickPowerText;
    [SerializeField] public TextMeshProUGUI m_ClientsText;
    [Header("2G Texts")]
    [SerializeField] public TextMeshProUGUI m_Level2GText;
    [SerializeField] public TextMeshProUGUI m_Cost2GText;
    [Header("Wifi Texts")]
    [SerializeField] public TextMeshProUGUI m_LevelWifiText;
    [SerializeField] public TextMeshProUGUI m_CostWifiText;

    public void UpdateText(TextMeshProUGUI textObject, string text)
    {
        textObject.text = text;
    }
}
