using System.Linq;
using UnityEngine;

public static class Utils
{
    public static void DebugLog(string message)
    {
        Debug.Log($"{Time.time} {message}");
    }

    // Return random position within the playfield.
    public static Vector3 GetRandomSpawnPosition()
    {
        float playfieldSize_x = 4.7f;
        float playfieldSize_y = 4.6f;
        float scale_x = 5.5f;
        float scale_y = 3.6f;

        Vector3 randomPosition = new Vector3(); ;
        bool isColliding = true;

        // Keep generating a new random position until it doesn't intersect with "collidable" objects
        while (isColliding)
        {
            randomPosition = new Vector3(
                Random.Range(-playfieldSize_x, playfieldSize_x) * scale_x,
                Random.Range(-playfieldSize_y, playfieldSize_y) * scale_y,
                0) * 0.9f;

            // Check for collisions with objects tagged as "collidable"
            Collider2D[] colliders = Physics2D.OverlapCircleAll(randomPosition, 1.0f); // Adjust the radius as needed

            // Check if any of the colliders are tagged as "collidable"
            isColliding = colliders.Any(collider => collider.CompareTag("Collidables"));
        }

        return randomPosition;
    }

    public static float GetPlayfieldSize()
    {
        return 4;
    }

    public static string getRandomName()
    {
        string[] names = { "Eddy", "Freddy", "Teddy", "Meddy", "Haddy", "Paddy", "Buddy", "Neddy", "Kevin", "Bob", "Frank", "Wayne" };

        return names[Random.Range(0, names.Length)] + Random.Range(1, 100);
    }
}
