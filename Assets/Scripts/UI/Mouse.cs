using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Mouse : MonoBehaviour
{
    [Range(1, 100)]
    public float mouseSize = 10f;

    Touch touch = new Touch();
    GameController controller;
    Vector3 screenSize;

    void Start()
    {
        GetObjectAndStats();
        SetupThePointer();
    }

    void GetObjectAndStats()
    {
        controller = FindObjectOfType<GameController>();
        screenSize = (Vector3)GameObject.FindGameObjectWithTag(Values.MAIN_CANVAS).GetComponent<RectTransform>().sizeDelta;
    }

    void SetupThePointer()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0f;

        CircleCollider2D collider2D = GetComponent<CircleCollider2D>();
        collider2D.isTrigger = true;
        collider2D.radius = mouseSize;
    }

    // Update is called once per frame
    void Update()
    {
        GetStats();
        Handle();
    }

    void Handle()
    {
        if (controller.mouseDragging)
        {
            transform.localPosition = Input.mousePosition - screenSize / 2;
        }
    }

    void OnMouseDrag()
    {
        Debug.Log("OnMouseDrag");
    }

    void GetStats()
    {
        if (Input.touchSupported)
            touch = Input.GetTouch(0);

        if (Input.GetMouseButtonDown(0) || touch.phase == TouchPhase.Moved)
        {
            controller.mouseDragging = true;
        }

        if (Input.GetMouseButtonUp(0) || touch.phase == TouchPhase.Ended)
        {
            if (controller.mouseDragging)
                controller.clearStack = true;

            controller.mouseDragging = false;
        }
    }

}
