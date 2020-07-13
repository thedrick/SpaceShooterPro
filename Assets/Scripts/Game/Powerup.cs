using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public enum PowerupType
    {
        TripleShot,
        Speed,
        Shield,
        COUNT,
    }

    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private PowerupType _type;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Random.Range(-6, 6), GameConstants.maxY, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < GameConstants.minY)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                switch (_type)
                {
                    case PowerupType.TripleShot:
                        player.EnableTripleShot();
                        break;
                    case PowerupType.Speed:
                        player.EnableSpeedUp();
                        break;
                    case PowerupType.Shield:
                        player.EnableShield();
                        break;
                }
            }
            Destroy(gameObject);
        }
    }
}
