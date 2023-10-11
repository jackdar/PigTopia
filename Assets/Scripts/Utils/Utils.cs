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
        float playfieldSize = GetPlayfieldSize();
        float scale_x = 5.5f;
        float scale_y = 3.6f;
        return new Vector3(Random.Range(-playfieldSize, playfieldSize) * scale_x, Random.Range(-playfieldSize, playfieldSize) * scale_y, 0) * 0.9f;
    }

    public static float GetPlayfieldSize()
    {
        return 5;
    }

    public static string getRandomName()
    {
        string[] names = { "Eddy", "Freddy", "Teddy", "Meddy", "Haddy", "Paddy", "Buddy", "Neddy", "Kevin", "Bob", "Frank", "Wayne" };

        return names[Random.Range(0, names.Length)] + Random.Range(1, 100);
    }
}
