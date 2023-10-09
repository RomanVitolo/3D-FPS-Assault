using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePivot : MonoBehaviour
{
    [SerializeField] private float adjustPivot;
    private void Start()
    {   
        transform.localPosition = new Vector3(0.172f, -0.013f, 0.133f);      
    }
}
