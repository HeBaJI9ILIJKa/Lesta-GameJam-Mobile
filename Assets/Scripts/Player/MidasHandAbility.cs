using UnityEngine;

public class MidasHandAbility : BaseAbility
{
    [SerializeField] private Transform _armaLaunchPosition;
    [SerializeField] private BaseArmaProjectile _armaProjectilePrefab;

    public override void Use()
    {
        var prjectile = Instantiate(_armaProjectilePrefab, _armaLaunchPosition.position, _armaLaunchPosition.rotation);
            prjectile.Launch();
    }
}
 