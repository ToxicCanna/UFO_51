using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroSelect : MonoBehaviour
{
    //when in my turn,I need select the hero to move in my already hero array
    [SerializeField] public List<HeroData> heros;//this list should been add when player buy hero
    private int currentSelectIndex;
    [SerializeField] private GridIndicator gridIndicator;
    private void Start()
    {
        if (heros == null || heros.Count == 0)
        {
            Debug.LogWarning("No heroes available in the list!");
            return;
        }
        GetSelectHeroIndex(0);
    }

    public void SwitchHero()
    {
        Debug.Log("hero in the scene"+heros.Count);
        if (heros == null || heros.Count == 0) return;
        currentSelectIndex = (currentSelectIndex + 1) % heros.Count;
        GetSelectHeroIndex(currentSelectIndex);
       
    }

    private void GetSelectHeroIndex(int index)
    {
        if (index < 0 || index >= heros.Count) return;

        HeroData selectedHero = heros[index];
        Debug.Log("current select" + heros[currentSelectIndex].name);
    }

    public Vector3 GetSelectedHeroPosition()
    {
        if (heros == null || heros.Count == 0) return Vector3.zero;
        return heros[currentSelectIndex].transform.position;
    }
}
