using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField]
    private MusicBox musicBox = null;

    [SerializeField]
    private AudioClip defaultAudio = null;

    private void Awake()
    {
        cameraThatFollowsATransform.SetUp();
        cameraThatFollowsATransform.SetOrthosize(14, true);
        musicBox.PlayAudio(defaultAudio);
        Application.targetFrameRate = 60;

    }

    private IEnumerator Start()
    {
        for (int i = 0; i < 4; i++)
        {
            SpawnRootEnemy(6);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        }
        player.CanMove = false;
        yield return new WaitForSeconds(2);
        cameraThatFollowsATransform.ReturnZoom();
        yield return new WaitForSeconds(0.25f);
        player.CanMove = true;
        cameraThatFollowsATransform.StartFollow(player.transform);

        yield return new WaitForSeconds(0.25f);
        while (gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f));

            if(rootEnemies.Count < 15)
                SpawnRootEnemy(2);
        }
    }
    private void Update()
    {
        gui.UpdateHealth(player.HealthPercentage);
        if (Input.GetButtonDown("Submit"))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void SpawnRootEnemy(int randomSpread)
    {
        RootEnemy rootEnemy = GameObject.Instantiate(rootEnemyPrefab);
        Vector3Int playerCell = grid.WorldToCell(player.transform.position);

        Vector3 enemySpawnPos = grid.GetCellCenterWorld(new Vector3Int(playerCell.x += Random.Range(-randomSpread, randomSpread), playerCell.y += Random.Range(-randomSpread, randomSpread)));
        int tried = 0;
        while (!CanSpawn(enemySpawnPos))
        {
            enemySpawnPos = grid.GetCellCenterWorld(new Vector3Int(playerCell.x += Random.Range(-randomSpread, randomSpread), playerCell.y += Random.Range(-randomSpread, randomSpread)));
            tried++;
            if (tried == 10)
                randomSpread *= 2;
            else if (tried == 20)
                randomSpread *= 2;
            else if (tried == 30)
                randomSpread *= 2;
            else
                break;
        }

        enemySpawnPos.z = 0;
        rootEnemy.transform.position = enemySpawnPos;
        rootEnemy.SetUp(player.Collider2D,(thisEnemy)=> 
        {
            Debug.Log("Remove me");
            rootEnemies.Remove(rootEnemy);
        });
        rootEnemies.Add(rootEnemy);
    }

    private bool CanSpawn(Vector3 enemySpawnPos)
    {
        float distance = 1000;
        for (int i = 0; i < rootEnemies.Count; i++)
        {
            distance = Mathf.Abs(Vector3.Distance(enemySpawnPos, rootEnemies[i].transform.position));
            if (distance < 1.28f)
                return false;
        }
        return true;
    }
}
