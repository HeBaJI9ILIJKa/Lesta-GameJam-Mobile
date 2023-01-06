using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _menuPanel, _creditsPanel, _volumeSlider;

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Credits()
    {
        _menuPanel.SetActive(!_menuPanel.activeInHierarchy);
        _creditsPanel.SetActive(!_creditsPanel.activeInHierarchy);
    }

    public void TurnVolumeSlider()
    {
        _volumeSlider.SetActive(!_volumeSlider.activeSelf);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
