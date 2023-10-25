using UnityEngine;
using System.Collections;
using Fusion;

public class FoodHandler : MonoBehaviour
{

	NetworkPlayerListHandler playerListHandler;

    void Awake()
    {
		playerListHandler = FindObjectOfType<NetworkPlayerListHandler>();
    }

	// Update is called once per frame
	void Update()
	{
		foreach (NetworkPlayer np in playerListHandler.Players)
		{
			float distance = Vector3.Distance(transform.position, np.transform.position);

			if (distance < 1f + (np.GetComponent<MovementHandler>().NetSize / 75))
			{
				if (tag.Equals("Food") || (tag.Equals("HealthPack") && np.NetHealth < np.NetMaxHealth))
                    transform.position = Vector3.Lerp(transform.position, np.transform.position, Time.deltaTime * 10f);
            }
		}
	}


}

