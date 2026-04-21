using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [Header("Setari Labirint")]
    public int width = 11;
    public int height = 11;
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public Transform playerTransform;
    public float spawnExtraHeight = 0.05f;
    public float winTriggerHeight = 1f;

    private int[,] maze;
    // Tinem o referinta la podeaua de la (1,1) pentru spawn
    private GameObject spawnFloor;

    void Start()
    {
        int savedSize = PlayerPrefs.GetInt("MazeSize", 11);
        if (savedSize % 2 == 0) savedSize++;
        width = savedSize;
        height = savedSize;

        GenerateNewMaze();
    }

    public void GenerateNewMaze()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        spawnFloor = null;

        if (width % 2 == 0) width++;
        if (height % 2 == 0) height++;

        maze = new int[width, height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                maze[x, y] = 1;

        MakePassage(1, 1);

        maze[1, 0] = 0;
        maze[width - 2, height - 1] = 0;

        DrawMaze();

        if (playerTransform != null)
            playerTransform.gameObject.SetActive(false);

        StartCoroutine(SpawnPlayerDelayed());
    }

    IEnumerator SpawnPlayerDelayed()
    {
        // Asteptam pana la urmatorul pas de fizica, dupa instantiate-uri.
        yield return new WaitForFixedUpdate();

        if (playerTransform != null && spawnFloor != null)
        {
            Collider floorCollider = spawnFloor.GetComponent<Collider>();
            if (floorCollider == null)
            {
                floorCollider = spawnFloor.AddComponent<BoxCollider>();
            }

            float floorSurface = floorCollider.bounds.max.y;

            float playerHalfHeight = GetPlayerHalfHeight(playerTransform);

            float playerY = floorSurface + playerHalfHeight + spawnExtraHeight;

            playerTransform.gameObject.SetActive(true);

            Rigidbody rb = playerTransform.GetComponent<Rigidbody>();
            if (rb != null)
                rb.isKinematic = true;

            playerTransform.position = new Vector3(1f, playerY, 1f);
            Physics.SyncTransforms();

            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                yield return new WaitForFixedUpdate();
                rb.isKinematic = false;
            }
        }
    }

    float GetPlayerHalfHeight(Transform player)
    {
        if (player == null) return 0.5f;

        SphereCollider sphere = player.GetComponent<SphereCollider>();
        if (sphere != null)
            return sphere.radius * Mathf.Max(player.lossyScale.x, player.lossyScale.y, player.lossyScale.z);

        CapsuleCollider capsule = player.GetComponent<CapsuleCollider>();
        if (capsule != null)
            return Mathf.Max(capsule.height * 0.5f, capsule.radius) * player.lossyScale.y;

        BoxCollider box = player.GetComponent<BoxCollider>();
        if (box != null)
            return box.size.y * 0.5f * player.lossyScale.y;

        Collider genericCollider = player.GetComponent<Collider>();
        if (genericCollider != null)
            return Mathf.Max(genericCollider.bounds.extents.y, 0.5f);

        return 0.5f;
    }

    void MakePassage(int x, int y)
    {
        maze[x, y] = 0;

        int[] dirs = { 0, 1, 2, 3 };
        for (int i = 0; i < 4; i++)
        {
            int r = Random.Range(i, 4);
            int tmp = dirs[i]; dirs[i] = dirs[r]; dirs[r] = tmp;
        }

        for (int i = 0; i < 4; i++)
        {
            int dx = 0, dy = 0;
            if (dirs[i] == 0) dy = 2;
            else if (dirs[i] == 1) dy = -2;
            else if (dirs[i] == 2) dx = 2;
            else if (dirs[i] == 3) dx = -2;

            int nx = x + dx;
            int ny = y + dy;

            if (nx > 0 && nx < width - 1 && ny > 0 && ny < height - 1 && maze[nx, ny] == 1)
            {
                maze[x + (dx / 2), y + (dy / 2)] = 0;
                MakePassage(nx, ny);
            }
        }
    }

    void DrawMaze()
    {
        if (floorPrefab == null)
        {
            Debug.LogWarning("MazeGenerator: floorPrefab nu este asignat!");
            return;
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 floorPos = new Vector3(x, 0.45f, y);
                GameObject floor = Instantiate(floorPrefab, floorPos, Quaternion.identity, transform);
                floor.transform.localScale = new Vector3(1f, 0.1f, 1f);

                // Garantam collider pe fiecare tile de podea.
                if (floor.GetComponent<Collider>() == null)
                    floor.AddComponent<BoxCollider>();

                // Salvam referinta la podeaua de la pozitia (1,1) — spawn point
                if (x == 1 && y == 1)
                    spawnFloor = floor;

                // Triggerul de win este pe ultimul patratel sigur din interior,
                // inainte de iesirea de la margine.
                if (x == width - 2 && y == height - 2)
                    CreateWinTrigger(floor);

                if (maze[x, y] == 1)
                {
                    Vector3 wallPos = new Vector3(x, 1f, y);
                    Instantiate(wallPrefab, wallPos, Quaternion.identity, transform);
                }
            }
        }
    }

    void CreateWinTrigger(GameObject floor)
    {
        if (floor == null) return;

        GameObject triggerObject = new GameObject("WinTrigger");
        triggerObject.transform.SetParent(floor.transform, false);
        triggerObject.transform.localPosition = new Vector3(0f, winTriggerHeight * 0.5f, 0f);

        BoxCollider triggerCollider = triggerObject.AddComponent<BoxCollider>();
        triggerCollider.isTrigger = true;
        triggerCollider.size = new Vector3(0.85f, winTriggerHeight, 0.85f);

        triggerObject.AddComponent<WinTrigger>();
    }
}