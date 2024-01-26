using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
using Random = System.Random; // Unity.Random doesn't have restorable states

public class LevelController : MonoBehaviour
{
    private static LevelController Instance { get; set; } // don't give access to the singleton - it's just that we don't want two of this.
    
    public int m_randomSeed = 1;
    public float m_waveDelay = 5f;// this should be set per wave, eventually we'll want crossing waves and so forth
    public int m_maxWaveLength = 5;
    public int m_maxEnemyFiringPattern = 3;
    public int m_minEnemyFiringPattern = 1;
    public int m_maxAutoFireType = 1;
    public float m_shipDelay = 0.6f;
    public float m_enemySpeed = 1f;
    private Random m_random;
    private SplineContainer m_splineContainer;
    private BulletLauncherConfig m_bulletLauncherConfig;
    private bool m_ready;
    private float m_nextWaveTimer = 0.0f;
    private float m_playerSpawnTimer = 1.0f;
    private GameObject m_playerPrefab;
    private GameObject m_playerInstance;
    
    private void Awake()
    {
        // If there is an instance, and it's not this, delete this.
    
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
        
        m_random = new Random(m_randomSeed);
        PoolsManager.Instance.CreatePools();
        AssetManager.Instance.RequestAsset<GameObject>("Assets/Prefabs/Player.prefab", (obj) => { m_playerPrefab = obj; });
        AssetManager.Instance.RequestAsset<GameObject>("Assets/Prefabs/SplineList.prefab", (obj) =>
        {
            StartCoroutine(SetupEnemies(obj));
        });
        AssetManager.Instance.RequestAsset<BulletLauncherConfig>("Assets/Configs/BulletLauncherConfig.asset", (obj) =>
        {
            m_bulletLauncherConfig = obj;
        });
        
        LivesManager.Reset();
        ScoreManager.Reset();
    }

    private void Update()
    {
        if (!m_ready)
        {
            return;
        }

        UpdateEnemies();
        UpdatePlayer();
    }

    private void UpdateEnemies()
    {
        m_nextWaveTimer -= Time.deltaTime;
        if (m_nextWaveTimer <= 0.0f)
        {
            CreateRandomWave();
            m_nextWaveTimer += m_waveDelay;
        }
    }

    private void UpdatePlayer()
    {
        if (m_playerInstance == null)
        {
            m_playerSpawnTimer -= Time.deltaTime;
            if (m_playerSpawnTimer <= 0.0f)
            {
                if (LivesManager.Lives > 0)
                {
                    m_playerInstance = Instantiate(m_playerPrefab);
                    m_playerInstance.GetComponent<Shooter>().SetConfiguration(m_bulletLauncherConfig.m_launcherTypes[0]);
                }
                else
                {
                    //GameOver
                    AssetManager.Instance.LoadScene("Assets/Scenes/DeadScene.unity");
                }
            }
        }
    }

    private IEnumerator SetupEnemies(GameObject splineContainerObject)
    {
        m_splineContainer = splineContainerObject.GetComponent<SplineContainer>();
        while (!PoolsManager.Instance.ArePoolsReady())
        {
            yield return null;
        }
        BulletManager.Setup();

        m_ready = true;
    }

    private void CreateRandomWave()
    {
        var spline =  m_splineContainer[m_random.Next(m_splineContainer.Splines.Count)];
        var wavelength = m_random.Next(m_maxWaveLength);
        var firingPattern = m_random.Next(m_minEnemyFiringPattern, m_maxEnemyFiringPattern);
        var autoFiringPattern = m_random.Next(m_maxAutoFireType);
        
        if (!PoolsManager.Instance.TryGetPool("Enemy", out var enemyPool))
        {
            Debug.Log("Oh noes! Can't get Enemy Pool");
            return;
        }

        for (int i = 0; i < wavelength; ++i)
        {
            if (!enemyPool.TryGetPooledObject(out var pooledObject))
            {
                Debug.Log("Oh noes! Can't get pooled object");
                return;
            }

            var follower = pooledObject.GetComponent<SplineFollower>();
            follower.Setup(spline, m_enemySpeed, i * m_shipDelay);
            follower.onSplineEnd += ReturnSplineFollowerToPool;

            var shooter = pooledObject.GetComponent<Shooter>();
            if (shooter != null)
            {
                shooter.SetConfiguration(m_bulletLauncherConfig.m_launcherTypes[firingPattern]);
            }

            var autofire = pooledObject.GetComponent<AutoFire>();
            if (autofire != null)
            {
                autofire.SetConfiguration(m_bulletLauncherConfig.m_autoFireTypes[autoFiringPattern]);
            }
        }
    }

    private void ReturnSplineFollowerToPool(SplineFollower follower)
    {
        follower.onSplineEnd -= ReturnSplineFollowerToPool;
        PoolsManager.Instance.ReturnObject("Enemy", follower.gameObject);
    }
}
