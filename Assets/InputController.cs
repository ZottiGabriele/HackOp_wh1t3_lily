// GENERATED AUTOMATICALLY FROM 'Assets/InputController.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputController : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputController()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputController"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""35d24dc6-9b18-49fd-904b-2b04b4d16b1d"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""f563888a-511c-4211-b33a-e4aa1355bacb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""29e4ec71-d9eb-4d1e-a274-c1b8ada4cd97"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RunStart"",
                    ""type"": ""Value"",
                    ""id"": ""df81d7b1-c91a-4a3e-8e95-f9ac8f2f5d44"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RunStop"",
                    ""type"": ""Value"",
                    ""id"": ""e7dd06ee-fc81-4b9d-ae52-23ce0e9940b0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""b01d7e21-c5a4-4a3b-9609-3c5df1cba001"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ToggleMenu"",
                    ""type"": ""Button"",
                    ""id"": ""fefe7de0-4b87-46e3-9bad-7128a8435cad"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e05c28ab-8771-445f-a76a-7c396d79afc3"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""axis"",
                    ""id"": ""223a7064-4986-4612-9aec-d99d9c1aecee"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""77a41173-e397-4cb8-9bba-cc1b91b77944"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""0182c49a-9eb7-4b6c-9d56-ca349a7d4fcd"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""6af0f679-cf40-47b9-a5eb-1ca7bcb6819a"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""RunStart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d0de77cd-a403-4119-b26b-9ea1113760a6"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""882c3d78-16c6-4421-b44e-5dd6a7491d2f"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""RunStop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9c0e6485-813e-4753-81f6-1b7a35adda68"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""ToggleMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KeyboardAndMouse"",
            ""bindingGroup"": ""KeyboardAndMouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_RunStart = m_Player.FindAction("RunStart", throwIfNotFound: true);
        m_Player_RunStop = m_Player.FindAction("RunStop", throwIfNotFound: true);
        m_Player_Interact = m_Player.FindAction("Interact", throwIfNotFound: true);
        m_Player_ToggleMenu = m_Player.FindAction("ToggleMenu", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Jump;
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_RunStart;
    private readonly InputAction m_Player_RunStop;
    private readonly InputAction m_Player_Interact;
    private readonly InputAction m_Player_ToggleMenu;
    public struct PlayerActions
    {
        private @InputController m_Wrapper;
        public PlayerActions(@InputController wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @RunStart => m_Wrapper.m_Player_RunStart;
        public InputAction @RunStop => m_Wrapper.m_Player_RunStop;
        public InputAction @Interact => m_Wrapper.m_Player_Interact;
        public InputAction @ToggleMenu => m_Wrapper.m_Player_ToggleMenu;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @RunStart.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRunStart;
                @RunStart.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRunStart;
                @RunStart.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRunStart;
                @RunStop.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRunStop;
                @RunStop.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRunStop;
                @RunStop.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRunStop;
                @Interact.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @ToggleMenu.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleMenu;
                @ToggleMenu.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleMenu;
                @ToggleMenu.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleMenu;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @RunStart.started += instance.OnRunStart;
                @RunStart.performed += instance.OnRunStart;
                @RunStart.canceled += instance.OnRunStart;
                @RunStop.started += instance.OnRunStop;
                @RunStop.performed += instance.OnRunStop;
                @RunStop.canceled += instance.OnRunStop;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @ToggleMenu.started += instance.OnToggleMenu;
                @ToggleMenu.performed += instance.OnToggleMenu;
                @ToggleMenu.canceled += instance.OnToggleMenu;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_KeyboardAndMouseSchemeIndex = -1;
    public InputControlScheme KeyboardAndMouseScheme
    {
        get
        {
            if (m_KeyboardAndMouseSchemeIndex == -1) m_KeyboardAndMouseSchemeIndex = asset.FindControlSchemeIndex("KeyboardAndMouse");
            return asset.controlSchemes[m_KeyboardAndMouseSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnJump(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnRunStart(InputAction.CallbackContext context);
        void OnRunStop(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnToggleMenu(InputAction.CallbackContext context);
    }
}
