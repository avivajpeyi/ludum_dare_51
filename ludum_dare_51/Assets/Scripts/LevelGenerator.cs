using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    private const float PLAYER_DISTANCE_SPAWN_LEVEL_PART = 200f;

    [SerializeField] private LevelTemplate levelPart_Start;
    [SerializeField] private List<Transform> levelPartList;
    [SerializeField] private Player player;

    private LevelTemplate lastLevelTemplate;

    private void Start() {
        lastLevelTemplate = levelPart_Start;

        int startingSpawnLevelParts = 5;
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
    }

    private void SpawnLevelPart() {
        Transform chosenLevelPart = levelPartList[Random.Range(0, levelPartList.Count)];
        LevelTemplate newLevelTemplate = SpawnLevelPart(
            chosenLevelPart, 
            lastLevelTemplate.GetEndPoint()
            );
        lastLevelTemplate = newLevelTemplate;
    }

    private LevelTemplate SpawnLevelPart(Transform levelPart, Vector3 spawnPosition) {
        Transform levelPartTransform = Instantiate(levelPart, spawnPosition, Quaternion.identity);
        LevelTemplate newLevelPart = levelPartTransform.GetComponent<LevelTemplate>();
        newLevelPart.AlignToPreviousLevelPart(lastLevelTemplate);
        return newLevelPart;
    }

}
