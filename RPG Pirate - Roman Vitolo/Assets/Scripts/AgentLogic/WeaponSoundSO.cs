using UnityEngine;

namespace AgentLogic
{
    [CreateAssetMenu(menuName = "Sounds/Weapon Sounds Configuration", fileName = "New Sounds")]
    public class WeaponSoundSO : ScriptableObject
    {
        [field: SerializeField] public AudioClip ShootClip { get; private set; } 
        [field: SerializeField] public AudioClip ReloadSound { get; private set; } 
    }
}