using UnityEngine;
using System.Collections.Generic;

public class CollectDetector : MonoBehaviour
{
    public List<CollectibleItem> itemsInRange = new List<CollectibleItem>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        CollectibleItem item = other.GetComponent<CollectibleItem>();
        if (item != null && !itemsInRange.Contains(item))
        {
            itemsInRange.Add(item);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        CollectibleItem item = other.GetComponent<CollectibleItem>();
        if (item != null)
        {
            itemsInRange.Remove(item);
        }
    }

    public CollectibleItem GetNearestItem(Vector3 playerPos)
    {
        CollectibleItem nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var item in itemsInRange)
        {
            if (item == null) continue;

            float dist = Vector2.Distance(playerPos, item.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = item;
            }
        }

        return nearest;
    }
}