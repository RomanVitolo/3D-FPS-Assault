using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Node : MonoBehaviour
{
    public List<Node> Neightbourds = new List<Node>();
    public bool hasTrap;
    Material mat;
    private void Start()
    {
        mat = GetComponent<Renderer>().material;   
    }
    private void Update()
    {
        if (hasTrap)
            mat.color = Color.red;
        else
            mat.color = Color.white;
    }                                    
}