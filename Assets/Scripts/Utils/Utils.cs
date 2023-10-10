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
        return new Vector3(Random.Range(-GetPlayfieldSize() / 2f, GetPlayfieldSize() / 2f), Random.Range(-GetPlayfieldSize() / 2f, GetPlayfieldSize() / 2f), 0) * 0.9f;
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
