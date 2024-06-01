using System.Collections.Generic;
using UnityEngine;        

public class Node : MonoBehaviour
{
    public List<Node> Neightbourds = new List<Node>();
    [field: SerializeField] public bool CanBeInitialNode { get; private set; }
    [field: SerializeField] public bool CanBeEndNode { get; private set; }
}