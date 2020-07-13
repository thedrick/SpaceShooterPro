using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _speedPowerupPrefab;
    [SerializeField]
    private GameObject _shieldPowerupPrefab;
    private float _enemySpawnRate = 3.0f;

    // MARK: Public

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    private bool _stopSpawning = false;

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerup());
    }

    // Spawn game objects every 5 seconds
    IEnumerator SpawnEnemy()
    {
        while (!_stopSpawning)
        {
            GameObject enemy = Instantiate(_enemyPrefab);
            enemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_enemySpawnRate);
        }
    }

    IEnumerator SpawnPowerup()
    {
        while (!_stopSpawning)
        {
            yield return new WaitForSeconds(Random.Range(4.0f, 8.0f));
            Powerup.PowerupType newPowerupType = RandomPowerup();
            switch (newPowerupType)
            {
                case Powerup.PowerupType.TripleShot:
                    Instantiate(_tripleShotPrefab);
                    break;
                case Powerup.PowerupType.Speed:
                    Instantiate(_speedPowerupPrefab);
                    break;
                case Powerup.PowerupType.Shield:
                    Instantiate(_shieldPowerupPrefab);
                    break;
            }
        }
    }

    Powerup.PowerupType RandomPowerup()
    {
        return (Powerup.PowerupType)Random.Range(0, (int)Powerup.PowerupType.COUNT);
    }
}
