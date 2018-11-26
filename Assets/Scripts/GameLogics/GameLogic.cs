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

        /// <summary>
        /// Инициализация начала игры
        /// </summary>
        public abstract void StartGame();

        /// <summary>
        /// Действия в течении каждого хода игры.
        /// </summary>
        public abstract void Iteration();
        
        /// <summary>
        /// Поиск и удаление собранных линий. 
        /// </summary>
        protected abstract void CheckLine();

        /// <summary>
        /// Добавляет количество очков игроку
        /// </summary>
        /// <param name="score">Кол-во очков</param>
        protected virtual void AddScore(int score)
        {
            game.Score += score;
        }

        /// <summary>
        /// Проверка на окончание игры. Если игра окончена, цкл останавливается.
        /// </summary>
        /// <returns>true - если игра закончилась, иначе false</returns>
        protected bool CheckGameEnded()
        {
            foreach (Transform block in currentFigure)
                if (block.position.y >= transform.position.x + field.height)
                {
                    game.isGameEnded = true;
                    return true;
                }

            return false;
        }

        /// <summary>
        /// Проверка, можно ли переместить блок.
        /// </summary>
        /// <param name="block">Перемещаемый блок</param>
        /// <param name="moveVector">Вектор перемещения</param>
        /// <returns>true - если можно, иначе false</returns>
        protected abstract bool IsValidBlockPosition(Transform block, Vector3 moveVector);

        /// <summary>
        /// Проверка, можно ли переместить фигуру.
        /// </summary>
        /// <param name="figure">Перемещаемая фигура</param>
        /// <param name="moveVector">Вектор перемещения</param>
        /// <returns>true - если можно, иначе false</returns>
        /// <seealso cref="IsValidBlockPosition(Transform, Vector3)"/> 
        public bool CanMoveFigure(Transform figure, Vector3 moveVector)
        {
            foreach (Transform block in figure)
                if (!IsValidBlockPosition(block, moveVector)) return false;
            return true;
        }

        /// <summary>
        /// Перемещение текущей фигура на вектор.
        /// </summary>
        /// <param name="moveVector">Вектор перемещения</param>
        /// <returns>true - если фигура перемещена, иначе false</returns>
        public abstract bool MoveCurentFigure(Vector3 moveVector);

        /// <summary>
        /// Поворот текущей фигуры
        /// </summary>
        /// <seealso cref="RotateFigure(Transform)"/>
        public virtual void RotateCurrentFigure()
        {
            RotateFigure(currentFigure);
        }

        /// <summary>
        /// Поворот фигуры
        /// </summary>
        /// <param name="figure">Поворачиваемая фигура</param>
        public virtual void RotateFigure(Transform figure)
        {
            var center = figure.position + figure.GetComponent<Figure>().getCenter();
            foreach (Transform block in figure)
            {
                block.RotateAround(center, Vector3.forward, 90);
                block.localEulerAngles = Vector3.zero;
                block.position = block.position.Round();
            }
            if (!CanMoveFigure(figure, Vector3.zero)) RotateCurrentFigure();
        }
    }
}
