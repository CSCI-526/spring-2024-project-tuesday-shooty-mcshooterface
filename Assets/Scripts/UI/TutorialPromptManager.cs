using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class TutorialPromptManager : MonoBehaviour {
    public static TutorialPromptManager Instance;
    private InputActions _inputActions;

    // public constants
    public List<Prompt> allPrompts = new List<Prompt>();
    [Header("Complete Prompts On InputAction")]
    public Prompt completeOnLook;
    public Prompt completeOnMove;
    public Prompt completeOnShoot;

    // private state
    private List<Prompt> _queuedPrompts = new List<Prompt>();
    public Prompt ActivePrompt => _queuedPrompts.Count > 0 ? _queuedPrompts[0] : null;
    public UnityEvent onActivePromptChange;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one instance of TutorialPromptManager");
            return;
        }
        Instance = this;

        _inputActions = new InputActions();
        _inputActions.Enable();
    }

    private void Start()
    {
        _queuedPrompts.Clear();
        
        // queue starting prompts
        foreach (var prompt in allPrompts)
        {
            if (prompt.queueOnStart)
            {
                QueuePrompt(prompt);
            }
        }
        
        _inputActions.Player.Look.performed += OnLook;
        _inputActions.Player.Move.performed += OnMove;
        _inputActions.Player.Shoot.performed += OnShoot;
    }

    private void OnDisable()
    {
        _inputActions.Player.Look.performed -= OnLook;
        _inputActions.Player.Move.performed -= OnMove;
        _inputActions.Player.Shoot.performed -= OnShoot;
    }

    public bool TryQueuePrompt(Prompt promptToQueue)
    {
        foreach (var prompt in allPrompts)
        {
            if (prompt == promptToQueue && !_queuedPrompts.Contains(promptToQueue))
            {
                QueuePrompt(prompt);
                return true;
            }
        }
        return false;
    }

    public bool TryCompletePrompt(Prompt promptToComplete)
    {
        // if it's the active prompt
        if (promptToComplete == ActivePrompt)
        {
            allPrompts.Remove(promptToComplete);
            _queuedPrompts.Remove(promptToComplete);

            onActivePromptChange?.Invoke();
            
            return true;
        }

        // if it can be completed early
        foreach (var prompt in allPrompts)
        {
            if (prompt == promptToComplete && prompt.canBeCompletedEarly) {
                allPrompts.Remove(promptToComplete);
                
                // if it's queued
                if (_queuedPrompts.Contains(promptToComplete)) _queuedPrompts.Remove(promptToComplete);
                
                return true;
            }
        }

        return false;
    }
    
    private void QueuePrompt(Prompt prompt)
    {
        bool activePromptChanged = _queuedPrompts.Count == 0;

        _queuedPrompts.Add(prompt);

        if (activePromptChanged) onActivePromptChange?.Invoke();
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        TryCompletePrompt(completeOnLook);
    }
    private void OnMove(InputAction.CallbackContext context)
    {
        TryCompletePrompt(completeOnMove);
    }
    private void OnShoot(InputAction.CallbackContext context)
    {
        TryCompletePrompt(completeOnShoot);
    }
}