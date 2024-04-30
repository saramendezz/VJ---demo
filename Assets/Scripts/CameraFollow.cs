using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Camera cam02;
    Vector3 offset;
    bool isRuning;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.position;
        Quaternion rotation = Quaternion.Euler(-20f, 132f, 0f);
        transform.SetPositionAndRotation(new Vector3(-2f, 0.5f, 9f), rotation);
        isRuning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRuning)
        {
            Vector3 targetPos = player.position + offset;
            targetPos.x = 0;
            transform.position = targetPos;
        }
    }

    public void setRuning()
    {
        offset = new Vector3(0f, 5f, -5);
        isRuning=true;
    }
}

