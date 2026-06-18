using UnityEngine;
using TMPro; 


// yum yums
public enum FoodType
{
    Meat,
    Plant
}

public class Food : MonoBehaviour
{
    [SerializeField] private FoodType _foodType = FoodType.Meat;
    [SerializeField] private float _nutritionValue = 6f;
    [SerializeField] private int _pointValue = 2;
    [SerializeField] private GameObject _floatingTextPrefab;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        if (_foodType == FoodType.Plant)
        {
            _nutritionValue /= 2;
            _pointValue /= 2;
        }
    }

    public FoodType GetFoodType() => _foodType;
    public float GetNutritionValue() => _nutritionValue;
    public int GetPointValue() => _pointValue;

    public void Consume()
    {
        if (_floatingTextPrefab != null)
        {
            GameObject textObj = Instantiate(_floatingTextPrefab, transform.position, Quaternion.identity);
            TextMeshProUGUI textMesh = textObj.GetComponentInChildren<TextMeshProUGUI>();

            if (textMesh != null)
            {
                if (_foodType == FoodType.Plant)
                {
                    textMesh.text = "+1";
                    textMesh.color = Color.green;
                }
                else if (_foodType == FoodType.Meat)
                {
                    textMesh.text = "+2";
                    textMesh.color = new Color(1f, 0.4f, 0.6f); // Reddish-pink text
                }
            }
        }

        Destroy(gameObject);
    }
}