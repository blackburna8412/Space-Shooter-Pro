
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Laser : MonoBehaviour
    {
        [SerializeField] float _speed = 3f;
        private bool _isEnemyLaser = false;
        public bool _isEnemyAlive = true;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(_isEnemyLaser == false)
            {
                LaserMoveUp();
            }
            else if (_isEnemyLaser == true)
            {
                LaserMoveDown();
            }
        }

        private void LaserMoveDown()
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);

            if (transform.position.y <= -8f)
            {
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }
                Destroy(this.gameObject);
            }
        }

        private void LaserMoveUp()
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);

            if (transform.position.y >= 8f)
            {
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }
                Destroy(this.gameObject);
            }
        }

        public void AssignEnemyLaser()
        {
            _isEnemyLaser = true;
        }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "Player" && _isEnemyLaser == true)
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                player.Damage();
            }
        }
    }
}
