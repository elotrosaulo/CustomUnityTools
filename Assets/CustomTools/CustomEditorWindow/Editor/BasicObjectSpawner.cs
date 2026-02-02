using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class BasicObjectSpawner : EditorWindow
{
    private Transform parentTransform;
    private GameObject objectToSpawn;
    
    private string objectBaseName;
    private int objectID = 1;
    
    private float objectScale = 1f;
    private float spawnRadius = 5f;
    private float yAxisMinPosition = 0f;
    private float yAxisMaxPosition = 0f;

    [MenuItem("Custom Tools/Basic Object Spawner")]
    public static void ShowWindow()     // Called when we open our window
    {
        GetWindow(typeof(BasicObjectSpawner));  
    }

    private void OnGUI()
    {
        GUILayout.Label("Spawn New Object", EditorStyles.boldLabel);

        parentTransform = EditorGUILayout.ObjectField("Parent Transform (Opt.)", parentTransform, typeof(Transform), true) as Transform;  
        objectToSpawn = EditorGUILayout.ObjectField("Prefab to Spawn", objectToSpawn, typeof(GameObject), false) as GameObject; // false = do not accept references from the scene.
        objectBaseName = EditorGUILayout.TextField("Name Prefix", objectBaseName);
        objectID = EditorGUILayout.IntField("Object ID", objectID);
        objectScale = EditorGUILayout.Slider("Object Scale", objectScale, 0.5f, 3f); // left and right value for the slider
        spawnRadius = EditorGUILayout.FloatField("Spawn Radius", spawnRadius);
        yAxisMinPosition = EditorGUILayout.FloatField("Y Axis Min Position", yAxisMinPosition);
        yAxisMaxPosition = EditorGUILayout.FloatField("Y Axis Max Position", yAxisMaxPosition);
        
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

        if (string.IsNullOrEmpty(objectBaseName))
        {
            Debug.LogError("Basic Object Spawner. Please enter a name prefix for the object.");
            return;
        }

        Vector2 spawnCircle = Random.insideUnitCircle * spawnRadius;
        float yPosition = Random.Range(yAxisMinPosition, yAxisMaxPosition);
        
        Vector3 spawnPos = new Vector3(spawnCircle.x, yPosition, spawnCircle.y);
        if (parentTransform != null)
        {
            spawnPos += parentTransform.position;
        }

        GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(objectToSpawn);  // To instantiate real prefabs and not just Game Objects
        Undo.RegisterCreatedObjectUndo(newObject, "Spawn Object");
        
        newObject.transform.position = spawnPos;
        newObject.transform.rotation = Quaternion.identity;
        
        if (parentTransform != null)
        {
            newObject.transform.SetParent(parentTransform, true);   // keep world position
        }
        
        newObject.name = objectBaseName + objectID;
        newObject.transform.localScale = Vector3.one * objectScale;

        objectID++;
    }
}
