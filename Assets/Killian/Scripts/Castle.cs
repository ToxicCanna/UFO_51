using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Castle : MonoBehaviour
{
    public int maxHP;
    public int castleHP;

    [SerializeField] private Image _image;
    [SerializeField] private Gradient _gradient;
    private float _target;

    private void Awake()
    {
        castleHP = maxHP;
        CheckHealthBarGradientAmount();
    }

    public void TakeDamage(int damage)
    {
        castleHP -= damage;
        _target = castleHP/maxHP;
        _image.fillAmount = _target;
        if (castleHP <= 0)
        {
            LoseGame();
        }
        CheckHealthBarGradientAmount();
    }
    private void CheckHealthBarGradientAmount()
    {
        _image.color = _gradient.Evaluate(_image.fillAmount);
    }
    public virtual void LoseGame()
    {

    }
}
