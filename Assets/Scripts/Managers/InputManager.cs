using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    public PlayerInput playerInput;
    
    [Range(0.0f, 0.5f)] public float mouseSmoothTime = 0.03f;
    [HideInInspector] public Vector2 currentMouseDelta = Vector2.zero;
    [HideInInspector] public Vector2 MovementVector;
    private bool updateMouseDelta = true;
    
    protected override void Awake()
    {
        playerInput = new PlayerInput();
        LoadBindingOverrides();
    }
    
    private void FixedUpdate()
    {
        MovementVector = playerInput.InGame.Movement.ReadValue<Vector2>();
    }
    
    public void LoadBindingOverrides()
    {
        if (PlayerPrefs.HasKey("rebinds"))
        {
            string json = PlayerPrefs.GetString("rebinds");
    
            // Apply the rebinds to the action asset
            playerInput.asset.LoadBindingOverridesFromJson(json);
        }
    }
    
    private void OnEnable()
    {
        playerInput.Enable();
    }
    
    private void OnDisable()
    {
        playerInput.Disable();
    }
    
    public void DisableInGameInput()
    {
        updateMouseDelta = false;
        currentMouseDelta = Vector2.zero;
        playerInput.InGame.Disable();
    }
    
    public void EnableInGameInput()
    {
        updateMouseDelta = true;
        playerInput.InGame.Enable();
    }
}
