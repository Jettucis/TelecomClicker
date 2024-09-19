using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public PlayerData m_PlayerData;
    public SaveManager m_SaveManager;
    public TextManager m_TextManager;
    public SpriteManager m_SpriteManager;
    public BlockerManager m_BlockerManager;
    public CursorManager m_CursorManager;
    public ButtonManager m_ButtonManager;

    private Random _random = new();
    private readonly int _randomMax = 1000;

    //For Super Clicks
    private int _clickCount = 0;
    private float _clickTimeWindow = 0.2f;
    private float _timeSinceLastClick = 0f;
    private bool _isCountingClicks = false;

    // For Blockers
    private bool _isWifiUnlocked = false;
    private bool _is2GUnlocked = false;

    private void Start()
    {
        m_SaveManager.Load();

        UpdateAllUI();


        ShouldUnlock(m_BlockerManager.Blocker2G, m_PlayerData.ClicksDone);
        ShouldUnlock(m_BlockerManager.BlockerWifi, m_PlayerData.Level2G);

        if (m_PlayerData.LevelWifi > 0)
        {
            GameObject.Find("PressButton").GetComponent<Image>().sprite = m_SpriteManager.m_SpriteLevel2;
        }

        CanBtnBeInteractable(m_ButtonManager.m_UpgradeButton2G, m_PlayerData.Cost2G);
        CanBtnBeInteractable(m_ButtonManager.m_UpgradeButtonWifi, m_PlayerData.CostWifi);
        //CanBtnBeInteractable(_level3GButton, playerData.Cost2G, playerData.Level2G);
        //CanBtnBeInteractable(_level4GButton, playerData.Cost2G, playerData.Level2G);
        //CanBtnBeInteractable(_level5GButton, playerData.Cost2G, playerData.Level2G);
        //CanBtnBeInteractable(_level6GButton, playerData.Cost2G, playerData.Level2G);
    }
    // FixedUpdate > Update
    private void FixedUpdate()
    {
        if (m_PlayerData.Clients == 0)
        {
            CanBtnBeInteractable(m_ButtonManager.m_UpgradeButton2G, m_PlayerData.Cost2G);
            return;
        } 

        m_PlayerData.Money += m_PlayerData.Clients * Time.deltaTime;
        UpdateText(m_TextManager.m_MoneyText, m_PlayerData.Money.ToString("F0") + " $");
        CanBtnBeInteractable(m_ButtonManager.m_UpgradeButton2G, m_PlayerData.Cost2G);
        CanBtnBeInteractable(m_ButtonManager.m_UpgradeButtonWifi, m_PlayerData.CostWifi);
    }

    private void OnApplicationQuit()
    {
        m_SaveManager.Save();
    }
    public void ClickPress()
    {
        if (!_isCountingClicks)
        {
            StartCoroutine(ClickTimer());
        }

        _clickCount++;

        if (_clickCount >= 4)
        {
            m_PlayerData.Money += m_PlayerData.ClickPower * 5;
            m_PlayerData.ClicksDone++;
            AudioManager.m_Instance.PlaySound("SuperClick");
            ResetClickData();
        }
        else
        {
            m_PlayerData.Money += m_PlayerData.ClickPower;
            m_PlayerData.ClicksDone++;
            AudioManager.m_Instance.PlaySound("Click");
        }

        if (!_is2GUnlocked && m_PlayerData.ClicksDone >= 15)
        {
            m_BlockerManager.Blocker2G.SetActive(false);
            _is2GUnlocked = true;
            AudioManager.m_Instance.PlaySound("Unlock");
        }

        UpdateText(m_TextManager.m_MoneyText, m_PlayerData.Money.ToString("F0") + " $");
        UpdateText(m_TextManager.m_ClicksDoneText, m_PlayerData.ClicksDone.ToString());

        // X no 1000 iespēja iegūt klientu uz katra click. Pagaidām constants, bet tā būtu kkāda prestige sistēma
        if (ClientRoll(1, _randomMax))
        {
            m_PlayerData.Clients++;
            UpdateText(m_TextManager.m_ClientsText, m_PlayerData.Clients.ToString());
        }
    }

    private IEnumerator ClickTimer()
    {
        _isCountingClicks = true;
        _timeSinceLastClick = 0f;

        while (_timeSinceLastClick < _clickTimeWindow)
        {
            _timeSinceLastClick += Time.deltaTime;
            yield return null;
        }

        ResetClickData();
    }

    private void ResetClickData()
    {
        _clickCount = 0;
        _timeSinceLastClick = 0f;
        _isCountingClicks= false;
        StopAllCoroutines();
    }

    public void Upgrade2G()
    {
        if (m_PlayerData.Money < m_PlayerData.Cost2G) return;

        m_PlayerData.Level2G++;
        m_PlayerData.Money -= m_PlayerData.Cost2G;
        m_PlayerData.ClickPower++;
        m_PlayerData.Clients++;
        m_PlayerData.Cost2G = (m_PlayerData.ClickPower * 10 * m_PlayerData.Clients);

        UpdateDefaultUI();
        UpdateText(m_TextManager.m_Level2GText, m_PlayerData.Level2G.ToString());
        UpdateText(m_TextManager.m_Cost2GText, m_PlayerData.Cost2G.ToString() + " $");

        if (!_isWifiUnlocked && m_PlayerData.Level2G >= 15)
        {
            m_BlockerManager.BlockerWifi.SetActive(false);
            _isWifiUnlocked = true;
            AudioManager.m_Instance.PlaySound("Unlock");
        }
        if (m_PlayerData.Level2G == 999)
        {
            m_ButtonManager.m_UpgradeButton2G.interactable = false;
        }

        AudioManager.m_Instance.PlaySound("");
    }

    public void UpgradeWifi()
    {
        if (m_PlayerData.Money < m_PlayerData.CostWifi) return;

        m_PlayerData.LevelWifi++;

        if (m_PlayerData.LevelWifi == 1)
        {
            GameObject.Find("PressButton").GetComponent<Image>().sprite = m_SpriteManager.m_SpriteLevel2;
        }

        m_PlayerData.Money -= m_PlayerData.CostWifi;
        m_PlayerData.ClickPower += 10;
        m_PlayerData.Clients += 5;
        m_PlayerData.CostWifi = (10 * m_PlayerData.Clients) + 3000;

        UpdateDefaultUI();
        UpdateText(m_TextManager.m_LevelWifiText, m_PlayerData.LevelWifi.ToString());
        UpdateText(m_TextManager.m_CostWifiText, m_PlayerData.CostWifi.ToString() + " $");
    }
    private void UpdateText(TextMeshProUGUI textObject, string text)
    {
        textObject.text = text;
    }
    private bool ClientRoll(int chance, int maxChance)
    {
        return _random.Next(1, maxChance + 1) <= chance;
    }

    private void UpdateDefaultUI()
    {
        UpdateText(m_TextManager.m_MoneyText, m_PlayerData.Money.ToString("F0") + " $");
        UpdateText(m_TextManager.m_ClickPowerText, m_PlayerData.ClickPower.ToString());
        UpdateText(m_TextManager.m_ClientsText, m_PlayerData.Clients.ToString());
    }
    private void UpdateAllUI()
    {
        UpdateText(m_TextManager.m_ClicksDoneText, m_PlayerData.ClicksDone.ToString());

        UpdateText(m_TextManager.m_MoneyText, m_PlayerData.Money.ToString("F0") + " $");
        UpdateText(m_TextManager.m_ClickPowerText, m_PlayerData.ClickPower.ToString());
        UpdateText(m_TextManager.m_ClientsText, m_PlayerData.Clients.ToString());

        UpdateText(m_TextManager.m_Level2GText, m_PlayerData.Level2G.ToString());
        UpdateText(m_TextManager.m_Cost2GText, m_PlayerData.Cost2G.ToString() + " $");

        UpdateText(m_TextManager.m_LevelWifiText, m_PlayerData.LevelWifi.ToString());
        UpdateText(m_TextManager.m_CostWifiText, m_PlayerData.CostWifi.ToString() + " $");
    }
    private void CanBtnBeInteractable(Button btn, int cost)
    {
        if (m_PlayerData.Money < cost)
            btn.interactable = false;
        else
            btn.interactable = true;
    }
    private void ShouldUnlock(GameObject gameObject, int level)
    {
        if (level >= 15)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);
    }
    public void DebugReloadData()
    {
        m_PlayerData.ResetData();
        m_SaveManager.Save();
        GameObject.Find("PressButton").GetComponent<Image>().sprite = m_SpriteManager.m_SpriteLevel1;
        m_BlockerManager.EnableAll();

        UpdateAllUI();
    }
    public void DebugGiveMoney()
    {
        m_PlayerData.Money += 1000;
        UpdateText(m_TextManager.m_MoneyText, m_PlayerData.Money.ToString("F0") + " $");
    }
}
