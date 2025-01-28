using UnityEngine;

[CreateAssetMenu(fileName = "HeroMovementRule", menuName = "Hero/MovementRule")]
public class HeroMovementRule : ScriptableObject
{
    
    public int upSteps = 0; 
    public int downSteps = 0; 
    public int leftSteps = 0; 
    public int rightSteps = 0; 
    public int diagonalSteps = 0; 
}