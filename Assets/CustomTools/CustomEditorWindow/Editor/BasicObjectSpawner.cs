using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class BasicObjectSpawner : EditorWindow
{
    private string objectBaseName;
    private int objectID = 1;
    private GameObject objectToSpawn;
    private float objectScale = 1f;
    private float spawnRadius = 5f;

    [MenuItem("Custom Tools/Basic Object Spawner")]
    public static void ShowWindow()     // Called when we open our window
    {
        GetWindow(typeof(BasicObjectSpawner));  
    }

    private void OnGUI()
    {
        GUILayout.Label("Spawn New Object", EditorStyles.boldLabel);

        objectBaseName = EditorGUILayout.TextField("Name Prefix", objectBaseName);
        objectID = EditorGUILayout.IntField("Object ID", objectID);
        objectScale = EditorGUILayout.Slider("Object Scale", objectScale, 0.5f, 3f); // left and right value for the slider
        spawnRadius = EditorGUILayout.FloatField("Spawn Radius", spawnRadius);
        objectToSpawn = EditorGUILayout.ObjectField("Prefab to Spawn", objectToSpawn, typeof(GameObject), false) as GameObject; // false is just to avoid spawning Scene Objects

        if (GUILayout.Button("Spawn Object"))   // If it's pressed, call the function in the body...
        {
            SpawnObject();
        }
    }

    private void SpawnObject()
    {
        if (objectToSpawn == null)
        {
            Debug.LogError("Basic Object Spawner. Please assign an object to be spawned.");
            return;
        }

        if (String.IsNullOrEmpty(objectBaseName))
        {
            Debug.LogError("Basic Object Spawner. Please enter a name prefix for the object.");
            return;
        }

        Vector2 spawnCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = new Vector3(spawnCircle.x, 0f, spawnCircle.y);   // For the moment, spawn always at y = 0

        GameObject newObject = Instantiate(objectToSpawn, spawnPos, Quaternion.identity);
        newObject.name = objectBaseName + objectID;
        newObject.transform.localScale = Vector3.one * objectScale;

        objectID++;
    }
}
