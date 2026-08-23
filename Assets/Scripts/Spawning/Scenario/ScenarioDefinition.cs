using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spawning/Scenario")]
public class ScenarioDefinition : ScriptableObject
{
    public string scenarioName;

    public int minDifficulty = 0;
    public int maxDifficulty = 10;
    public float weight = 1f;

    // public float duration = 3f;
    public float distanceInterval = 20f;

    public List<ScenarioSpawnStep> steps = new List<ScenarioSpawnStep>();
}

[System.Serializable]
public class ScenarioSpawnStep
{
    public ObjectTags objectTag;

    //Position relative to the scenario spawn origin."
    public Vector2 offset;

    public SpawnOptions? spawnOptions;
}
