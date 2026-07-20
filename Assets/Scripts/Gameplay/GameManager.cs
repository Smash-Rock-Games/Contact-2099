using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StoneSmashGames.Contact.Gameplay
{
    [System.Serializable]
    public class Location
    {
        public string name;
        public bool isAdded;
        public Transform[] spawnPoints;
    }

    public class GameManager : MonoBehaviour
    {
        public GameObject bugPrefab;

        public float roundDelay = 10f;
        public int currentRound = 0;

        public float bugsAddedPerRound = 3.5f;
        public int startingBugs = 8;

        public Vector2 spawnDelay = new Vector2(1.5f, 3.5f);
        public int maxSpawnedBugs = 35;

        public int remainingBugs = 0;
        public int spawnedBugs = 0;
        public int totalBugsToSpawn = 0;

        public Location[] locations;
        public List<Transform> activeSpawnPoints = new List<Transform>();

        void Start()
        {
            AddLocationSpawns(0);
            StartNextRound();
        }

        void Update()
        {
            if (remainingBugs <= 0)
                StartNextRound();
        }

        void StartNextRound()
        {
            currentRound++;
            totalBugsToSpawn = startingBugs + (int)(currentRound * bugsAddedPerRound);
            remainingBugs = totalBugsToSpawn;

            StartCoroutine(SpawnWave());
        }

        IEnumerator SpawnWave()
        {
            yield return new WaitForSeconds(roundDelay);

            while (spawnedBugs < totalBugsToSpawn)
            {
                if(spawnedBugs < maxSpawnedBugs && spawnedBugs < remainingBugs)
                {
                    SpawnBug();
                }

                yield return new WaitForSeconds(Random.Range(spawnDelay.x, spawnDelay.y));
            }
        }

        public void SpawnBug()
        {
            Transform _targetSpawnPoint = activeSpawnPoints[Random.Range(0, activeSpawnPoints.Count)];

            spawnedBugs++;
            Instantiate(bugPrefab, _targetSpawnPoint.position, _targetSpawnPoint.rotation);
        }

        public void AddLocationSpawns(int _locationIndex)
        {
            if (locations[_locationIndex].isAdded) { return; }

            activeSpawnPoints.AddRange(locations[_locationIndex].spawnPoints);
            locations[_locationIndex].isAdded = true;
        }
    }
}
