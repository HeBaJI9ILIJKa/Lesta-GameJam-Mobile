using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _orderPanel;

    [SerializeField]
    private GameObject _scorePanel;

    [SerializeField]
    private GameObject _abilitiesPanel;

    [SerializeField]
    private GameObject _gameOverPanel;

    [SerializeField]
    private GameObject _pauseScreen;

    [SerializeField]
    private GameObject _rulesScreen;

    [SerializeField]
    private GameObject _gameScreen;

    [SerializeField]
    private TextMeshProUGUI _textDeathPanelScore;

    [SerializeField]
    private TextMeshProUGUI _textUICurrentGold;

    [SerializeField]
    private TextMeshProUGUI _textUIEarned;

    [SerializeField]
    private TextMeshProUGUI _textUIHighScore;

    [SerializeField]
    private Image _imageMeleeAttack;

    [SerializeField]
    private Image _imageRangeAttack;

    [SerializeField]
    private Image _imageGrenade;

    [SerializeField]
    private Image _imageOrder;
    [SerializeField]
    private BasePlayer _player;

    [SerializeField]
    private Sprite[] _orderSprites;

    private AbilityType _activeFireType;

    private int _currentDisplayedOrder = 0;

    private void OnEnable()
    {
        EventManager.OnGameOver.AddListener(ShowDeathPanel);

        EventManager.OnGoldChanged.AddListener(ChangeGold);
        EventManager.OnHighscoreChanged.AddListener(ChangeHighscore);

        EventManager.OnOrderChanged.AddListener(ChangeOrder);

        EventManager.OnAbilityChanged.AddListener(UseAbility);
    }

    void Start()
    {
        //ChangeFireType(_activeFireType);  
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    private void ShowDeathPanel() 
    {
        _gameOverPanel.SetActive(true);
        _textDeathPanelScore.SetText(Score.TotalEarned.ToString());
        SetUI(false);
    }


    private void ChangeGold(int value) 
    {
        //_goldAmount += goldChange;
        //if(_goldAmount < 0)
        //{
        //    _goldAmount = 0;
        //}
        //if (goldChange > 0)
        //{
        //    _totalEarned += Math.Abs(goldChange);
        //}

        //_textUICurrentGold.SetText(_goldAmount.ToString());
        //_textUIEarned.SetText(_totalEarned.ToString());

        //if (_goldAmount > _highScore)
        //{
        //    ChangeHighScore(_goldAmount);
        //}


        _textUICurrentGold.SetText(value.ToString());
        ChangeTotalEarned(Score.TotalEarned);

        //_textUIEarned.SetText(Score.TotalEarned.ToString()); //?
    }
    private void ChangeTotalEarned(int value)
    {
        _textUIEarned.SetText(value.ToString());
    }
    private void ChangeHighscore(int value) 
    {
        _textUIHighScore.SetText(value.ToString());
    }

    
    private void UseAbility(AbilityType type)
    {
        switch (type)
        {
            case AbilityType.MidasHand:
                if (_activeFireType != type)
                {
                    StartCoroutine(SetAbilityToCooldown(_imageMeleeAttack, 0.3f));
                }
                ChangeFireType(AbilityType.MidasHand);
                break;

            case AbilityType.CloseCombat:
                if (_activeFireType != type)
                {
                    StartCoroutine(SetAbilityToCooldown(_imageRangeAttack, 0.3f));
                }
                ChangeFireType(AbilityType.CloseCombat);
                break;
        }
    }

    private IEnumerator SetAbilityToCooldown(Image abilityImage, float cooldown) 
    {
        for (float t = 0; t < 1; t += Time.deltaTime / cooldown)
        {
            abilityImage.color = Color.Lerp(Color.gray, Color.white, t);
            yield return null;
        }
    }
    
    private void ChangeFireType(AbilityType abilityType)
    {
        _activeFireType = abilityType;

        if (abilityType == AbilityType.MidasHand)
        {
            _imageRangeAttack.rectTransform.localScale = new Vector3(1, 1, 1);
            _imageMeleeAttack.rectTransform.localScale = new Vector3(0.8f, 0.8f, 1);
        }
        else
        {
            _imageMeleeAttack.rectTransform.localScale = new Vector3(1, 1, 1);
            _imageRangeAttack.rectTransform.localScale = new Vector3(0.8f, 0.8f, 1);
        }
    }

    private void ChangeOrder()
    {
        int newOrder = OrderManager.Order;
        if (newOrder != _currentDisplayedOrder)
        {
            _currentDisplayedOrder = newOrder;
            _imageOrder.sprite = _orderSprites[newOrder];
        }
    }

    private void SetUI(bool show)
    {
        //_orderPanel.SetActive(show);
        //_scorePanel.SetActive(show);
        //_abilitiesPanel.SetActive(show);
        _gameScreen.SetActive(show);
    }

    public void Pause()
    {
        PauseManager.Pause();

        if (_rulesScreen.activeInHierarchy)
        {
            _rulesScreen.SetActive(!_rulesScreen.activeInHierarchy);
        }
        else
        {
            _pauseScreen.SetActive(!_pauseScreen.activeInHierarchy);
        }

        SetUI(!_pauseScreen.activeInHierarchy);
    }

    public void ShowRules()
    {
        _rulesScreen.SetActive(!_rulesScreen.activeInHierarchy);
        _pauseScreen.SetActive(!_pauseScreen.activeInHierarchy);
    }

    public void ToMainMenuScene()
    {
        SceneManager.LoadScene(0);
    }

    public void CallRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
