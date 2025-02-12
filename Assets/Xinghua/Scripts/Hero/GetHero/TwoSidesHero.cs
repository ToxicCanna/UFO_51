using System.Collections.Generic;
using UnityEngine;

public class TwoSidesHero : MonoBehaviour
{
    [SerializeField] private List<GameObject> startHeros = new List<GameObject>();//red sice
    public List<GameObject> GetStartHeros()
    {
        return startHeros;
    }
}
