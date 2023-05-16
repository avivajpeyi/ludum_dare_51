using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    private const float PLAYER_DISTANCE_SPAWN_LEVEL_PART = 200f;
    public int startingSpawnLevelParts = 5;

    [SerializeField] private LevelRoom levelPart_Start;
    [SerializeField] private List<Transform> levelPartList;
    [SerializeField] private Player player;

    private List<Transform> levelCache;
    
    private LevelRoom _lastLevelRoom;
    private int maxLevelParts = 10;

    private void Start()
    {
        levelCache = new List<Transform>();
        levelCache.Add(levelPart_Start.transform);
        _lastLevelRoom = levelPart_Start;
        for (int i = 0; i < startingSpawnLevelParts; i++) {
            SpawnLevelPart();
        }
    }

    private void Update() {
        if (Vector3.Distance(player.GetPosition(), _lastLevelRoom.GetEndPoint()) < 
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
        LevelRoom newLevelRoom = SpawnLevelPart(
            chosenLevelPart, 
            _lastLevelRoom.GetEndPoint()
            );
        _lastLevelRoom = newLevelRoom;
        levelCache.Add(newLevelRoom.transform);
    }

    private LevelRoom SpawnLevelPart(Transform levelPart, Vector3 spawnPosition) {
        Transform levelPartTransform = Instantiate(levelPart, spawnPosition, Quaternion.identity);
        LevelRoom newLevelPart = levelPartTransform.GetComponent<LevelRoom>();
        newLevelPart.AlignToPreviousLevelPart(_lastLevelRoom);
        return newLevelPart;
    }

}
