using System.Collections.Generic;
using UnityEngine;

public class TwoSidesHero : MonoBehaviour
{
   [SerializeField] private List<GameObject> heros= new List<GameObject>();
    public List<GameObject> GetHeros() 
    { 
        return heros;
    }
}
