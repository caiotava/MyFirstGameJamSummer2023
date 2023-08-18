using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameControl : MonoBehaviour
{
    [SerializeField] private InputAction mouseLeftControls;
    [SerializeField] private InputAction mouseRightControls;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private Transform selectArea;
    [SerializeField, Range(5, 200)] private float selectAreaScale;

    private Camera mainCamera;
    private Vector3 startMousePosition;
    private List<Unit> selectedUnits;

    private void Awake()
    {
        selectArea.gameObject.SetActive(false);
        mainCamera = Camera.main;
        selectedUnits = new List<Unit>();
    }

    private void OnEnable()
    {
        mouseLeftControls.Enable();
        mouseRightControls.Enable();
        mouseLeftControls.performed += MouseLeftPressed;
        mouseRightControls.performed += MouseRightPressed;
    }

    private void OnDisable()
    {
        mouseLeftControls.Disable();
        mouseRightControls.Disable();
        mouseLeftControls.performed -= MouseLeftPressed;
        mouseRightControls.performed -= MouseRightPressed;
        selectedUnits.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Mouse.current.leftButton.isPressed)
        {
            return;
        }

        var worldMousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var scale = (worldMousePosition - startMousePosition) * selectAreaScale;

        selectArea.position = new Vector3(startMousePosition.x, startMousePosition.y, selectArea.position.z);
        selectArea.localScale = new Vector3(scale.x, scale.y, selectArea.localScale.z);
    }

    private void MouseLeftPressed(InputAction.CallbackContext context)
    {
        var worldMousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        if (Mouse.current.leftButton.isPressed)
        {
            startMousePosition = worldMousePosition;
            selectArea.gameObject.SetActive(true);
            return;
        }

        var colliders = Physics2D.OverlapAreaAll(startMousePosition, worldMousePosition, playerLayerMask);

        foreach (var unit in selectedUnits)
        {
            unit.SetSelected(false);
        }

        selectedUnits.Clear();

        foreach (var collider2D in colliders)
        {
            var unit = collider2D.GetComponent<Unit>();

            if (unit is null)
            {
                continue;
            }

            unit.SetSelected(true);
            selectedUnits.Add(unit);
        }
        
        selectArea.gameObject.SetActive(false);
    }

    private void MouseRightPressed(InputAction.CallbackContext context)
    {
        var worldPosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var targetPositionList = GetPositionListAround(worldPosition, new float[]{1f, 2f, 3f}, new int[]{5, 10, 15});

        var targetPositionIndex = 0;
        foreach (var unit in selectedUnits)
        {
            var moveToPosition = targetPositionList[targetPositionIndex];
            unit.SetMovePosition(new Vector3(moveToPosition.x, moveToPosition.y, unit.transform.position.z));
            targetPositionIndex = (targetPositionIndex + 1) % targetPositionList.Count;
        }
    }

    private static List<Vector3> GetPositionListAround(Vector3 startPosition, float distance, int positionCount)
    {
        var positionList = new List<Vector3>();
        for (var i = 0; i < positionCount; i++)
        {
            var angle = i * (360f / positionCount);
            var dir = Quaternion.Euler(0, 0, angle) * new Vector3(1, 0);
            var position = startPosition + dir * distance;
            positionList.Add(position);
        }

        return positionList;
    }

    private static List<Vector3> GetPositionListAround(Vector3 startPosition, float[] ringDistanceArray,
        int[] ringPositionCount)
    {
        var positionList = new List<Vector3>();
        for (var i = 0; i < ringDistanceArray.Length; i++)
        {
            positionList.AddRange(GetPositionListAround(startPosition, ringDistanceArray[i], ringPositionCount[i]));
        }

        return positionList;
    }
}