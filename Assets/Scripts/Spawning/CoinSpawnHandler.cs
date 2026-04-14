using UnityEngine;

public class CoinSpawnHandler : SpawnHandler
{
    // COIN FORMATIONS
    [System.Serializable]
    public struct CoinGridFormation
    {
        public string name;
        public int rows;
        public int cols;
        public float spacing;

        public CoinGridFormation(string name, int rows, int cols, float spacing)
        {
            this.name = name;
            this.rows = rows;
            this.cols = cols;
            this.spacing = spacing;
        }
    }

    // Initializes different coin grid formations
    public CoinGridFormation[] formations = new CoinGridFormation[]
    {
        new CoinGridFormation("2x3", 2, 3, 1.0f),
        new CoinGridFormation("2x4", 2, 4, 1.0f),
        new CoinGridFormation("3x3", 3, 3, 1.0f),
        new CoinGridFormation("3x4", 3, 4, 1.0f),
    };


    public override ObjectTags ObjectTag => ObjectTags.Coin;

    public override void Spawn(Vector3 position)
    {
        CoinGridFormation pickedFormation = PickCoinFormation();

        for (int row = 0; row < pickedFormation.rows; row++)
        {
            for (int col = 0; col < pickedFormation.cols; col++)
            {
                Vector3 pos = new Vector3(
                    position.x + (col * pickedFormation.spacing),
                    position.y + (row * pickedFormation.spacing),
                    position.z
                );

                ObjectPooler.Instance.SpawnFromPool(
                    ObjectTag.ToString(),
                    pos,
                    Quaternion.identity
                );
            }
        }
    }

    private CoinGridFormation PickCoinFormation()
    {
        return formations[Random.Range(0, formations.Length)];
    }
}
