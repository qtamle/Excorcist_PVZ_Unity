using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool hasPlant;

    public bool HasPlant()
    {
        return hasPlant;
    }

    public void SetHasPlant(bool value)
    {
        hasPlant = value;
    }

    // Phương thức này sẽ được gọi khi cây bị xóa
    public void PlantRemoved()
    {
        hasPlant = false;
    }
}
