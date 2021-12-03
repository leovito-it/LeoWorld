using UnityEngine;

public class DistanceCalculator
{
    GameObject NearestObject()
    {
        GameObject result = null;
        //code here

        return result;
    }

    public static bool IsSameAxis(GameObject obj1, GameObject obj2)
    {
        if (obj1 == null || obj2 == null)
            return true;

        Vector2 distance = obj1.transform.position - obj2.transform.position;

        return  (distance.x == 0) || (distance.y == 0 ) ?  true : false;
    }

    public static bool IsObjectInCircle(GameObject obj1, GameObject obj2, float range)
    {
        if (obj1 == null || obj2 == null)
            return true;

        Vector2 distance = obj1.transform.localPosition - obj2.transform.localPosition;


        return (distance.magnitude <= range) ? true : false;
    }

    public static bool IsObjectInBox(GameObject obj1, GameObject obj2, Vector2 range)
    {
        if (obj1 == null || obj2 == null)
            return true;

        Vector2 distance = obj1.transform.localPosition - obj2.transform.localPosition;

        return (distance.x <= range.x) && (distance.y <= range.y) ? true : false;
    }
}
