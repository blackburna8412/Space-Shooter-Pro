
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    [SerializeField] private GameObject _laserPrefab;

    [SerializeField] private AudioSource _audioSource;

    Animator _enemyDestroyedAnim;
    private PlayerController player;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;
    private bool _isEnemyAlive = true;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();

        if(player == null)
        {
            Debug.LogError("Player is NULL");
        }


        _enemyDestroyedAnim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        EnemenyMovement();

        if(Time.time > _canFire && _isEnemyAlive == true)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }

        }
    }

    private void EnemenyMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 8f, transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageMethod(other);
    }

    private void DamageMethod(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (player != null)
            {
                player.Damage();
            }

            _enemyDestroyedAnim.SetTrigger("IsDestroyed");
            _speed = 0;
            Destroy(GetComponent<BoxCollider2D>());
            _audioSource.Play(0);
            _isEnemyAlive = false;
            Destroy(this.gameObject, 2.8f);
        }

        if (other.gameObject.name == "Laser(Clone)")
        {
            if(player != null)
            {
                player.AddScore(10);
            }

            _enemyDestroyedAnim.SetTrigger("IsDestroyed");
            _speed = 0;
            GetComponent<BoxCollider2D>().enabled = false;
            _audioSource.Play(0);
            Destroy(this.gameObject, 2.8f);
            

            Destroy(other.gameObject);
            Debug.Log("Enemy destroyed!");
        }
    }
}
