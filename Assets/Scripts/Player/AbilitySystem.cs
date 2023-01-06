using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

public enum AbilityType { None = -1, MidasHand, CloseCombat }

public class AbilitySystem : MonoBehaviour
{
    [SerializeField] private AbilityType _standartAbility = AbilityType.None;

    [SerializeField] private List<BaseAbility> _abilities;

    private BasePlayer _player = null;

    private BaseAbility _currentAbility = null;

    public int AbilityPrice => _currentAbility.Price;

    public void Initialize(BasePlayer player)
    {
        _player = player;
        _player.OnUseAbility += UseArmament;

        InitializeAbility();
        SetStandartAbility();
    }

    private void InitializeAbility()
    {
        foreach (var a in _abilities)
            a.Initialize(this);
    }
    private void SetStandartAbility()
    {
        _currentAbility = GetAbilityByType(_standartAbility);
    }

    private BaseAbility GetAbilityByType(AbilityType type)
    {
        foreach (var ability in _abilities)
        {
            if(ability.AbilityType == type)
                return ability;
        }
        return null;
    }

    public void Deinitialize()
    {
        _currentAbility = null;
        _abilities.Clear();

        _player.OnUseAbility -= UseArmament;
        _player = null;
    }

    private void UseArmament() => _currentAbility?.Use();

    private void Update()
    {
        //if (!GameParameters.GameRunning) return;
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    _currentAbility = GetAbilityByType(AbilityType.CloseCombat);
        //    EventManager.SendAbilityChanged(_currentAbility.AbilityType);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    _currentAbility = GetAbilityByType(AbilityType.MidasHand);
        //    EventManager.SendAbilityChanged(_currentAbility.AbilityType);
        //}
    }

}
