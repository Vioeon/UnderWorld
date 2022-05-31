using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform playerTransform;
    [SerializeField]
    Vector3 cameraPosition;

    [SerializeField]
    Vector2 center;
    [SerializeField]
    Vector2 mapSize;

    [SerializeField]
    float cameraMoveSpeed;
    float height;
    float width;

    bool boo = false;

    void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();

        //Invoke("setplayer", 0.2f);

        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;
    }/*
    public void setplayer()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }*/
    void FixedUpdate()
    {
        /*if (boo == false)
        {
            boo = true;
            Invoke("LimitCameraArea", 0.3f);
            
        }
        else
            LimitCameraArea();*/
        LimitCameraArea();
    }

    void LimitCameraArea()
    {
        transform.position = Vector3.Lerp(transform.position,
                                          playerTransform.position + cameraPosition,
                                          Time.deltaTime * cameraMoveSpeed);
        float lx = mapSize.x - width;
        float clampX = Mathf.Clamp(transform.position.x, -lx + center.x, lx + center.x);

        float ly = mapSize.y - height;
        float clampY = Mathf.Clamp(transform.position.y, -ly + center.y, ly + center.y);

        transform.position = new Vector3(clampX, clampY, -10f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, mapSize * 2);
    }
}