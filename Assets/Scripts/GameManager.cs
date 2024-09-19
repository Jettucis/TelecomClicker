using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public PlayerData playerData;
    public SaveManager saveManager;


    public GameObject _level2Blocker;
    // Buttons
    public Button _level2GButton;
    public Button _levelWifiButton;
    public Button _level3GButton;
    public Button _level4GButton;
    public Button _level5GButton;
    public Button _level6GButton;

    // Texts 
    public TextMeshProUGUI _moneyText;
    public TextMeshProUGUI _clicksDoneText;


    public TextMeshProUGUI _clickPowerLevel;
    public TextMeshProUGUI _clickPowerCostText;
    public TextMeshProUGUI _clickPowerText;

    public TextMeshProUGUI _clientsText;

    private Random _random = new();
    private readonly int _randomMax = 1000;

    private bool _isWifiUnlocked = false;

    public TextMeshProUGUI _wifiLevelText;
    public TextMeshProUGUI _wifiCostText;

    // Sprites
    [SerializeField] Sprite SpriteLevel1;
    [SerializeField] Sprite SpriteLevel2;
    [SerializeField] Sprite SpriteLevel3;
    [SerializeField] Sprite SpriteLevel4;
    [SerializeField] Sprite SpriteLevel5;
    [SerializeField] Sprite SpriteLevel6;

    void Start()
    {
        saveManager.Load();

        UpdateAllUI();

        ShouldUnlock(_level2Blocker, playerData.Level2G);

        CanBtnBeInteractable(_level2GButton, playerData.Cost2G);
        CanBtnBeInteractable(_levelWifiButton, playerData.CostWifi);
        //CanBtnBeInteractable(_level3GButton, playerData.Cost2G, playerData.Level2G);
        //CanBtnBeInteractable(_level4GButton, playerData.Cost2G, playerData.Level2G);
        //CanBtnBeInteractable(_level5GButton, playerData.Cost2G, playerData.Level2G);
        //CanBtnBeInteractable(_level6GButton, playerData.Cost2G, playerData.Level2G);


        StartCoroutine(ToggleAutoSave());
    }

    IEnumerator ToggleAutoSave()
    {
        yield return new WaitForSeconds(60);
        saveManager.Save();
        Debug.Log("Performed Save from coroutine!");
    }
    // Lietoju FixedUpdate, jo Update izsaucas katru frame, jebšu spēle strādās ātrāk/lēnāk uz dažādām ierīcēm.
    private void FixedUpdate()
    {
        // Sākumā bija savādāka loģika, bet uzliku šādi, lai sākumā naudu updeito un tad skatās vai pogai jābūt ieslēgtai.
        // Savādāk lietotājs UI redzēs, ka viņam pietiek nauda "upgreidam", bet poga būs izslēgta
        if (playerData.Clients == 0)
        {
            CanBtnBeInteractable(_level2GButton, playerData.Cost2G);
            return;
        } 

        playerData.Money += playerData.Clients * Time.deltaTime;
        UpdateText(_moneyText, playerData.Money.ToString("F0") + " $");
        CanBtnBeInteractable(_level2GButton, playerData.Cost2G);
        CanBtnBeInteractable(_levelWifiButton, playerData.CostWifi);
    }

    private void OnApplicationQuit()
    {
        saveManager.Save();
        Debug.Log("Performed Save from OnApplicationQuit!");
    }
    public void ClickPress()
    {
        playerData.Money += playerData.ClickPower;
        playerData.ClicksDone++;
        UpdateText(_moneyText, playerData.Money.ToString("F0") + " $");
        UpdateText(_clicksDoneText, playerData.ClicksDone.ToString());

        // X no 1000 iespēja iegūt klientu uz katra click. Pagaidām constants, bet tā būtu kkāda prestige sistēma
        if (ClientRoll(1, _randomMax))
        {
            playerData.Clients++;
            UpdateText(_clientsText, playerData.Clients.ToString());
        }
    }

    public void Upgrade2G()
    {
        if (playerData.Money < playerData.Cost2G) return;

        playerData.Level2G++;
        playerData.Money -= playerData.Cost2G;
        playerData.ClickPower++;
        playerData.Clients++;
        playerData.Cost2G = (playerData.ClickPower * 10 * playerData.Clients);

        UpdateDefaultUI();
        UpdateText(_clickPowerLevel, playerData.Level2G.ToString());
        UpdateText(_clickPowerCostText, playerData.Cost2G.ToString() + " $");

        if (!_isWifiUnlocked && playerData.Level2G >= 15)
        {
            _level2Blocker.SetActive(false);
            _isWifiUnlocked = true;
        }
        if (playerData.Level2G == 999)
        {
            _level2GButton.interactable = false;
        }
    }

    public void UpgradeWifi()
    {
        if (playerData.Money < playerData.CostWifi) return;

        playerData.LevelWifi++;

        if (playerData.LevelWifi == 1)
        {
            GameObject.Find("PressButton").GetComponent<Image>().sprite = SpriteLevel2;
        }

        playerData.Money -= playerData.CostWifi;
        playerData.ClickPower += 10;
        playerData.Clients += 5;
        playerData.CostWifi = (10 * playerData.Clients) + 3000;

        UpdateDefaultUI();
        UpdateText(_wifiLevelText, playerData.LevelWifi.ToString());
        UpdateText(_wifiCostText, playerData.CostWifi.ToString() + " $");
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
        UpdateText(_moneyText, playerData.Money.ToString("F0") + " $");
        UpdateText(_clickPowerText, playerData.ClickPower.ToString());
        UpdateText(_clientsText, playerData.Clients.ToString());
    }
    private void UpdateAllUI()
    {
        UpdateText(_clicksDoneText, playerData.ClicksDone.ToString());

        UpdateText(_moneyText, playerData.Money.ToString("F0") + " $");
        UpdateText(_clickPowerText, playerData.ClickPower.ToString());
        UpdateText(_clientsText, playerData.Clients.ToString());

        UpdateText(_clickPowerLevel, playerData.Level2G.ToString());
        UpdateText(_clickPowerCostText, playerData.Cost2G.ToString() + " $");

        UpdateText(_wifiLevelText, playerData.LevelWifi.ToString());
        UpdateText(_wifiCostText, playerData.CostWifi.ToString() + " $");
    }
    private void CanBtnBeInteractable(Button btn, int cost)
    {
        if (playerData.Money < cost)
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
        playerData.ResetData();
        saveManager.Save();
        GameObject.Find("PressButton").GetComponent<Image>().sprite = SpriteLevel1;
        _level2Blocker.SetActive(true);
        //LockUnlockLevel("Level3Blocker", false);
        UpdateAllUI();
    }
    public void DebugGiveMoney()
    {
        playerData.Money += 1000;
        UpdateText(_moneyText, playerData.Money.ToString("F0") + " $");
    }
}
