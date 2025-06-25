using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyStatus : MonoBehaviour
{
    [Header("Required")]
    public bool isStar;
    public bool isPlanet;
    public bool isJunk;
    public bool obtained;
    [Header("Junk Only")]
    public float junkValue;
    public string junkName; 
}
