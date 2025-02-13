using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Security.Cryptography;

public class Castle : MonoBehaviour
{
    public float maxHP;
    public float castleHP;

    [SerializeField] private Image _HPBar;
    [SerializeField] private Gradient _gradient;
    public TMP_Text tMPro;
    private float _target;

    private void Awake()
    {
        castleHP = maxHP;
        _target = castleHP / maxHP;
        _HPBar.fillAmount = _target;
        CheckHealthBarGradientAmount();
    }
    //debug xinghua
    private void Start()
    {
      
        Debug.Log("start maxHP:"+ this.maxHP);
        Debug.Log("start castleHP:" + this.castleHP);
    }
    //Debug end
    public void TakeDamage(int damage)
    {
        castleHP -= damage;
        Debug.Log($"castle HP: {castleHP}");

        _target = castleHP/maxHP;
        Debug.Log($"HealthBar fill: {_target}");

        _HPBar.fillAmount = _target;
        CheckHealthBarGradientAmount();
        if (castleHP <= 0)
        {
            LoseGame();
        }
    }
    private void CheckHealthBarGradientAmount()
    {
        _HPBar.color = _gradient.Evaluate(_HPBar.fillAmount);
    }
    public virtual void LoseGame()
    {

    }
}
