using System;
using System.Collections.Generic;
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
    
    [Header("Visual Feedback")]
    public Material completedMaterial; // Optional: Material to show completed cubes
    
    [Header("Question Assignment")]
    [SerializeField] private int assignedQuestionIndex = -1; // Manually assign in inspector or use auto-assignment
    
    private GameObject spawnedPrefab;
    private static GameObject globalSpawnedPrefab; // Track prefab across all cubes
    private bool Paused = false;
    private Renderer cubeRenderer; // For visual feedback
    private Material originalMaterial; // Store original material

    void Start()
    {
        // Make sure the cube has a collider for mouse detection
        if (GetComponent<Collider>() == null)
        {
            Debug.LogWarning($"No collider found on {gameObject.name}. Adding BoxCollider for mouse detection.");
            gameObject.AddComponent<BoxCollider>();
        }
        
        // Store renderer for visual feedback
        cubeRenderer = GetComponent<Renderer>();
        if (cubeRenderer != null)
        {
            originalMaterial = cubeRenderer.material;
        }
        
        // Determine question index for this cube
        DetermineQuestionIndex();
        
        // Update visual state based on completion
        UpdateVisualState();
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
        
        // Check if this question is already completed
        if (assignedQuestionIndex >= 0 && GameStatsManager.Instance.IsQuestionCompleted(assignedQuestionIndex))
        {
            Debug.Log($"Question {assignedQuestionIndex} already completed! Cube click ignored for {gameObject.name}.");
            return; // Don't open task screen for completed questions
        }
        
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

    void DetermineQuestionIndex()
    {
        // Method 1: Use manually assigned index (set in inspector)
        if (assignedQuestionIndex >= 0)
        {
            Debug.Log($"Cube {gameObject.name} using manually assigned question index: {assignedQuestionIndex}");
            return;
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
    
    void UpdateVisualState()
    {
        if (cubeRenderer == null) return;
        
        // Change appearance if question is completed
        if (assignedQuestionIndex >= 0 && GameStatsManager.Instance.IsQuestionCompleted(assignedQuestionIndex))
        {
            if (completedMaterial != null)
            {
                cubeRenderer.material = completedMaterial;
            }
            else
            {
                // Default: Make it darker/greener to show completion
                Color completedColor = Color.green;
                completedColor.a = 0.7f;
                cubeRenderer.material.color = completedColor;
            }
        }
        else
        {
            cubeRenderer.material = originalMaterial;
        }
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

        // Get the correct question for THIS specific cube
        Question question = QuestionDatabase.All[assignedQuestionIndex];

        Debug.Log($"Cube {gameObject.name} opening question {assignedQuestionIndex}: {question.GetContext()}");

        // Pause interaction
        PauseInteraction();

        Action completionAction = () =>
        {
            Destroy(spawnedPrefab);
            ResumeInteraction();
            UpdateVisualState(); // Update appearance after completion
        };
        
        spawnedPrefab.GetComponent<TaskScreen>().Execute(question, completionAction, assignedQuestionIndex);
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
    
    // Public method to refresh visual state (call this when stats change)
    public void RefreshVisualState()
    {
        UpdateVisualState();
    }
}