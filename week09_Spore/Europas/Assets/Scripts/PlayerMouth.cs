using UnityEngine;

// inherited class for the player only so he can eat foods

public class PlayerMouth : MouthScript
{
    private PlayerCreature _playerCreature;

    void Start()
    {
        if (_playerCreature == null)
        {
            _playerCreature = GetComponentInParent<PlayerCreature>();
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Food foodInstance = collision.GetComponent<Food>();

        if (foodInstance != null)
        {
            DietType playerDiet = _playerCreature.GetDiet();
            FoodType foodType = foodInstance.GetFoodType();

            if (playerDiet == DietType.Carnivore && foodType == FoodType.Plant)
            {
                return;
            }

            if (playerDiet == DietType.Herbivore && foodType == FoodType.Meat)
            {
                return;
            }

            _playerCreature.TakeDamage(-foodInstance.GetNutritionValue()); // HEALING HERE **************************
            _playerCreature.AddScore(foodInstance.GetPointValue());
            foodInstance.Consume();
            return;
        }

        base.OnTriggerEnter2D(collision);
    }
}