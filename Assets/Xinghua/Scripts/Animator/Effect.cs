using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] GameObject effect;

    public static Effect Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void PlayAttackEffect(Vector3 positon)
    {
        if (effect != null)
            effect.transform.position = positon;
        effect.SetActive(true);
    }
    public void HideAttackEffect()
    {
        if (effect != null)
        {
            effect.SetActive(false);
        }

    }


}
