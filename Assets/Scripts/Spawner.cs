using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject m_SuperClickTextPrefab;
    public GameObject m_ClickTextPrefab;

    [SerializeField] private float m_xAxisMin;
    [SerializeField] private float m_xAxisMax;
    [SerializeField] private float m_yAxisMin;
    [SerializeField] private float m_yAxisMax;

    public void SpawnClickText(int value)
    {
        Vector3 pos = GenerateTransformVector(transform);
        m_ClickTextPrefab.GetComponent<TextMeshProUGUI>().text = value.ToString();
        var textObject = (GameObject)Instantiate(m_ClickTextPrefab, pos, Quaternion.identity, transform);
        StartCoroutine(Timer(textObject));
    }

    public void SpawnSuperClickText(int value)
    {
        Vector3 pos = GenerateTransformVector(transform);
        m_SuperClickTextPrefab.GetComponent<TextMeshProUGUI>().text = value.ToString();
        var textObject = (GameObject)Instantiate(m_SuperClickTextPrefab, pos, Quaternion.identity, transform);
        StartCoroutine(Timer(textObject));
    }

    private Vector3 GenerateTransformVector(Transform origin)
    {
        float randomX = Random.Range(m_xAxisMin, m_xAxisMax);
        float randomY = Random.Range(m_yAxisMin, m_yAxisMax);

        return new Vector3(origin.position.x + randomX, origin.position.y + randomY);
    }

    private IEnumerator Timer(GameObject obj)
    {
        float i = 0f;

        while (i < 1f)
        {
            i += Time.deltaTime;
            yield return null;
        }

        Destroy(obj);
    }
}
