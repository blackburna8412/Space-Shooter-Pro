using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 3f;
    [SerializeField] GameObject _explosionPrefab;
    private SpawnManager _spawnManager;
    [SerializeField] private GameObject _explosionContainer;



    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            explosion.transform.parent = _explosionContainer.transform;
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject, .15f);

        }
    }
}
