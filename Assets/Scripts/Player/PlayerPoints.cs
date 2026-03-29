using UnityEngine;

public class PlayerPoints : MonoBehaviour
{
    [Header("Points")]
    [SerializeField] private int maxPoints = 100;
    [SerializeField] private int currentPoints = 30;

    void Start()
    {
        currentPoints = 0;
    }

   
    public void AddPoints(int amount)
    {
        currentPoints += amount;

        if (currentPoints > maxPoints)
            currentPoints = maxPoints;

        Debug.Log("Points: " + currentPoints);
    }

    
    public bool UsePoints(int amount)
    {
        if (currentPoints < amount)
        {
            Debug.Log("Sem pontos suficientes");
            return false;
        }

        currentPoints -= amount;

        Debug.Log("Gastou pontos. Restante: " + currentPoints);

        return true;
    }

   
    public int GetPoints()
    {
        return currentPoints;
    }
}