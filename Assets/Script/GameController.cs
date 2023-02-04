using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
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

    [SerializeField]
    private PowerUpSetting[] powerUpSettings = null;

    [SerializeField]
    private PowerUpPickable powerUpPickablePrefab = null;

    [SerializeField]
    private float changeToDropOnKill = 100;

    [SerializeField]
    private Vector2 minMaxSpawnTimeRandom = new Vector2(4, 4);

    [SerializeField]
    private int startSpread = 4;

    [SerializeField]
    private int maxEnemyCount = 15;

    [SerializeField]
    private float difficultyTick = 10f;

    [SerializeField]
    private float changeToDropReductionPerTick = 5f;

    [SerializeField]
    private RootBoss rootBoss = null;

    private void Awake()
    {
        cameraThatFollowsATransform.SetUp();
        cameraThatFollowsATransform.SetOrthosize(14, true);
        musicBox.PlayAudio(defaultAudio);
        Application.targetFrameRate = 60;

        FindObjectOfType<MapController>().SetUp(player.transform);

    }

    private IEnumerator Start()
    {
        Vector3Int playerCell = grid.WorldToCell(player.transform.position);
        int randomSpread = 4;
        for (int i = 0; i < 5; i++)
        {
            Vector3 powerUpPosition = grid.GetCellCenterWorld(new Vector3Int(playerCell.x += Random.Range(-randomSpread, randomSpread), playerCell.y += Random.Range(-randomSpread, randomSpread)));
            CreateRandomPowerUp(powerUpPosition);
        }

        rootBoss.SetUp(player.Collider2D, player.transform, (thisRootBoss, killed) => 
        { 
        
        });

        for (int i = 0; i < 2; i++)
        {
            SpawnRootEnemy(8);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        }
        player.CanMove = false;
        yield return new WaitForSeconds(2);
        cameraThatFollowsATransform.ReturnZoom();
        yield return new WaitForSeconds(0.25f);
        player.CanMove = true;
        cameraThatFollowsATransform.StartFollow(player.transform);

        yield return new WaitForSeconds(0.25f);
        StartCoroutine(StartSpawnLoop());
        StartCoroutine(DifficultyTick());
    }

    private IEnumerator StartSpawnLoop()
    {
        while (gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(Random.Range(minMaxSpawnTimeRandom.x, minMaxSpawnTimeRandom.y));

            if (rootEnemies.Count < maxEnemyCount)
                SpawnRootEnemy(startSpread);
        }
    }

    private IEnumerator DifficultyTick()
    {
        while (gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(difficultyTick);
            minMaxSpawnTimeRandom = new Vector2(minMaxSpawnTimeRandom.x * 0.8f, minMaxSpawnTimeRandom.y * 0.9f);

            startSpread--;
            if (startSpread < 2)
                startSpread = 2;

            maxEnemyCount += 1;

            if (maxEnemyCount > 50)
            {
                maxEnemyCount = 50;
            }

            changeToDropOnKill -= changeToDropReductionPerTick;
            if (changeToDropOnKill < 5)
                changeToDropOnKill = 5;
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

    private Vector3 GetEnemySpawnInGrid()
    {


        return Vector3.zero;
    }

    private Grid GetGridNearPlayer()
    {
        Grid[] grids = FindObjectsOfType<Grid>();
        float choosenDistance, distance = 0;
        Grid choosenOne = grids[0];
        choosenDistance = Vector3.Distance(choosenOne.transform.position, player.transform.position);
        for (int i = 1; i < grids.Length; i++)
        {
            distance = Vector3.Distance(choosenOne.transform.position, player.transform.position);
            if (distance < choosenDistance)
            {
                choosenOne = grids[i];
                choosenDistance = Vector3.Distance(choosenOne.transform.position, player.transform.position);
            }
        }
        return choosenOne;
    }

    private void SpawnRootEnemy(int randomSpread)
    {

        Grid grid = GetGridNearPlayer();
        Vector3Int playerCell = grid.WorldToCell(player.transform.position);
        Vector3 enemySpawnPos = grid.GetCellCenterWorld(new Vector3Int(playerCell.x += Random.Range(-randomSpread, randomSpread), playerCell.y += Random.Range(-randomSpread, randomSpread)));
        int tried = 0;

        bool spawnFailed = false;
        while (!CanSpawn(enemySpawnPos))
        {
            enemySpawnPos = grid.GetCellCenterWorld(new Vector3Int(playerCell.x += Random.Range(-randomSpread, randomSpread), playerCell.y += Random.Range(-randomSpread, randomSpread)));
            tried++;

            /*
            if (tried == 10)
                randomSpread *= 2;
            else if (tried == 20)
                randomSpread *= 2;
            else if (tried == 30)
                randomSpread *= 2;
            else
            {
                spawnFailed = true;
                break;
            }
            */

            if(tried > 30)
            {
                spawnFailed = true;
                break;
            }
        }

        if(spawnFailed)
        {
            //Debug.Log("Spawn failed terribly");
            return;
        }

        RootEnemy rootEnemy = GameObject.Instantiate(rootEnemyPrefab);
        enemySpawnPos.z = 0;
        rootEnemy.transform.position = enemySpawnPos;
        rootEnemy.SetUp(player.Collider2D,(thisEnemy, killed)=> 
        {
            if (killed)
            {
                float percentage = Random.Range(0f, 100f);
                if (percentage < changeToDropOnKill)
                {
                    CreateRandomPowerUp(thisEnemy.transform.position);
                }
            }
            rootEnemies.Remove(thisEnemy);
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

    private void CreateRandomPowerUp(Vector3 position)
    {
        PowerUpPickable powerUpPickable = GameObject.Instantiate(powerUpPickablePrefab);
        powerUpPickable.SetUp(powerUpSettings[Random.Range(0, powerUpSettings.Length)]);
        powerUpPickable.transform.position = position;
    }
}