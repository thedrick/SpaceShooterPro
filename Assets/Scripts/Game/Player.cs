using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // MARK: SerializeField

    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _speedUpSpeed = 10.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.3f;
    [SerializeField]
    private int _lives = 3;

    // MARK: Private

    private float _maxY = 0;
    private float _minY = -3.8f;
    private Vector3 _laserOffset = new Vector3(0, 1.05f, 0);
    private float _nextFire = 0.0f;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;

    // Powerups

    private bool _isTripleShotActive = false;
    private bool _isSpeedUpActive = false;
    private bool _isShieldActive = false;

    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    private int _score;

    private IEnumerator _tripleShotDisableCoroutine;
    private IEnumerator _speedUpDisableCoroutine;

    // MARK: Public

    public void Damage()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
        } 
        else
        {
            _lives--;
        }

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            _uiManager.GameOver();
            Destroy(gameObject);
        }
    }

    public void EnableTripleShot()
    {
        if (_tripleShotDisableCoroutine != null)
        {
            StopCoroutine(_tripleShotDisableCoroutine);
        }
        _isTripleShotActive = true;
        _tripleShotDisableCoroutine = DisableTripleShotAfterDelay();
        StartCoroutine(_tripleShotDisableCoroutine);
    }

    public void EnableSpeedUp()
    {
        if (_speedUpDisableCoroutine != null)
        {
            StopCoroutine(_speedUpDisableCoroutine);
        }
        _isSpeedUpActive = true;
        _speedUpDisableCoroutine = DisableSpeedUpAfterDelay();
        StartCoroutine(_speedUpDisableCoroutine);
    }

    public void EnableShield()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.SetScore(_score);
    }

    private void Awake()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_spawnManager == null)
        {
            Debug.LogAssertion("Spawn Manager could not be found!");
        }
        if (_uiManager == null)
        {
            Debug.LogAssertion("UIManager could not be found!");
        }
    }

    void Update()
    {
        CalculateMovement();
        FireLaser();
    }

    private void FireLaser()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
        {
            _nextFire = Time.time + _fireRate;

            GameObject laser = _isTripleShotActive ? _tripleShotPrefab : _laserPrefab;
            Instantiate(laser, transform.position + _laserOffset, Quaternion.identity);
        }
    }

    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput);
        transform.Translate(direction * currentSpeed() * Time.deltaTime);

        // Vertical movement - restrict player from 0 -> -3.8 on y axis
        if (transform.position.y >= _maxY)
        {
            transform.position = new Vector3(transform.position.x, _maxY, transform.position.z);
        }
        else if (transform.position.y <= _minY)
        {
            transform.position = new Vector3(transform.position.x, _minY, transform.position.z);
        }

        // Horizontal movement - restrict player from -11 -> 11 on x axis
        if (transform.position.x > GameConstants.maxX)
        {
            transform.position = new Vector3(GameConstants.minX, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < GameConstants.minX)
        {
            transform.position = new Vector3(GameConstants.maxX, transform.position.y, transform.position.z);
        }
    }

    private float currentSpeed()
    {
        return _isSpeedUpActive ? _speedUpSpeed : _speed;
    }

    IEnumerator DisableTripleShotAfterDelay()
    {
        yield return new WaitForSeconds(8.0f);
        _isTripleShotActive = false;
    }

    IEnumerator DisableSpeedUpAfterDelay()
    {
        yield return new WaitForSeconds(8.0f);
        _isSpeedUpActive = false;
    }
}
