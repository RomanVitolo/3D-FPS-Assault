using System;
using UnityEngine;

namespace AgentLogic
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private AmmoAttributesSO _ammoAttributes;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _gunBarrel;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }           
            
        public void Shoot()
        {  
            GameObject bullet = Instantiate(gameObject, _gunBarrel.position,
                _gunBarrel.rotation);      
           
            Vector3 shootingDirection = (_target.position - _gunBarrel.position).normalized;
            _rb.velocity = shootingDirection * _ammoAttributes.BulletSpeed;   
            
            Destroy(bullet, _ammoAttributes.BulletLifeTime);
        }
    }
}