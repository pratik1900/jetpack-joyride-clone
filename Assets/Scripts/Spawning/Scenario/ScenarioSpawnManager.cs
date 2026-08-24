using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioSpawnManager : MonoBehaviour
{
    //Spawn origin
    [SerializeField]
    private float spawnOriginX = 15f;

    [SerializeField]
    private float spawnOriginY = 0f;

    // [SerializeField]
    // private int currentDifficulty = 0;

    [SerializeField]
    private List<ScenarioDefinition> scenarios = new List<ScenarioDefinition>();

    private Dictionary<ObjectTags, SpawnHandler> handlers;

    [SerializeField]
    private DistanceTracker distanceTracker;

    private void Start()
    {
        BuildHandlerLookup();
        StartCoroutine(SpawnScenarioLoop());
    }

    private void BuildHandlerLookup()
    {
        handlers = new Dictionary<ObjectTags, SpawnHandler>();

        foreach (SpawnHandler handler in GetComponentsInChildren<SpawnHandler>())
        {
            handlers[handler.ObjectTag] = handler;
        }
    }

    private IEnumerator SpawnScenarioLoop()
    {
        while (true)
        {
            ScenarioDefinition scenario = PickScenario();

            if (scenario == null)
            {
                Debug.LogWarning("No valid spawn scenario found.");
                yield return new WaitForSeconds(1f);
                continue;
            }

            SpawnScenario(scenario);

            // yield return new WaitForSeconds(scenario.duration);
            yield return WaitForDistance(scenario.distanceInterval);
        }
    }

    private IEnumerator WaitForDistance(float distanceInterval)
    {
        float startDistance = distanceTracker.MetersTraveled;
        while (distanceTracker.MetersTraveled - startDistance < distanceInterval)
            yield return null;
    }

    private void SpawnScenario(ScenarioDefinition scenario)
    {
        Vector3 origin = new Vector3(spawnOriginX, spawnOriginY, 0f);

        foreach (ScenarioSpawnStep step in scenario.steps)
        {
            if (!handlers.TryGetValue(step.objectTag, out SpawnHandler handler))
            {
                Debug.LogWarning($"No spawn handler found for object tag: {step.objectTag}");
                continue;
            }

            Vector3 spawnPosition = origin + new Vector3(step.offset.x, step.offset.y, 0f);

            handler.Spawn(spawnPosition, step.spawnOptions);
        }
    }

    private ScenarioDefinition PickScenario()
    {
        int currentDifficulty = DifficultyManager.Instance.CurrentScenarioDifficultyLevel;

        List<ScenarioDefinition> validScenarios = new List<ScenarioDefinition>();

        foreach (ScenarioDefinition scenario in scenarios)
        {
            if (
                currentDifficulty >= scenario.minDifficulty
                && currentDifficulty <= scenario.maxDifficulty
                && scenario.weight > 0f
            )
            {
                validScenarios.Add(scenario);
            }
        }

        if (validScenarios.Count == 0)
        {
            return null;
        }

        float totalWeight = 0f;

        foreach (ScenarioDefinition scenario in validScenarios)
        {
            totalWeight += scenario.weight;
        }

        float roll = Random.Range(0f, totalWeight);

        foreach (ScenarioDefinition scenario in validScenarios)
        {
            roll -= scenario.weight;

            if (roll <= 0f)
            {
                return scenario;
            }
        }

        return validScenarios[validScenarios.Count - 1];
    }
}
