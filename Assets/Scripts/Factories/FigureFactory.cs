using UnityEngine;

namespace Tetris.Factories
{
    public class FigureFactory : MonoBehaviour
    {
        public GameObject[] figures;
        public int[] chances;

        public virtual GameObject createFigure(Vector3 spawn)
        {
            var value = Random.Range(0, 100);
            int i = -1;
            do
            {
                i++;
                value -= chances[i];
            } while (value > 0);
            var figure = Instantiate(figures[i], transform);
            figure.transform.position = spawn;
            return figure;
        }
    }
}