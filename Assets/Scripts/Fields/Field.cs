using UnityEngine;

namespace Tetris.Fields
{
    [ExecuteInEditMode]
    public class Field : MonoBehaviour
    {
        public int width = 10, height = 20;
        public GameObject wallBlock;
        protected Transform[,] map;
        public Vector3 spawnPoint { get; private set; }

        public void Update()
        {
            if (Application.isEditor)
            {
                var walls = transform.Find("WALLS");
                if (walls) DestroyImmediate(walls.gameObject);
                createWalls();
            }
        }

        public void Awake()
        {
            map = new Transform[width, height];
            spawnPoint = new Vector3(width / 2, height, 1);
            createWalls();
        }

        protected virtual void createWalls()
        {
            if (wallBlock == null) return;
            var walls = transform.Find(("WALLS"));
            if (walls == null) walls = new GameObject("WALLS").transform;
            walls.parent = transform;
            for (var y = transform.position.y; y < transform.position.y + height; y++)
            {
                var blockLeft = Instantiate(wallBlock, walls.transform);
                var blockRight = Instantiate(wallBlock, walls.transform);

                blockLeft.transform.position = new Vector3(transform.position.x - 1, transform.position.y + y, transform.position.z);
                blockRight.transform.position = new Vector3(transform.position.x + width, transform.position.y + y, transform.position.z);
            }
            for (var x = transform.position.x - 1; x <= transform.position.x + width; x++)
            {
                var blockBottom = Instantiate(wallBlock, walls.transform);
                blockBottom.transform.position = new Vector3(x, transform.position.y - 1, transform.position.z);
            }
        }

        public void addFigure(Transform figure)
        {
            foreach (Transform block in figure)
            {
                var blockPosition = (figure.localPosition + block.localPosition).Round();
                var x = (int)blockPosition.x;
                var y = (int)blockPosition.y;
                map[x, y] = block;
            }
        }

        public void removeLine(int y)
        {
            for (var x = 0; x < width; x++) removeBlock(x, y);
            for (; y < height - 1; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    if (map[x, y + 1] != null) map[x, y + 1].position += new Vector3(0, -1, 0);
                    map[x, y] = map[x, y + 1];

                }
            }
        }

        public Transform getCeilObject(Vector3 position)
        {
            return getCeilObject((int)position.x, (int)position.y);
        }

        public void removeBlock(int x, int y)
        {
            var block = map[x, y];
            if (block.parent.childCount < 2) DestroyImmediate(block.parent.gameObject);
            else DestroyImmediate(block.gameObject);
            map[x, y] = null;
        }

        public Transform getCeilObject(int x, int y)
        {
            if (x >= 0 && x < width)
                if (y >= 0 && y < height)
                    return map[x, y];
            return null;
        }
    }
}
