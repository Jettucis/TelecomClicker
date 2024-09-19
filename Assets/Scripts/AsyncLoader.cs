using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AsyncLoader : MonoBehaviour
{
    [Header("Menu Screens")]
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private GameObject _mainMenu;
    [Header("Slider")]
    [SerializeField] private Slider _loadingSlider;

    public void LoadLevel(string level)
    {
        _mainMenu.SetActive(false);
        _loadingScreen.SetActive(true);

        // Async
        StartCoroutine(LoadLevelAsync(level));
    }

    IEnumerator LoadLevelAsync(string level)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(level);

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            _loadingSlider.value = progressValue;
            yield return null;
        }
    }
}
