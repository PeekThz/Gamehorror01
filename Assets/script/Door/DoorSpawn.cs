using UnityEngine;

public class DoorSpawn : MonoBehaviour
{
    public string spawnID;

    void Start()
    {
        // อ่านค่า spawn
        string targetSpawn =
            PlayerPrefs.GetString("SpawnPoint", "");

        Debug.Log("TARGET: " + targetSpawn);
        Debug.Log("MY ID: " + spawnID);

        // ไม่มีค่า → ไม่ต้องทำอะไร
        if (string.IsNullOrEmpty(targetSpawn))
            return;

        // ถ้าตรงกัน → ย้าย player
        if (targetSpawn == spawnID)
        {
            GameObject player =
                GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                player.transform.position =
                    transform.position;

                Debug.Log("PLAYER MOVED");
            }

            // 🔥 ล้างหลังใช้เสร็จ
            PlayerPrefs.DeleteKey("SpawnPoint");
        }
    }
}