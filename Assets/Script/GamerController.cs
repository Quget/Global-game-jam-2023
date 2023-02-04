using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamerController : MonoBehaviour
{
    [SerializeField]
    private Player player = null;

    [SerializeField]
    private CameraThatFollowsATransform cameraThatFollowsATransform = null;

    [SerializeField]
    private RootEnemy rootEnemyPrefab = null;

    [SerializeField]
    private Grid grid = null;

    private List<RootEnemy> rootEnemies = new List<RootEnemy>();

    [SerializeField]
    private GUI gui = null;

    private IEnumerator Start()
    {
        /*
        yield return new WaitForSeconds(5);
        cameraThatFollowsATransform.SetOrthosize(20);
        yield return new WaitForSeconds(5);
        cameraThatFollowsATransform.ReturnZoom();
        */
        yield return new WaitForSeconds(0.25f);
        cameraThatFollowsATransform.StartFollow(player.transform);

        while (gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 1));
            SpawnRootEnemy();
        }
    }
    private void Update()
    {
        gui.UpdateHealth(player.HealthPercentage);
    }

    private void SpawnRootEnemy()
    {
        RootEnemy rootEnemy = GameObject.Instantiate(rootEnemyPrefab);
        Vector3Int playerCell = grid.WorldToCell(player.transform.position);

        int randomSpread = 1;
        Vector3 enemySpawnPos = grid.GetCellCenterWorld(new Vector3Int(playerCell.x += Random.Range(-randomSpread, randomSpread), playerCell.x += Random.Range(-randomSpread, randomSpread)));
        int tried = 0;
        while (CanSpawn(enemySpawnPos))
        {
            enemySpawnPos = grid.GetCellCenterWorld(new Vector3Int(playerCell.x += Random.Range(-randomSpread, randomSpread), playerCell.x += Random.Range(-randomSpread, randomSpread)));
            tried++;
            if (tried == 10)
                randomSpread = 2;
            else if (tried == 20)
                randomSpread = 4;
            else if (tried == 30)
                randomSpread = 10;
            else
                break;
        }

        enemySpawnPos.z = 0;
        rootEnemy.transform.position = enemySpawnPos;
        rootEnemies.Add(rootEnemy);
    }

    private bool CanSpawn(Vector3 enemySpawnPos)
    {
        float distance = 1000;
        for (int i = 0; i < rootEnemies.Count; i++)
        {
            if (rootEnemies[i] == null)
                continue;
            distance = Mathf.Abs(Vector3.Distance(enemySpawnPos, rootEnemies[i].transform.position));
            if (distance < 1)
                return false;
        }
        return true;
    }
}
