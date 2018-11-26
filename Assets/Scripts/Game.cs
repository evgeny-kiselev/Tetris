using Tetris.Controllers;
using Tetris.Logic;
using Tetris.Fields;
using UnityEngine.UI;
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
        public bool isGameEnded = false;
        [Range(0.1f, 5f)]
        public float speed = 1;
        public int score;
        public Text scoreText;

        public int Score
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
                scoreText.text = "Score: " + score;
            }
        }

        public void Awake()
        {
            controller = GetComponent<GameController>();
            gameLogic = GetComponent<GameLogic>();
            field = GetComponent<Field>();
        }

        public void Start()
        {
            gameLogic.StartGame();
        }

        public void Update()
        {
            if (!isGameEnded)
            {
                iteratorTime = 1f / speed;
                deltaTime += Time.deltaTime;
                controller.readPlayerInput();
                if (deltaTime > iteratorTime)
                {
                    gameLogic.Iteration();
                    deltaTime = 0;
                }
            }
        }
    }
}
