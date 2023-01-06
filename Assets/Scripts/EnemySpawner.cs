using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Space]
    [SerializeField] private BasePlayer _player;
    [Space]
    [SerializeField] private GameObject _enemyPoolsParant;
    [SerializeField] private int _poolSize = 50;

    [Space]
    [SerializeField] private float _spawnInterval = 3f, _stepTime = 0.1f;
    [SerializeField] private int _stepGold = 200;
    
    [Space]
    [SerializeField] private GameObject[] _spawnAreas;

    private Enemy[] _enemyPrefabs;
    private ObjectPool<Enemy>[] _enemyPools;
    private Coroutine _spawnRandomEnemy;

    private List<int> _allChances;
    private int _chancesSum;
    private float _spawnCooldown;

    private static EnemySpawner _instance;

    public static EnemySpawner GetInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        _instance = this;
    }

    private void OnEnable()
    {
        EventManager.OnEnemyKilled.AddListener(ReturnEnemyToPool);
        EventManager.OnGoldChanged.AddListener(CalculateSpawnInterval);
    }

    private void Start()
    {
        StartSpawning();
    }

    private void CalculateSpawnInterval(int value)
    {
        _spawnCooldown = _spawnInterval - (Score.TotalEarned / _stepGold) * _stepTime;
        if (_spawnCooldown <= 0.1f)
        {
            _spawnCooldown = 0.2f;
        }
    }

    public void StopSpawning()
    {
        StopCoroutine(_spawnRandomEnemy);
    }

    public void StartSpawning()
    {
        PreparePools();

        _spawnRandomEnemy = StartCoroutine(SpawnRandomEnemyFromPool());
    }

    private void PreparePools()
    {
        _enemyPrefabs = Resources.LoadAll<Enemy>("Prefabs/EnemyPrefabs");

        CreatePools(_enemyPrefabs, _poolSize);
    }

    private void CreatePools(Enemy[] enemyPrefabs, int poolSize)
    {
        _enemyPools = new ObjectPool<Enemy>[enemyPrefabs.Length];

        _allChances = new List<int>();
        _chancesSum = 0;

        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            _enemyPools[(int)enemyPrefabs[i].Type] = new ObjectPool<Enemy>(createFunc: () => new Enemy(), actionOnGet: (obj) => obj.gameObject.SetActive(true), actionOnRelease: (obj) => obj.gameObject.SetActive(false), actionOnDestroy: (obj) => Destroy(obj), false, defaultCapacity: poolSize);

            AddChances(enemyPrefabs[i].Weight);
            FillPool(_enemyPools[(int)enemyPrefabs[i].Type], enemyPrefabs[i], poolSize);
        }
    }

    private void AddChances(int weight)
    {
        _chancesSum += weight;
        _allChances.Add(weight);
    }

    private void FillPool(ObjectPool<Enemy> pool, Enemy prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Enemy enemy = Instantiate(prefab);
            enemy.transform.SetParent(_enemyPoolsParant.transform);
            pool.Release(enemy);
        }
    }

    private IEnumerator SpawnRandomEnemyFromPool()
    {
        while (GameParameters.GameRunning)
        {
            SpawnPrefabFromPool(GetWeightRandomPrefabFromPool(), GetRandomSpawnPositionInZones()); //GetRandomSpawnPosition(_arenaWidth, _arenaHeight));
            yield return new WaitForSeconds(_spawnCooldown);
        }
    }

    private void SpawnPrefabFromPool(ObjectPool<Enemy> pool, Vector3 position)
    {
        if (pool.CountInactive > 0)
        {
            Enemy enemy = pool.Get();
            //enemy.transform.position = position;
            enemy.SetPosition(position);
            enemy.SetTarget(_player);
            enemy.GetComponent<CharacterController>().enabled = true;
        }
    }

    private ObjectPool<Enemy> GetWeightRandomPrefabFromPool()
    {
        int value = Random.Range(0, _chancesSum);
        int sum = 0;

        for (int i = 0; i < _allChances.Count; i++)
        {
            sum += _allChances[i];
            if (value < sum)
            {
                return _enemyPools[i];
            }
        }

        return _enemyPools[_enemyPools.Length - 1];
    }

    private Vector3 GetRandomSpawnPositionInZones()
    {
        Transform zoneTransform = _spawnAreas[Random.Range(0, _spawnAreas.Length)].transform;
        Vector3 position = new Vector3();
        position.x = Random.Range(0, zoneTransform.localScale.x) + zoneTransform.position.x - zoneTransform.localScale.x / 2;
        position.z = Random.Range(0, zoneTransform.localScale.z) + zoneTransform.position.z - zoneTransform.localScale.z / 2;
        position.y = 0;

        Collider[] colliders = Physics.OverlapSphere(position, 0.5f);
  
        if (colliders.Length > 1)  
        {
            return GetRandomSpawnPositionInZones();
        }
        else
        {
            return position;
        }  
    }

    private void ReturnEnemyToPool(Enemy enemy)
    {
        enemy.GetComponent<CharacterController>().enabled = false;
        _enemyPools[(int)enemy.Type].Release(enemy);
    }
}


