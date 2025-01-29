using System.Collections.Generic;
using UnityEngine;

public class TwoSidesHero : MonoBehaviour
{
   [SerializeField] private List<GameObject> redSideheros = new List<GameObject>();//red sice
    [SerializeField] private List<GameObject> blueSideheros = new List<GameObject>();
    public List<GameObject> GetHerosRed() 
    { 
        return redSideheros;
    }
    public List<GameObject> GetHerosBlue()
    {
        return blueSideheros;
    }
}
