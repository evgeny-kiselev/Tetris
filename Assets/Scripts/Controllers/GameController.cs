using Tetris.Logic;
using UnityEngine;

namespace Tetris.Controllers
{
    public class GameController : MonoBehaviour
    {
        protected delegate bool MoveBlockMethod(Vector3 moveVector);
        protected delegate void RotateBlockMethod();

        protected MoveBlockMethod moveBlock;
        protected RotateBlockMethod rotateBlock;

        public void Awake()
        {
            var gameLogic = GetComponent<GameLogic>();
            moveBlock = gameLogic.moveCurentFigure;
            rotateBlock = gameLogic.rotateCurrentFigure;
        }

        public virtual void readPlayerInput()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) moveBlock(new Vector3(1, 0, 0));
            if (Input.GetKeyDown(KeyCode.LeftArrow)) moveBlock(new Vector3(-1, 0, 0));
            if (Input.GetKey(KeyCode.DownArrow)) moveBlock(new Vector3(0, -1, 0));
            if (Input.GetKeyDown(KeyCode.UpArrow)) rotateBlock();

        }
    }
}
