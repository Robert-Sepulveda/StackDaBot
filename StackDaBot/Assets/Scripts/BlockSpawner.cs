using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public GameObject SpawnBlock(GameObject prefab)
    {
        return Instantiate(prefab, transform.position, transform.rotation);
    }
    public void KillBlock(GameObject prefab)
    {
        Destroy(prefab);
    }
}
