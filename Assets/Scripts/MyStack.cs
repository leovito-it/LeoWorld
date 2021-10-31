using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyStack : MonoBehaviour
{
    Stack<GameObject> stack = new Stack<GameObject>();
    [SerializeField] Color firstObjectColor = Color.blue;
    [SerializeField] Color midObjectColor = Color.yellow;
    [SerializeField] Color lastObjectColor = Color.red;
    [SerializeField] Color defaultColor = Color.white;
    public Vector3 changeMidObjectColor = new Vector3(0f,0f,0f);

    void Update_UI()
    {
        for (int i = 0; i < stack.Count; i++)
        {
            Color color = i == 0 ? firstObjectColor : 
                i == stack.Count - 1 ?  lastObjectColor : 
                new Color(midObjectColor.r -changeMidObjectColor.x*i, midObjectColor.g - changeMidObjectColor.y*i, midObjectColor.b - changeMidObjectColor.z*i);

            GameObject obj = GetObjectAtIndex(i);

            obj.GetComponentInChildren<Text>().text = (i).ToString();
            obj.GetComponentInChildren<Image>().color = color;
        }
    }

    public void Handle(GameObject selecting)
    {
        if (selecting.GetComponent<ButtonManager>().GetActive())
        {
            if (IsCallbackState(selecting))
            {
                Remove(1);
            }
        }
        else
        {
            PushObject(selecting);
        }   
    }

    bool IsCallbackState(GameObject selecting)
    {
        if (stack.Count >= 2)
        {
            GameObject lastObj, secondLastObj;

            lastObj = stack.Pop();
            secondLastObj = stack.Peek();

            // CHECK THIS OBJECT IS THE SECOND LAST ONE, IF TRUE, REMOVE THE LAST ONE FROM STACK
            if (selecting.GetInstanceID() == secondLastObj.GetInstanceID())
            {
                PushObject(lastObj);
                return true;
            }
            // ELSE PUSH IT OBJECT INTO STACK
            else
            {
                PushObject(lastObj);
                return false;
            }
        }
        else
            return false;
    }

    void PushObject(GameObject selecting)
    {
        stack.Push(selecting);
        selecting.GetComponent<ButtonManager>().SetActive(true);
        Update_UI();
    }

    public void Remove(int amout)
    {
        if (amout > stack.Count)
        {
            amout = stack.Count;
            Debug.LogError("Stack is empty now");
        }

        for (int i = 0; i < amout; i ++)
        {
            GameObject lastObj = stack.Pop();
            lastObj.GetComponent<ButtonManager>().SetActive(false);
            lastObj.GetComponentInChildren<Text>().text = Values.EMPTY_STRING;
            lastObj.GetComponentInChildren<Image>().color = defaultColor;
        }
        Update_UI();
    }

    public GameObject GetLastObject()
    {
        return stack.Count != 0 ? stack.Peek() : null;
    }

    public GameObject GetObjectAtIndex(int index)
    {
        if (index < 0 || stack.Count == 0)
            return null;

        return stack.ToArray()[stack.Count - 1 - index];
    }

    public Vector2 GetPositionOfObject(int index)
    {
        if (index < 0 || stack.Count == 0)
            return Vector2.zero;

        return GetObjectAtIndex(index).GetComponent<RectTransform>().position;
    }

    public Queue<GameObject> ToQueue()
    {
        Queue<GameObject> queue = new Queue<GameObject>();
        for (int i = 0; i < stack.Count; i++ )
        {
            queue.Enqueue(GetObjectAtIndex(i));
        }
        return queue;
    }

    public int Count()
    {
        return stack.Count;
    }

}
