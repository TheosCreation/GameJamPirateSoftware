using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction : byte
{
    NONE = 0b0000, // 0
    UP = 0b0001,  // 1
    RIGHT = 0b0010,  // 2
    DOWN = 0b0100,  // 4
    LEFT = 0b1000   // 8
}
public struct node
{
    Direction direction;
    
}
public class TotalWaveCollapse : MonoBehaviour
{
    
}
