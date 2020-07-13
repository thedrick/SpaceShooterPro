using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    float _speed = 4.0f;

    private Player _player;

    // MARK: Lifecycle

    void Awake()
    {
        transform.position = RandomStartPosition();
    }

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < GameConstants.minY)
        {
            transform.position = RandomStartPosition();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HandlePlayerCollision(other);
        } 
        else if (other.CompareTag("Laser"))
        {
            HandleLaserCollision(other);
        }
    }

    // MARK: Private

    private Vector3 RandomStartPosition()
    {
        return new Vector3(Random.Range(-9, 9), GameConstants.maxY, 0);
    }

    private void HandlePlayerCollision(Collider2D other)
    {
        Player player = other.transform.GetComponent<Player>();
        if (player != null)
        {
            player.Damage();
        }
        Destroy(this.gameObject);
    }

    private void HandleLaserCollision(Collider2D other)
    {
        UpdateScore();
        Destroy(other.gameObject);
        Destroy(this.gameObject);
    }

    private void UpdateScore()
    {
        if (_player != null)
        {
            _player.AddScore(10);
        }
    }
}
