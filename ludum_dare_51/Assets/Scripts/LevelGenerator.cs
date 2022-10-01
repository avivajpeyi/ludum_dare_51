using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    private const float PLAYER_DISTANCE_SPAWN_LEVEL_PART = 200f;
    public int startingSpawnLevelParts = 5;

    [SerializeField] private LevelTemplate levelPart_Start;
    [SerializeField] private List<Transform> levelPartList;
    [SerializeField] private Player player;

    private List<Transform> levelCache;
    
    private LevelTemplate lastLevelTemplate;
    private int maxLevelParts = 10;

    private void Start()
    {
        levelCache = new List<Transform>();
        levelCache.Add(levelPart_Start.transform);
        lastLevelTemplate = levelPart_Start;
        for (int i = 0; i < startingSpawnLevelParts; i++) {
            SpawnLevelPart();
        }
    }

    private void Update() {
        if (Vector3.Distance(player.GetPosition(), lastLevelTemplate.GetEndPoint()) < 
            PLAYER_DISTANCE_SPAWN_LEVEL_PART) {
            // Spawn another level part
            SpawnLevelPart();
        }
        
        if (levelCache.Count > maxLevelParts) {
            Destroy(levelCache[0].gameObject);
            levelCache.RemoveAt(0);
        }
        
    }

    private void SpawnLevelPart() {
        Transform chosenLevelPart = levelPartList[Random.Range(0, levelPartList.Count)];
        LevelTemplate newLevelTemplate = SpawnLevelPart(
            chosenLevelPart, 
            lastLevelTemplate.GetEndPoint()
            );
        lastLevelTemplate = newLevelTemplate;
        levelCache.Add(newLevelTemplate.transform);
    }

    private LevelTemplate SpawnLevelPart(Transform levelPart, Vector3 spawnPosition) {
        Transform levelPartTransform = Instantiate(levelPart, spawnPosition, Quaternion.identity);
        LevelTemplate newLevelPart = levelPartTransform.GetComponent<LevelTemplate>();
        newLevelPart.AlignToPreviousLevelPart(lastLevelTemplate);
        return newLevelPart;
    }

}
