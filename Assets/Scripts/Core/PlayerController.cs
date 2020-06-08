using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed = 3.5f;
        [SerializeField] private float _speedMultiplier = 2;
        [SerializeField] private float _fireRate = .5f;
        [SerializeField] private float _boosterSpeed = 1f;
        private float _canFire = -1f;

        [SerializeField] private GameObject _laserPrefab = null;
        [SerializeField] private GameObject _tripleShotPrefab = null;
        [SerializeField] private GameObject _shieldObject = null;
        [SerializeField] private GameObject[] _damagedThrusters;


        [SerializeField] private SpawnManager _spawnManager;
        [SerializeField] private UIManager _uiManager;

        [SerializeField] private AudioClip _laserSoundClip;
        [SerializeField] private AudioSource _audioSource;

        private bool isTripleShotActive = false;
        private bool isSpeedBoostActive = false;
        public bool isShieldActive = false;

        [SerializeField] private int _playerHealth = 3;
        [SerializeField] public int _score = 0;
        
        private Vector3 offsetLaser = new Vector3(0, 1.05f, 0);

        // Start is called before the first frame update
        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
            _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();

            if(_spawnManager == null)
            {
                Debug.LogError("The Spawn Manager is NULL.");
            }

            if(_uiManager == null)
            {
                Debug.LogError("UI Manager is NULL");
            }

            if(_audioSource == null)
            {
                Debug.LogError("AudioSource on the Player is NULL");
            }
            else
            {
                _audioSource.clip = _laserSoundClip;
            }
        }

        // Update is called once per frame
        void Update()
        {
            CalculateMovement();
            if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
            {
                FireLaser();
            }

            CheckPlayerDamage();
        }

        private void FireLaser()
        {
            _canFire = Time.time + _fireRate;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(isTripleShotActive == true)
                {
                    Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(_laserPrefab, transform.position + offsetLaser, Quaternion.identity);
                }

                _audioSource.Play();
            }

        }

        private void CalculateMovement()
    {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(hInput, vInput, 0);

        transform.Translate(direction * _movementSpeed * _boosterSpeed * Time.deltaTime);
        
        BoosterSpeed();

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);
        if (transform.position.x > 11.3f)
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        else if (transform.position.x < -11.3f)
            transform.position = new Vector3(11.3f, transform.position.y, 0);
    }

    private void BoosterSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _boosterSpeed = 2f;
        }
        else
        {
            _boosterSpeed = 1f;
        }
    }

    public void Damage()
        {
            if (isShieldActive == true)
            {
                isShieldActive = false;
                _shieldObject.SetActive(false);
                return;
            }

                _playerHealth--;

            _uiManager.UpdateLives(_playerHealth);

                if (_playerHealth <= 0)
                {
                    _spawnManager.StopSpawning();
                    Destroy(this.gameObject);
                }
        }

        public void TripleShotActive()
        {
            isTripleShotActive = true;
            StartCoroutine(TripleShotPowerDownRoutine());
        }

        IEnumerator TripleShotPowerDownRoutine()
        {
            yield return new WaitForSeconds(5);
            isTripleShotActive = false;
        }

        public void SpeedBoostActive()
        {
            if(isSpeedBoostActive != true)
            {
                isSpeedBoostActive = true;
                _movementSpeed *= _speedMultiplier;
                StartCoroutine(SpeedBoostPowerDownRoutine());
            }
        }

        IEnumerator SpeedBoostPowerDownRoutine()
        {
            yield return new WaitForSeconds(5);
            isSpeedBoostActive = false;
            _movementSpeed /= _speedMultiplier;
        }

        public void ShieldActive()
        {
            isShieldActive = true;
            _shieldObject.SetActive(true);
        }

        //methos to add 10 to score
        //Communicate with the UI to update the score
        public void AddScore(int points)
        {
            _score += points;
            _uiManager.UpdateScore(_score);
        }

        public void CheckPlayerDamage()
        {
            if(_playerHealth == 2)
            {
                _damagedThrusters[0].SetActive(true);
            }
            if (_playerHealth == 1)
            {
                _damagedThrusters[1].SetActive(true);
            }
        }
    }