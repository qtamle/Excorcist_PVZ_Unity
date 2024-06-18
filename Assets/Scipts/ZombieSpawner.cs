using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieSpawner : MonoBehaviour
{
    public Transform[] spawnpoints;
    public WaveGhost[] waveGhosts;

    private List<ZombieTypes> probList = new List<ZombieTypes>();

    public int zombieMax;
    public int zombiesSpawned;
    public Slider progressBar;
    public float zombieDelay = 5;
    public AudioClip zombieSpawnAudio;
    public AudioClip zombieComing;
    public Image centerImage; // Reference to the center Image on the screen

    private AudioSource audioSource;
    private bool firstZombieSpawned = false;
    private int currentWaveIndex = 0;
    private Animator animator;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        HideCenterImage();
        animator = GetComponent<Animator>();
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
            return;
        }

        probList.Clear();
        foreach (ZombieType zom in waveGhosts[currentWaveIndex].zombieTypes)
        {
            for (int i = 0; i < zom.probability; i++)
            {
                probList.Add(zom.type);
            }
        }

        zombiesSpawned = 0;
        progressBar.maxValue = waveGhosts[currentWaveIndex].defeatedCount;
        InvokeRepeating("SpawnZombie", 10, zombieDelay);


        // Show centerImage at the start of the next wave
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
        zombiesSpawned++;
        int r = Random.Range(0, spawnpoints.Length);

        GameObject myZombie = Instantiate(probList[Random.Range(0, probList.Count)].zombiePrefab, spawnpoints[r].position, Quaternion.identity);

        // Attach Zombie component if not already attached
        Zombie zombieComponent = myZombie.GetComponent<Zombie>();
        if (zombieComponent == null)
        {
            zombieComponent = myZombie.AddComponent<Zombie>();
        }
        zombieComponent.type = probList[Random.Range(0, probList.Count)];

        // Play spawn audio
        if (!firstZombieSpawned && audioSource != null && zombieSpawnAudio != null)
        {
            audioSource.PlayOneShot(zombieSpawnAudio);
            firstZombieSpawned = true;
        }

        if (zombiesSpawned >= waveGhosts[currentWaveIndex].defeatedCount)
        {
            CancelInvoke("SpawnZombie");
            StartCoroutine(WaitAndStartNextWave());
        }
    }

    IEnumerator WaitAndStartNextWave()
    {
        // Wait until all zombies of the current wave are defeated
        while (zombiesSpawned < waveGhosts[currentWaveIndex].defeatedCount)
        {
            yield return null;
        }

        for (int i = 5; i > 0; i--)
        {
            Debug.Log($"Preparing to start next wave in {i} second(s)...");
            yield return new WaitForSeconds(1);
        }
        // Hide centerImage before starting the next wave
        HideCenterImage();

        currentWaveIndex++;
        StartNextWave();
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
