using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{

    [SerializeField]
    private GameObject[] map = null;

    private Transform transformToFollow = null;
    public void SetUp(Transform transformToFollow)
    {
        this.transformToFollow = transformToFollow;
    }

    private void Update()
    {
        if (transformToFollow == null)
            return;

        for (int i = 0; i < map.Length; i++)
        {
            if (transformToFollow.position.x > map[i].transform.position.x + 75)
            {
                Debug.Log("Move x plus");
                map[i].transform.position = new Vector3(map[i].transform.position.x + 150, map[i].transform.position.y, 0);
            }

            if (transformToFollow.position.x < map[i].transform.position.x - 75)
            {
                Debug.Log("Move x min");
                map[i].transform.position = new Vector3(map[i].transform.position.x - 150, map[i].transform.position.y, 0);
            }

            if (transformToFollow.position.y > map[i].transform.position.y + 75)
            {
                Debug.Log("Move y plus");
                map[i].transform.position = new Vector3(map[i].transform.position.x , map[i].transform.position.y + 150, 0);
            }
            if (transformToFollow.position.y < map[i].transform.position.y - 75)
            {
                Debug.Log("Move y min");
                map[i].transform.position = new Vector3(map[i].transform.position.x , map[i].transform.position.y - 150, 0);
            }
        }
    }
}
