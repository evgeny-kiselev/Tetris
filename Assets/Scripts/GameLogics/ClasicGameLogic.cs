using UnityEngine;
using Tetris.Factories;

namespace Tetris.Logic
{
    [RequireComponent(typeof(FigureFactory))]
    public class ClasicGameLogic : GameLogic
    {
        public override void Iteration()
        {
            if (!MoveCurentFigure(new Vector3(0, -1, 0)))
            {
                if (!CheckGameEnded())
                {
                    field.addFigure(currentFigure);
                    currentFigure = factory.createFigure(field.spawnPoint).transform;
                    CheckLine();
                }
            }
        }

        public override void StartGame()
        {
            currentFigure = factory.createFigure(field.spawnPoint).transform;
        }

        protected override void CheckLine()
        {
            for (var y = 0; y < field.height; y++)
            {
                bool lineCollected = true;
                for (var x = 0; x < field.width && lineCollected; x++)
                    if (field.getCeilObject(x, y) == null) lineCollected = false;
                if (lineCollected)
                {
                    AddScore(1);
                    field.removeLine(y);
                    y--;
                }
            }
        }

        protected override bool IsValidBlockPosition(Transform block, Vector3 moveVector)
        {
            var newPosition = (block.position + moveVector - transform.position).Round();
            var x = newPosition.x;
            var y = newPosition.y;
            if (y < 0) return false;
            if (x < 0 || x >= field.width) return false;
            if (field.getCeilObject(newPosition) != null) return false;
            return true;
        }

        public override bool MoveCurentFigure(Vector3 moveVector)
        {
            if (CanMoveFigure(currentFigure, moveVector))
            {
                currentFigure.position += moveVector;
                return true;
            }
            return false;
        }

    }
}
