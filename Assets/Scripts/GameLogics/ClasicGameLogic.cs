using UnityEngine;
using Tetris.Factories;

namespace Tetris.Logic
{
    [RequireComponent(typeof(FigureFactory))]
    public class ClasicGameLogic : GameLogic
    {
        ///See <see cref="GameLogic.Iteration"/>
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

        ///See <see cref="GameLogic.StartGame"/>
        public override void StartGame()
        {
            currentFigure = factory.createFigure(field.spawnPoint).transform;
        }

        ///See <see cref="GameLogic.CheckLine"/>
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

        ///See <see cref="GameLogic.IsValidBlockPosition(Transform, Vector3)"/>
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

        ///See <see cref="GameLogic.MoveCurentFigure(Vector3)"/>
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
