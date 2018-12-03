using UnityEngine;

namespace Tetris.Figures
{
    public class Figure : MonoBehaviour
    {
        private new Transform transform;
        private Vector3 center;

        void Start()
        {
            transform = GetComponent<Transform>();
            calculateCenter(out center);
        }

        public Vector3 getCenter()
        {
            return center;
        }

        private void calculateCenter(out Vector3 center)
        {
            float minX = 0f;
            float maxX = 0f;
            float minY = 0f;
            float maxY = 0f;
            foreach (Transform block in transform)
            {
                var posX = block.localPosition.x;
                var posY = block.localPosition.y;

                if (posX > maxX) maxX = posX;
                if (posX < minX) minX = posX;
                if (posY < minY) minY = posY;
                if (posY > maxY) maxY = posY;
            }
            center = new Vector3((maxX - minX) / 2f, (maxY - minY) / 2f, 0).Round();
        }
    }
}