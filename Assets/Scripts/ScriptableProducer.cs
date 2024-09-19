using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Scriptable Objects/Upgrade")]
public class ScriptableProducer : ScriptableObject
{
    [Range(0,999)]
    [Tooltip("Producer Level")]
    ushort level;
    [Tooltip("Producer Cost")]
    ulong cost;
    [Tooltip("How much Power producer provides")]
    ushort power;
    [Tooltip("Upgrade Button")]
    Button upgradeButton;
    [Tooltip("Producer Image")]
    Image image;
}
