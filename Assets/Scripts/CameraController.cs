using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    
    //Camera zoom
    [SerializeField] private float maxZoomOut = 30.0f;
    [SerializeField] private float maxZoomIn = 5.0f;
    [SerializeField] private float defaultZoom = 5.0f;
    [SerializeField] private float zoomChangeAmount = 25.0f;

    //Camera Drag
    [SerializeField] private Vector2 camDragOrigin = Vector2.zero;
    [SerializeField] private float camMoveSpeed = 25.0f;
    [SerializeField] private float limitLeft = -10.0f, limitRight = 10.0f, limitUp = 10.0f, limitDown = -10.0f; //the boundaries where you can move the camera

    private void Awake()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        DragCamera();
        Zoom();
    }

    private void Zoom()
    {
        //Zoom in
        if (Input.mouseScrollDelta.y > 0)
        {
            cam.orthographicSize -= zoomChangeAmount * Time.deltaTime;

            if (cam.orthographicSize < maxZoomIn)
            {
                cam.orthographicSize = maxZoomIn;
            }
        }
        //Zoom out
        else if (Input.mouseScrollDelta.y < 0)
        {
            cam.orthographicSize += zoomChangeAmount * Time.deltaTime;

            if (cam.orthographicSize > maxZoomOut)
            {
                cam.orthographicSize = maxZoomOut;
            }
        }
    }

    //Moves the camera when you hold the middle mouse button
    private void DragCamera()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            camDragOrigin = Input.mousePosition;
        }

        if (Input.GetKey(KeyCode.Mouse2))
        {
            Vector2 pos = Camera.main.ScreenToViewportPoint((Vector2)Input.mousePosition - camDragOrigin);
            Vector2 move = new Vector2(pos.x * camMoveSpeed * Time.deltaTime, pos.y * camMoveSpeed * Time.deltaTime);
            
            //If you hit the top or bottom boundaries, don't let it move up or down
            if ((move.y > 0 && transform.position.y >= limitUp) ||
                (move.y < 0 && transform.position.y <= limitDown))
            {
                move.y = 0.0f;
            }

            if ((move.x > 0 && transform.position.x > limitRight) ||
                (move.x < 0 && transform.position.x < limitLeft))
            {
                move.x = 0.0f;
            }
            transform.Translate(move, Space.World);
        }
    }
}
