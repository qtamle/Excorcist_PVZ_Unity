using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieSpawner : MonoBehaviour
{
    public Transform[] spawnpoints;
    public WaveGhost[] waveGhosts;

    private List<ZombieType> probList = new List<ZombieType>();

    public int zombiesSpawned;
    public int zombiesDefeated;
    public Slider progressBar;
    public float zombieDelay = 5;
    public AudioClip zombieSpawnAudio;
    public AudioClip zombieComing;
    public Image centerImage;

    private AudioSource audioSource;
    private bool firstZombieSpawned = false;
    private int currentWaveIndex = 0;
    private Animator animator;

    public GameObject giftPrefab;

    private bool gameEnded = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        HideCenterImage();
        animator = GetComponent<Animator>();

        // Khởi động wave đầu tiên
        StartNextWave();
    }

    private void Update()
    {
        progressBar.value = zombiesSpawned;
    }

    private void StartNextWave()
    {
        if (currentWaveIndex >= waveGhosts.Length)
        {
            Debug.Log("All waves completed.");
            StartCoroutine(WaitAndEndGame());
            return;
        }

        probList.Clear();
        foreach (ZombieType zom in waveGhosts[currentWaveIndex].zombieTypes)
        {
            for (int i = 0; i < zom.probability; i++)
            {
                probList.Add(zom);
            }
        }

        zombiesSpawned = 0;
        zombiesDefeated = 0;
        progressBar.maxValue = waveGhosts[currentWaveIndex].defeatedCount;
        InvokeRepeating("SpawnZombie", 10, zombieDelay);

        if (centerImage != null)
        {
            centerImage.gameObject.SetActive(true);
            StartCoroutine(HideCenterImageAfterDelay(3f));
        }

        if (currentWaveIndex > 0 && audioSource != null && zombieComing != null)
        {
            audioSource.PlayOneShot(zombieComing);
        }
    }

    void SpawnZombie()
    {
        int r = Random.Range(0, spawnpoints.Length);

        ZombieType selectedZombieType = probList[Random.Range(0, probList.Count)];
        GameObject myZombie = Instantiate(selectedZombieType.type.zombiePrefab, spawnpoints[r].position, Quaternion.identity);

        Zombie zombieComponent = myZombie.GetComponent<Zombie>();
        if (zombieComponent == null)
        {
            zombieComponent = myZombie.AddComponent<Zombie>();
        }
        zombieComponent.type = selectedZombieType.type;

        zombiesSpawned++;

        if (!firstZombieSpawned && audioSource != null && zombieSpawnAudio != null)
        {
            audioSource.PlayOneShot(zombieSpawnAudio);
            firstZombieSpawned = true;
        }

        if (zombiesSpawned >= waveGhosts[currentWaveIndex].defeatedCount)
        {
            CancelInvoke("SpawnZombie");
            if (currentWaveIndex == waveGhosts.Length - 1)
            {
                // Đây là wave cuối cùng
                StartCoroutine(WaitAndEndGame());
            }
            else
            {
                StartCoroutine(WaitAndStartNextWave());
            }
        }
    }

    public void OnZombieDefeated()
    {
        zombiesDefeated++;
        Debug.Log($"Zombies Defeated: {zombiesDefeated}");

        if (currentWaveIndex == waveGhosts.Length - 1 && zombiesDefeated >= waveGhosts[currentWaveIndex].defeatedCount)
        {
            Vector3 lastZombiePosition = FindLastZombiePosition();
            if (lastZombiePosition != Vector3.zero && giftPrefab != null)
            {
                GameObject gift = Instantiate(giftPrefab, lastZombiePosition, Quaternion.identity);
                gift.AddComponent<Gift>();
            }
            gameEnded = true;
            StartCoroutine(WaitAndEndGame());
        }
    }

    IEnumerator WaitAndStartNextWave()
    {
        while (zombiesSpawned < waveGhosts[currentWaveIndex].defeatedCount)
        {
            yield return null;
        }

        for (int i = 5; i > 0; i--)
        {
            Debug.Log($"Preparing to start next wave in {i} second(s)...");
            yield return new WaitForSeconds(1);
        }
        HideCenterImage();

        currentWaveIndex++;
        StartNextWave();
    }

    IEnumerator WaitAndEndGame()
    {
        // Chờ cho đến khi tất cả zombie trong game bị tiêu diệt
        while (GameObject.FindWithTag("Ghost") != null)
        {
            yield return null;
        }

        Debug.Log("All zombies defeated. Game over!");

        if (!gameEnded)
        {
            gameEnded = true;

            // Tạo món quà ở giữa màn hình
            if (giftPrefab != null)
            {
                Vector3 giftPosition = GetGiftPosition();
                GameObject gift = Instantiate(giftPrefab, giftPosition, Quaternion.identity);

                gift.AddComponent<Gift>();
            }
        }
    }


    IEnumerator HideCenterImageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideCenterImage();
    }

    void HideCenterImage()
    {
        if (centerImage != null)
        {
            centerImage.gameObject.SetActive(false);
        }
    }

    Vector3 GetGiftPosition()
    {
        // Đặt món quà ở giữa màn hình
        return Vector3.zero;
    }

    Vector3 FindLastZombiePosition()
    {
        // Tìm vị trí của con zombie cuối cùng
        Zombie[] zombies = FindObjectsOfType<Zombie>();
        Vector3 lastPosition = Vector3.zero;
        foreach (Zombie zombie in zombies)
        {
            if (zombie != null && zombie.gameObject.activeSelf)
            {
                lastPosition = zombie.transform.position;
            }
        }
        return lastPosition;
    }

    [System.Serializable]
    public class WaveGhost
    {
        public ZombieType[] zombieTypes;
        public int defeatedCount;
    }

    [System.Serializable]
    public class ZombieType
    {
        public ZombieTypes type;
        public int probability;
    }
}
