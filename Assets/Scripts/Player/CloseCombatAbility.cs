using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseCombatAbility : BaseAbility
{
    [SerializeField] private GameObject _CombatAtackZone;
    public override void Use()
    {
        Debug.Log($"CloseCombatAbility.Use");
        //включить триггер
        _CombatAtackZone.SetActive(true);
    }
}
