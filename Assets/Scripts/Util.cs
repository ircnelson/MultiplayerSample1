using UnityEngine;

public class Util
{
    public static void SetLayerRecursively(GameObject go, int layer)
    {
        if (go == null)
            return;

        go.layer = layer;

        foreach (Transform _child in go.transform)
        {
            if (_child == null)
                continue;

            SetLayerRecursively(_child.gameObject, layer);
        }
    }
}
