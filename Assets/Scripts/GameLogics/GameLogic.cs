using UnityEngine;
using Tetris.Factories;
using Tetris.Fields;
using Tetris.Figures;

namespace Tetris.Logic
{
    [RequireComponent(typeof(FigureFactory))]
    public abstract class GameLogic : MonoBehaviour
    {
        protected Transform currentFigure;
        protected Field field;
        protected FigureFactory factory;
        protected Game game;

        public void Awake()
        {
            game = GetComponent<Game>();
            factory = GetComponent<FigureFactory>();
            field = GetComponent<Field>();
        }

        public abstract void startGame();

        public abstract void iteration();

        protected abstract void checkLine();

        protected virtual void addScore(int score)
        {
            game.score += score;
        }

        protected abstract bool isValidBlockPosition(Transform block, Vector3 moveVector);

        public bool canMoveFigure(Transform figure, Vector3 moveVector)
        {
            foreach (Transform block in figure)
                if (!isValidBlockPosition(block, moveVector)) return false;
            return true;
        }

        public abstract bool moveCurentFigure(Vector3 moveVector);

        public virtual void rotateCurrentFigure()
        {
            rotateFigure(currentFigure);
        }

        public virtual void rotateFigure(Transform figure)
        {
            var center = figure.position + figure.GetComponent<Figure>().getCenter();
            foreach (Transform block in figure)
            {
                block.RotateAround(center, Vector3.forward, 90);
                block.localEulerAngles = Vector3.zero;
                block.position = block.position.Round();
            }
            if (!canMoveFigure(figure, Vector3.zero)) rotateCurrentFigure();
        }
    }
}
