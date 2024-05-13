using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Camera cam02;
    Vector3 offset;
    bool isRuning;
    bool isXdirection;
    bool isNegative;
    float defaultDirection;

    // TEST SMOOTH ROTATION
    private bool isRotatingL;
    private bool isRotatingR;
    private float rotationSpeed = 4.0f;
    private Quaternion targetRotation;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.position;
        Quaternion rotation = Quaternion.Euler(-20f, 132f, 0f);
        transform.SetPositionAndRotation(new Vector3(-2f, 0.5f, 9f), rotation);
        isRuning = false;
        isXdirection = true;
        isNegative = true;
        defaultDirection = 0f;

        isRotatingL = false;
        isRotatingR = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRuning)
        {
            Vector3 targetPos = player.position + offset;
            if (isXdirection)
            {
                targetPos.x = defaultDirection;
                if (isNegative) targetPos.z += -5;
                else targetPos.z += 5;
            }
            else
            {

                targetPos.z = defaultDirection;
                if (isNegative) targetPos.x += -5;
                else targetPos.x += 5;
            }
            transform.position = targetPos;

            if (isRotatingL) RotateCameraSmoothly();

            if (isRotatingR) RotateCameraSmoothly();
        }
    }

    public void setRuning()
    {
        offset = new Vector3(0f, 5f, 0f);
        isRuning=true;
    }

    public void turnCamera(bool rotationRight, Vector3 vecDefaultDir)
    {
        // Quaternion rotation = transform.rotation;
        // transform.GetPositionAndRotation(transform.position, rotation);
        if (isXdirection) 
        {
            defaultDirection = vecDefaultDir.z;
            if (isNegative)
            {
                if (rotationRight) isNegative = true;
                else isNegative = false;
            }
            else
            {
                if (rotationRight) isNegative = false;
                else isNegative = true;
            }
        }
        else
        {
            defaultDirection = vecDefaultDir.x;
            if (isNegative)
            {
                if (rotationRight) isNegative = false;
                else isNegative = true;
            }
            else
            {
                if (rotationRight) isNegative = true;
                else isNegative = false;
            }
        }
        isXdirection =!isXdirection;

        if (rotationRight)
        {
            isRotatingL = false; isRotatingR = true;
        }
        else
        {
            isRotatingL = true; isRotatingR = false;
        }
        float rotation = rotationRight ? 90f : -90f;
        targetRotation = Quaternion.Euler(0, rotation, 0) * transform.rotation;
        // targetRotation = transform.rotation * Quaternion.Euler(0, rotationRight ? 90f : -90f, 0);
        
        /*
        float rotation = rotationRight ? 90f : -90f;
        Quaternion newRotation = Quaternion.Euler(0, rotation, 0) * transform.rotation;

        // Apply the new rotation to the main camera
        Camera.main.transform.rotation = newRotation;
         */


        //transform.rotation.SetEulerRotation(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + rotation, transform.rotation.eulerAngles.z));
    }

    private void RotateCameraSmoothly()
    {
        // Calculate the rotation step based on the rotation speed and time
        float step = rotationSpeed * Time.deltaTime;

        // Rotate the camera towards the target rotation
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, step);

        // Check if the rotation is close enough to the target rotation to stop
        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            // Reset rotation flag
            isRotatingL = false;
            isRotatingR = false;
        }
    }
}

