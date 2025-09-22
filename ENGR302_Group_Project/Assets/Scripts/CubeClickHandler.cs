using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class CubeClickHandler : MonoBehaviour
{
    [Header("Prefab Settings")]
    public GameObject prefabToSpawn; // Drag your prefab here in the Inspector
    public Vector3 spawnOffset = new Vector3(0, 2, 0); // Where to spawn relative to cube
    
    [Header("Spawn Options")]
    public bool destroyPreviousPrefab = true; // Destroy old prefab when spawning new one
    public bool lookAtCamera = false; // Make prefab face the camera
    
    private GameObject spawnedPrefab;
    private static GameObject globalSpawnedPrefab; // Track prefab across all cubes

    private bool Paused = false;

    void Start()
    {
        // Make sure the cube has a collider for mouse detection
        if (GetComponent<Collider>() == null)
        {
            Debug.LogWarning($"No collider found on {gameObject.name}. Adding BoxCollider for mouse detection.");
            gameObject.AddComponent<BoxCollider>();
        }


    }
    
    void Update()
    {
        // Check for mouse click
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            CheckForCubeClick();
        }
    }
    
    void CheckForCubeClick()
    {
        if (Paused) { return; }
        // Get mouse position and create a ray from camera
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        
        // Check if ray hits this cube
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // If the hit object is this cube, spawn the prefab
            if (hit.collider.gameObject == gameObject)
            {
                SpawnPrefab();
            }
        }
    }

    void PauseInteraction()
    {
        Paused = true;
    }
    void ResumeInteraction()
    {
        Paused = false;
    }

    void SpawnPrefab()
    {
        

        if (prefabToSpawn == null)
        {
            Debug.LogWarning("No prefab assigned to spawn!");
            return;
        }

        // Destroy previous prefab if enabled
        if (destroyPreviousPrefab && globalSpawnedPrefab != null)
        {
            Destroy(globalSpawnedPrefab);
        }

        // Calculate spawn position
        Vector3 spawnPosition = transform.position + spawnOffset;

        // Spawn the prefab
        spawnedPrefab = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        globalSpawnedPrefab = spawnedPrefab;

        // Optional: Make prefab face the camera
        if (lookAtCamera && Camera.main != null)
        {
            spawnedPrefab.transform.LookAt(Camera.main.transform);
            spawnedPrefab.transform.Rotate(0, 180, 0); // Flip if needed
        }

        Debug.Log($"Prefab spawned by cube '{gameObject.name}' at position {spawnPosition}");


        // Replace the three lines below to use MapManager in order to get the correct question per room rather than the same.
        JsonReader jr = new();
        Question question;

        RoomButtonManager roomButtonManager = FindFirstObjectByType<RoomButtonManager>();
        if (roomButtonManager != null)
        {
            question = jr.GetAllQuestions()[roomButtonManager.GetCurrentRoomIndex()];
        }
        else
        {
            int index = UnityEngine.Random.Range(0, jr.GetAllQuestions().Count);
            question = jr.GetAllQuestions()[index];
        }


        // Before these lines other buttons need to be paused (including CubeClickHandler) to prevent unintended behaviour.

        RoomTeleporter teleporter = FindFirstObjectByType<RoomTeleporter>();

        PauseInteraction();
        teleporter.DisableTeleport();

        Action completionAction = () =>
        {
            Destroy(spawnedPrefab);
            ResumeInteraction();
            teleporter.EnableTeleport();
        };

        spawnedPrefab.GetComponent<TaskScreen>().Execute(question, completionAction);
    }
    
    // Method to destroy the spawned prefab (can be called from UI buttons)
    public void DestroySpawnedPrefab()
    {
        if (spawnedPrefab != null)
        {
            Destroy(spawnedPrefab);
            spawnedPrefab = null;
            
            if (globalSpawnedPrefab == spawnedPrefab)
                globalSpawnedPrefab = null;
        }
    }
}