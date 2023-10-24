using UnityEngine;
using System.Collections;
using Fusion;

public class Pickup : MonoBehaviour
{
	NetworkPlayerListHandler playerListHandler;

    void Awake()
    {
		playerListHandler = GameObject.FindObjectOfType<NetworkPlayerListHandler>();
    }

	// Update is called once per frame
	void Update()
	{
		foreach (NetworkPlayer np in playerListHandler.Players)
		{
			float distance = Vector3.Distance(transform.position, np.transform.position);

            if (distance < 1f)
			{
				if (tag.Equals("Food") || (tag.Equals("HealthPack") && np.NetHealth < np.NetMaxHealth))
                    transform.position = Vector3.Lerp(transform.position, np.transform.position, Time.deltaTime * 5f);
            }
		}
	}
}

