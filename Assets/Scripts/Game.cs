using Tetris.Controllers;
using Tetris.Logic;
using Tetris.Fields;
using UnityEngine;

namespace Tetris
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(GameLogic))]
    [RequireComponent(typeof(GameController))]
    [RequireComponent(typeof(Field))]
    public class Game : MonoBehaviour
    {
        protected GameController controller;
        protected GameLogic gameLogic;
        protected Field field;
        private float deltaTime;
        private float iteratorTime;
        [Range(0.1f, 5f)]
        public float speed = 1;
        public int score;

        public void Awake()
        {
            controller = GetComponent<GameController>();
            gameLogic = GetComponent<GameLogic>();
            field = GetComponent<Field>();
        }

        public void Start()
        {
            gameLogic.startGame();
        }

        public void Update()
        {
            iteratorTime = 1f / speed;
            deltaTime += Time.deltaTime;
            controller.readPlayerInput();
            if (deltaTime > iteratorTime)
            {
                gameLogic.iteration();
                deltaTime = 0;
            }
        }
    }
}
