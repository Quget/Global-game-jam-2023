using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{

    [SerializeField]
    private GameObject[] map = null;

    [SerializeField]
    private CloudyMovement cloudyMovementPrefab = null;

    [SerializeField]
    private Sprite[] cloudSprites = null;

    [SerializeField]
    private int maxClouds = 10;

    private List<CloudyMovement> cloudyMovements = new List<CloudyMovement>();

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
                map[i].transform.position = new Vector3(map[i].transform.position.x + 150, map[i].transform.position.y, 0);
            }

            if (transformToFollow.position.x < map[i].transform.position.x - 75)
            {
                map[i].transform.position = new Vector3(map[i].transform.position.x - 150, map[i].transform.position.y, 0);
            }

            if (transformToFollow.position.y > map[i].transform.position.y + 75)
            {
                map[i].transform.position = new Vector3(map[i].transform.position.x , map[i].transform.position.y + 150, 0);
            }
            if (transformToFollow.position.y < map[i].transform.position.y - 75)
            {
                map[i].transform.position = new Vector3(map[i].transform.position.x , map[i].transform.position.y - 150, 0);
            }
        }

        if (cloudyMovements.Count < maxClouds)
        {
            CreateCloud();
        }
    }

    private void CreateCloud()
    {
        CloudyMovement cloudyMovement = GameController.Instantiate(cloudyMovementPrefab);
        cloudyMovement.SetUp(cloudSprites[Random.Range(0, cloudSprites.Length)], (thisCloudyMovement) =>
        {
            Destroy(thisCloudyMovement.gameObject);
            cloudyMovements.Remove(thisCloudyMovement);
        });

        //Vector3 cloudPosition = new Vector3(transformToFollow.x )
        float radius = Random.Range(30f, 50f);


        Vector2 cloudPosition = Random.insideUnitCircle.normalized * radius + (Vector2)transformToFollow.position;
        cloudyMovement.transform.position = cloudPosition;
        cloudyMovements.Add(cloudyMovement);
    }
}
