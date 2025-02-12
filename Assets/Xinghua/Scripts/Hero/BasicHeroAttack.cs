using UnityEngine;

public class BasicHeroAttack : MonoBehaviour
{
    private HeroPath heroPath;
    private void Start()
    {
        heroPath = GetComponent<HeroPath>();
    }
    private void CheckArriveCastle()
    {
        if(heroPath !=null && heroPath.heroType == "Red" && this.transform.position == new Vector3(8, 4,0))
        {
            Vector2Int pos = new Vector2Int((int)this.transform.position.x, (int)this.transform.position.y);
            if(GridManager.Instance.GetGridOccupiedHeroType(pos) == "Blue")//check if blue enemy there
            {
                Debug.Log("kill enemy first");
            }

        }
    }

 
}
