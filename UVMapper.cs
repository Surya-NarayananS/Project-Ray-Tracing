using UnityEngine;

public class UVMapper : MonoBehaviour
{
    Vector3 lightColor = new Vector3(0.9f, 0.9f, 0.9f);
    Vector3 darkColor = new Vector3(0f, 0f, 0f);
    float chekersWidth = 20; //20
    float chekersHeight = 10; //10

    public Vector3 UVMap(Vector3 unitVectorToCenter)
    {
        float u, v, u1, v1;

        u = 0.5f + (Mathf.Atan2(unitVectorToCenter.x, unitVectorToCenter.z) / (2 * Mathf.PI));
        v = 0.5f - (Mathf.Asin(unitVectorToCenter.y) / Mathf.PI);

        u1 = Mathf.Floor(u * chekersWidth);
        v1 = Mathf.Floor(v * chekersHeight);

        if(((u1 + v1)%2) == 0)
        {
            return darkColor;
        }
        else
        {
            return lightColor;
        }

    }
}
