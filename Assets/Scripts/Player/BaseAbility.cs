using UnityEngine;

public abstract class BaseAbility : MonoBehaviour
{
    [SerializeField] protected AbilityType _abilityType = AbilityType.None;
    [SerializeField] protected int _price;
    public AbilityType AbilityType => _abilityType;
    public int Price => _price;

    protected AbilitySystem _system = null;

    public virtual void Initialize(AbilitySystem system)
    {
        _system = system;
    }

    public abstract void Use();
}
 