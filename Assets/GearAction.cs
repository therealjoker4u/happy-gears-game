using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearAction : MonoBehaviour
{
    public float rotationSpeed;
    public float rotationMultiplier;
    public bool isSource = false;
    public bool isTarget = false;
    private bool isDragging = false;
    [HideInInspector]
    public bool isLocked = false;
    private Vector3 draggingStart;
    private Vector3 deltaDrag = Vector3.zero;
    private Rigidbody rigidBody;
    private Quaternion stopRotation;
    private bool canEmitCollision = true;
    private Material mainMaterial;
    private MeshRenderer renderer;

    void Start()
    {
        Vector3 gearPosition = transform.position;
        gearPosition.y = GameController.globalGearHeight;

        transform.position = gearPosition;

        rigidBody = GetComponent<Rigidbody>();
        rigidBody.constraints = RigidbodyConstraints.FreezeAll;

        renderer = GetComponent<MeshRenderer>();
        mainMaterial = renderer.materials[0];

    }

    private void OnMouseDown()
    {
        if (GameController.canDrag && !isSource && !isTarget)
        {
            isDragging = true;
            Vector3 dragging = CalculateDragging();
            draggingStart = dragging;
            deltaDrag = transform.position - dragging;

            stopRotation = transform.rotation;

            //// Changing Rigid Body constraints 
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

            canEmitCollision = false;

        }
    }

    private void OnMouseUp()
    {
        if(!isSource && !isTarget)
        {
            isDragging = false;

            //// Changing Rigid Body constraints 
            StartCoroutine(FreezeRigidbody());

            GameController.UpdateActions();

            canEmitCollision = true;
        }
    }

    IEnumerator FreezeRigidbody()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        rigidBody.constraints = RigidbodyConstraints.FreezeAll;
    }

    Vector3 CalculateDragging()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.y - transform.position.y;

        Vector3 newPos = Camera.main.ScreenToWorldPoint(mousePosition);

        newPos.y = transform.position.y;

        return newPos;
    }


    private void OnCollisionStay(Collision collision)
    {
        if (canEmitCollision && !isDragging && collision.gameObject.tag == "Gear")
        {
            GameController.ConnectGears(gameObject.name, collision.gameObject.name);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Gear")
        {
            GameController.DisconnectGears(gameObject.name, collision.gameObject.name);
        }
    }

    public void ChangeMaterialToUnexpected()
    {
        renderer.material = GameController.unexpectedMaterial;
    }

    public void ChangeMaterialToNormal()
    {
        renderer.material = mainMaterial;
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 newPos = CalculateDragging() + deltaDrag;

            transform.position = newPos;
            transform.rotation = stopRotation;

        }
        else if( rotationSpeed != 0)
        {
            transform.RotateAround(transform.position, Vector3.up, Time.deltaTime * rotationSpeed * rotationMultiplier);
        }
    }
}
