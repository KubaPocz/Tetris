using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public GameObject[] blocks;
    public GameObject[] spawnPoints;

    private GameObject _spawnedBlock;
    private TetrisController _spawnedBlockTetrisController;

    public GridManager[] gridManager;

    private void Awake()
    {
        // (Optional) Setup logic if needed later
    }

    private void Start()
    {
        // Set initial next blocks for each player
        gridManager[0].nextBlock = blocks[Random.Range(0, blocks.Length)];
        gridManager[1].nextBlock = blocks[Random.Range(0, blocks.Length)];

        // Spawn first block for each player
        SpawnBlock(1);
        SpawnBlock(2);

        // Delay next block UI update to ensure proper setup
        Invoke(nameof(SendNextBlockUpdate), 0.5f);
    }

    // Send next block preview update to UI
    private void SendNextBlockUpdate()
    {
        Events.UpdateNextBlock(1, gridManager[0].nextBlock);
        Events.UpdateNextBlock(2, gridManager[1].nextBlock);
    }

    // Spawn a block for the given player
    public void SpawnBlock(int playerId)
    {
        GameObject blockToSpawn;

        switch (playerId)
        {
            case 1:
                blockToSpawn = gridManager[0].nextBlock;
                _spawnedBlock = Instantiate(
                    blockToSpawn,
                    spawnPoints[0].transform.position,
                    Quaternion.identity
                );
                _spawnedBlock.GetComponent<TetrisController>().Initialize(1);
                gridManager[0].nextBlock = blocks[Random.Range(0, blocks.Length)];
                Events.UpdateNextBlock(1, gridManager[0].nextBlock);
                break;

            case 2:
                blockToSpawn = gridManager[1].nextBlock;
                _spawnedBlock = Instantiate(
                    blockToSpawn,
                    spawnPoints[1].transform.position,
                    Quaternion.identity
                );
                _spawnedBlock.GetComponent<TetrisController>().Initialize(2);
                gridManager[1].nextBlock = blocks[Random.Range(0, blocks.Length)];
                Events.UpdateNextBlock(2, gridManager[1].nextBlock);
                break;

            default:
                break;
        }
    }
}
