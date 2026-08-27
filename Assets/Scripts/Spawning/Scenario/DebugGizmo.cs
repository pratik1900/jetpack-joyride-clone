using System.Collections.Generic;
using UnityEngine;

// TEMP DEBUG TOOL — delete this file once the balancing pass is done.
// Draws a live wireframe box around a scenario's actual footprint. Bounds are
// recomputed from scenario.steps[*].offset on every gizmo draw call (nothing
// cached), so editing offsets on the ScenarioDefinition asset — even while
// playing — moves the box immediately.
//
// Some steps expand into more than one object at spawn time (e.g. Coin steps
// via CoinSpawnHandler picking a random grid formation). Since that pick is
// random, this uses the WORST-CASE spread across the handler's known
// formations rather than guessing which one a given spawn rolled — that's
// what you actually want when checking for overlap/spacing.
public class ScenarioBoundsGizmo : MonoBehaviour
{
    public ScenarioDefinition scenario;
    public Color color = Color.cyan;

    [Tooltip(
        "Small dot on every step's exact spawn anchor — lets you confirm which objects this scenario's steps list actually claims, vs. objects from a neighboring scenario that just happen to be nearby."
    )]
    public bool showStepMarkers = true;

    private Dictionary<ObjectTags, SpawnHandler> handlers;

    // Use this if your scenario objects share a common parent/root GameObject
    // per spawn. One line at your existing spawn site:
    //   ScenarioBoundsGizmo.AttachTo(scenarioRoot, scenario, handlers);
    // The gizmo rides along with (and is destroyed with) that root automatically.
    public static void AttachTo(
        GameObject scenarioRoot,
        ScenarioDefinition scenario,
        Dictionary<ObjectTags, SpawnHandler> handlers
    )
    {
        var gizmo = scenarioRoot.AddComponent<ScenarioBoundsGizmo>();
        gizmo.scenario = scenario;
        gizmo.handlers = handlers;
    }

    // Use this if you spawn each object individually with no common parent.
    // One line at your existing spawn site, using the same origin/handlers you already have:
    //   ScenarioBoundsGizmo.Attach(scenario, origin, handlers);
    // Creates a small standalone marker object to hold the gizmo.
    public static ScenarioBoundsGizmo Attach(
        ScenarioDefinition scenario,
        Vector3 origin,
        Dictionary<ObjectTags, SpawnHandler> handlers
    )
    {
        var go = new GameObject($"[Debug] {scenario.scenarioName} bounds");
        go.transform.position = origin;
        go.AddComponent<ScrollLeft>(); // keeps the marker moving in sync with spawned objects
        var gizmo = go.AddComponent<ScenarioBoundsGizmo>();
        gizmo.scenario = scenario;
        gizmo.handlers = handlers;
        return gizmo;
    }

    private void OnDrawGizmos()
    {
        if (scenario == null || scenario.steps == null || scenario.steps.Count == 0)
            return;

        Vector3 origin = transform.position;
        Bounds bounds = new Bounds(origin + (Vector3)scenario.steps[0].offset, Vector3.zero);

        foreach (var step in scenario.steps)
        {
            Vector3 anchor = origin + (Vector3)step.offset;
            Vector2 halfSpread = GetWorstCaseHalfSpread(step);

            bounds.Encapsulate(anchor + new Vector3(halfSpread.x, halfSpread.y, 0f));
            bounds.Encapsulate(anchor - new Vector3(halfSpread.x, halfSpread.y, 0f));
        }

        Gizmos.color = color;
        Gizmos.DrawWireCube(bounds.center, bounds.size);

        if (showStepMarkers)
        {
            Gizmos.color = Color.magenta;
            foreach (var step in scenario.steps)
                Gizmos.DrawWireSphere(origin + (Vector3)step.offset, 0.15f);
        }
    }

    private static readonly Dictionary<string, Vector2> prefabUnrotatedSizeCache =
        new Dictionary<string, Vector2>();

    // Worst-case half-extent a single step could spread to once its handler
    // does its thing (a random coin formation, a rotated laser prefab, etc).
    // Add more "is XHandler" cases here as other handlers turn out to need it.
    private Vector2 GetWorstCaseHalfSpread(ScenarioSpawnStep step)
    {
        if (handlers == null || !handlers.TryGetValue(step.objectTag, out SpawnHandler handler))
            return Vector2.zero;

        if (handler is CoinSpawnHandler coinHandler && coinHandler.formations != null)
        {
            float maxWidth = 0f;
            float maxHeight = 0f;

            foreach (var formation in coinHandler.formations)
            {
                maxWidth = Mathf.Max(maxWidth, (formation.cols - 1) * formation.spacing);
                maxHeight = Mathf.Max(maxHeight, (formation.rows - 1) * formation.spacing);
            }

            return new Vector2(maxWidth * 0.5f, maxHeight * 0.5f);
        }

        if (handler is LaserSpawnHandler)
        {
            Vector2 unrotatedSize = GetPrefabUnrotatedSize(step.objectTag.ToString());

            if (step.spawnOptions != null && step.spawnOptions.useCustomRotation)
                return GetRotatedHalfExtents(unrotatedSize, step.spawnOptions.rotationZ);

            // No fixed rotation for this step -> the laser could end up at any
            // angle, so use the diagonal as a worst-case square envelope.
            float diag = unrotatedSize.magnitude;
            return new Vector2(diag * 0.5f, diag * 0.5f);
        }

        return Vector2.zero;
    }

    // Real, unrotated world-size of a tag's prefab (as authored, rotation
    // zero), read straight from ObjectPooler's prefab reference — combines
    // ALL child renderers so e.g. laser end-cap sprites are included, not
    // just the main beam. Cached since prefabs don't change mid-session.
    private Vector2 GetPrefabUnrotatedSize(string tag)
    {
        if (prefabUnrotatedSizeCache.TryGetValue(tag, out Vector2 cached))
            return cached;

        Vector2 size = Vector2.zero;
        GameObject prefab = FindPrefab(tag);

        if (prefab != null)
        {
            Renderer[] renderers = prefab.GetComponentsInChildren<Renderer>(true);
            if (renderers.Length > 0)
            {
                Bounds combined = renderers[0].bounds;
                for (int i = 1; i < renderers.Length; i++)
                    combined.Encapsulate(renderers[i].bounds);
                size = combined.size;
            }
        }

        prefabUnrotatedSizeCache[tag] = size;
        return size;
    }

    private GameObject FindPrefab(string tag)
    {
        if (ObjectPooler.Instance == null || ObjectPooler.Instance.poolsDetailsList == null)
            return null;

        foreach (var details in ObjectPooler.Instance.poolsDetailsList)
        {
            if (details.tag == tag)
                return details.prefab;
        }

        return null;
    }

    private static Vector2 GetRotatedHalfExtents(Vector2 unrotatedSize, float rotationZDegrees)
    {
        float rad = rotationZDegrees * Mathf.Deg2Rad;
        float cos = Mathf.Abs(Mathf.Cos(rad));
        float sin = Mathf.Abs(Mathf.Sin(rad));

        float rotatedWidth = unrotatedSize.x * cos + unrotatedSize.y * sin;
        float rotatedHeight = unrotatedSize.x * sin + unrotatedSize.y * cos;

        return new Vector2(rotatedWidth * 0.5f, rotatedHeight * 0.5f);
    }
}
