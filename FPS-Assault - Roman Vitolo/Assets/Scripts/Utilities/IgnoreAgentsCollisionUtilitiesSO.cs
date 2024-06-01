using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create IgnoreAgentsCollisionUtilities", fileName = "IgnoreAgentsCollisionUtilities", order = 0)]
public class IgnoreAgentsCollisionUtilitiesSO : ScriptableObject
{    
    [field: SerializeField] public CharacterController[] TeamColliders { get;  set; }

    private Collider teamColliders;

    public void FindColliders()
    {
        foreach (var teamCollider in TeamColliders)
        {
            teamColliders = teamCollider.GetComponent<Collider>();
        }
    }

    public void IgnoreCollision(CharacterController ownCollider)
    {
        Collider myCollider = ownCollider.GetComponent<Collider>();      

        if (myCollider != null && teamColliders != null)
        {
            Physics.IgnoreCollision(myCollider, teamColliders);
        }
    }
}
