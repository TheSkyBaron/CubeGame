using Game.Data;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public class PlayerWorkbenchController : MonoBehaviour
    {
        private InputActionMap InputMap;
        private InputAction Move, Look, Zoom, Hold, Place, RemoveSwitch;
        private Rigidbody PlayerBody;
        private Camera PlayerCamera;
        private GameObject GhostObject;
        private Material GhostMaterial;
        private GameObject ActiveWorkbench;

        private Vector3 PlayerMovementVector;
        private Vector2 PlayerCurrentRotation;
        private float PlayerCurrentZoom;
        private bool Is_PlayerHoldingScreen;
        private bool Is_Removing;
        private Vector3 HitPointPlace;
        private Vector3 HitPointRemove;

        private readonly float PlayerMinZoom = 0;
        private readonly float PlayerMaxZoom = 100;
        private readonly float FloatErrorPointAdjustment = 0.01f;
        private readonly Vector3 GhostPlacementTrim = new(0.5f, 0.5f, 0.5f);

        private void Start()
        {
            PlayerCurrentZoom = PlayerMaxZoom / 2;
            PlayerBody = gameObject.GetComponent<Rigidbody>();
            GhostObject = GameObject.FindGameObjectWithTag("GhostObject");
            GhostMaterial = GhostObject.GetComponent<MeshRenderer>().sharedMaterial;
            ActiveWorkbench = GameObject.FindGameObjectWithTag("Workbench");
            PlayerCamera = Camera.main;
          
        }

        private void OnEnable()
        {
            InputMap = gameObject.GetComponent<PlayerInput>().currentActionMap;

            //Action Map

            Move = InputMap.FindAction("Move");
            Look = InputMap.FindAction("Look");
            Zoom = InputMap.FindAction("Zoom");
            Hold = InputMap.FindAction("Hold");
            Place = InputMap.FindAction("PlaceBlock");
            RemoveSwitch = InputMap.FindAction("RemoveSwitch");

            //Movement Controlls

            Move.started += PlayerMove;
            Move.performed += PlayerMove;
            Move.canceled += PlayerMove;

            //Camera Controlls

            Look.started += PlayerLook;
            Look.performed += PlayerLook;
            Look.canceled += PlayerLook;

            //Zoom Controlls

            Zoom.started += PlayerZoom;

            //Player Mouse Controlls

            Hold.started += PlayerHoldStart;
            Hold.canceled += PlayerHoldEnd;

            //PlayerCursorControlls
            RemoveSwitch.performed += PlayerRemovePlaceSwitch;
            Place.performed += PlayerPlaceBlock;
        }

        private void OnDisable()
        {

            //Movement Controlls

            Move.started -= PlayerMove;
            Move.performed -= PlayerMove;
            Move.canceled -= PlayerMove;

            //Camera Controlls

            Look.started -= PlayerLook;
            Look.performed -= PlayerLook;
            Look.canceled -= PlayerLook;

            //Zoom Controlls

            Zoom.started -= PlayerZoom;

            //Player Mouse Controlls

            Hold.started -= PlayerHoldStart;
            Hold.canceled -= PlayerHoldEnd;

            //PlayerCursorControlls
            RemoveSwitch.performed -= PlayerRemovePlaceSwitch;
            Place.performed -= PlayerPlaceBlock;
        }

        private void FixedUpdate()
        {
            PlayerBody.AddForce((transform.right * PlayerMovementVector.x) + (transform.forward * PlayerMovementVector.z), ForceMode.VelocityChange);
            PlayerCamera.transform.localPosition = new Vector3(0, 0, -PlayerCurrentZoom);
            PlayerBody.transform.localEulerAngles = new Vector3(PlayerCurrentRotation.y, PlayerCurrentRotation.x, 0);
            BlockPlacementSystem();
        }

        private void PlayerMove(InputAction.CallbackContext CTX)
        {
            Vector2 Temp = CTX.ReadValue<Vector2>();
            PlayerMovementVector = new Vector3(Temp.x, 0, Temp.y);
        }

        private void PlayerZoom(InputAction.CallbackContext CTX)
        {
            PlayerCurrentZoom -= CTX.ReadValue<Vector2>().y * 0.5f;
            PlayerCurrentZoom = Mathf.Clamp(PlayerCurrentZoom, PlayerMinZoom, PlayerMaxZoom);
        }

        private void PlayerLook(InputAction.CallbackContext CTX)
        {
            if (Is_PlayerHoldingScreen) PlayerCurrentRotation += CTX.ReadValue<Vector2>() * PlayerSettings.MouseSensevity;
        }

        private void PlayerHoldStart(InputAction.CallbackContext CTX)
        {
            Is_PlayerHoldingScreen = true;
        }

        private void PlayerHoldEnd(InputAction.CallbackContext CTX)
        {
            Is_PlayerHoldingScreen = false;
        }

        private void PlayerPlaceBlock(InputAction.CallbackContext CTX)
        {
            Workbench Current = ActiveWorkbench.GetComponent<Workbench>();
            if (Is_Removing && HitPointRemove != -Vector3.one)
            {
                int3 RemovePoint = new((int)HitPointRemove.x,(int)HitPointRemove.y,(int)HitPointRemove.z);
                Current.GridSystem.SetBlock(RemovePoint, 0);
            }
            else if(HitPointPlace != -Vector3.one)
            {
                int3 PlacePoint = new((int)HitPointPlace.x, (int)HitPointPlace.y, (int)HitPointPlace.z);
                Current.GridSystem.SetBlock(PlacePoint, 1);
            }
            Current.RenderWorkbench();
        }

        private void PlayerRemovePlaceSwitch(InputAction.CallbackContext CTX)
        {
            Is_Removing = !Is_Removing;
        }

        private void BlockPlacementSystem()
        {
            Ray MouseRaycast = PlayerCamera.ScreenPointToRay(Mouse.current.position.ReadValue(), Camera.MonoOrStereoscopicEye.Mono);
            if (Physics.Raycast(MouseRaycast, out RaycastHit Hit, 1000))
            {
                if (!Is_Removing)
                {
                    Vector3 GridPoint = (Hit.point - Hit.transform.position) + (Hit.normal * FloatErrorPointAdjustment);
                    HitPointPlace = new Vector3(Mathf.FloorToInt(GridPoint.x), Mathf.FloorToInt(GridPoint.y), Mathf.FloorToInt(GridPoint.z));
                    PlaceGhostObject(HitPointPlace + GhostPlacementTrim, Vector3.one, new Color(1, 1, 1, 0.5f));
                }
                else
                {
                    Vector3 GridPoint = (Hit.point - Hit.transform.position) - (Hit.normal * FloatErrorPointAdjustment);
                    HitPointRemove = new Vector3(Mathf.FloorToInt(GridPoint.x), Mathf.FloorToInt(GridPoint.y), Mathf.FloorToInt(GridPoint.z));
                    PlaceGhostObject(HitPointRemove + GhostPlacementTrim, new Vector3(1.1f, 1.1f, 1.1f), new Color(1, 0, 0, 0.5f));
                }
            }
            else
            {
                HitPointPlace = -Vector3.one;
                HitPointRemove = -Vector3.one;
                GhostObject.SetActive(false);
            }
        }

        private void PlaceGhostObject(Vector3 Position,Vector3 Scale,Color GhostColor)
        {
            GhostObject.SetActive(true);
            GhostObject.transform.position = Position;
            GhostObject.transform.localScale = Scale;
            GhostMaterial.color = GhostColor;
        }
    }
}