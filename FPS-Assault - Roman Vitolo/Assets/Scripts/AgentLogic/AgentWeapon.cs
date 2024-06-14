using UnityEngine;    

namespace AgentLogic
{   
    public class AgentWeapon : MonoBehaviour
    {
        [SerializeField] private WeaponStatsSO _weaponStats;
        [SerializeField] private ParticleSystem _muzzleFlash;
        [SerializeField] private AudioSource _weaponAudioFX;
        [SerializeField] private WeaponSoundSO _weaponSounds;

        public float WeaponFireRate() => _weaponStats.FireRate;

        private void Awake()
        {
            _muzzleFlash = GetComponentInChildren<ParticleSystem>();
            _weaponAudioFX = GetComponentInChildren<AudioSource>();

            _weaponStats.CurrentAmmo = _weaponStats.InitialWeaponAmmo;
        }

        public bool CheckForEnoughAmmo()
        {
            if (_weaponStats.CurrentAmmo > 0) return true;
            return false;
        }     
        
        public void Shoot()
        {
            if (_weaponAudioFX.clip != _weaponSounds.ShootClip)  _weaponAudioFX.clip = _weaponSounds.ShootClip;  
            else  
                _weaponAudioFX.Play();        
            
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, _weaponStats.WeaponShootRange, _weaponStats.KillLayer))
            {
                _muzzleFlash.Play();
                AgentHealth agentHealth = hitInfo.transform.GetComponent<AgentHealth>();
                if (agentHealth != null)
                {
                    agentHealth.TakeDamage(_weaponStats.WeaponDamage);
                    //Debug.Log(agentHealth.GetCurrentLife());  
                }
                //Debug.Log("Hit Entity" + hitInfo.transform.name);
            }   
            _weaponStats.CurrentAmmo--;
        }   

        public void LoadReloadFX()
        {
            _weaponAudioFX.clip = _weaponSounds.ReloadSound;
            _weaponAudioFX.Play();
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * _weaponStats.WeaponShootRange);
        }   
        
    }
}