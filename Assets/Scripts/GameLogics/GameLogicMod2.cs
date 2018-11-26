using UnityEngine;

namespace Tetris.Logic
{
    public class GameLogicMod2 : ClasicGameLogic
    {
        protected Transform hideLeftFig, hidenRightFig;

        public override void iteration()
        {
            if (!moveCurentFigure(new Vector3(0, -1, 0)))
            {
                Transform figure = collectVisibleBlocksInFigure();
                field.addFigure(figure);
                checkLine();

                currentFigure = factory.createFigure(field.spawnPoint).transform;
                createHidenFigures();
            }
        }

        protected Transform collectVisibleBlocksInFigure()
        {
            var figure = Instantiate(currentFigure.gameObject, transform).transform;
            while(figure.childCount > 0) DestroyImmediate(figure.GetChild(0).gameObject);
            figure.localPosition = new Vector3(0, figure.localPosition.y, figure.localPosition.z);
            while (currentFigure.childCount > 0)
            {
                var currentFigureBlock = currentFigure.GetChild(0);
                var leftHiddenBlock = hideLeftFig.GetChild(0);
                var rightHiddenBlock = hidenRightFig.GetChild(0);
                var minX = transform.position.x;
                var maxX = minX + field.width;

                if (currentFigureBlock.position.x >= minX && currentFigureBlock.position.x < maxX)
                    currentFigureBlock.parent = figure;
                else DestroyImmediate(currentFigureBlock.gameObject);

                if (leftHiddenBlock.position.x >= minX && leftHiddenBlock.position.x < maxX)
                    leftHiddenBlock.parent = figure;
                else DestroyImmediate(leftHiddenBlock.gameObject);

                if (rightHiddenBlock.position.x >= minX && rightHiddenBlock.position.x < maxX)
                    rightHiddenBlock.parent = figure;
                else DestroyImmediate(rightHiddenBlock.gameObject);
            }
            Destroy(currentFigure.gameObject);
            return figure;
        }

        public override void rotateCurrentFigure()
        {
            base.rotateCurrentFigure();
            rotateFigure(hideLeftFig);
            rotateFigure(hidenRightFig);
        }

        protected virtual void createHidenFigures()
        {
            if (hideLeftFig != null) DestroyImmediate(hideLeftFig.gameObject);
            if (hidenRightFig != null) DestroyImmediate(hidenRightFig.gameObject);

            hideLeftFig = Instantiate(currentFigure.gameObject, transform).transform;
            hideLeftFig.transform.position -= new Vector3(field.width, 0, 0);

            hidenRightFig = Instantiate(currentFigure.gameObject, transform).transform;
            hidenRightFig.transform.position += new Vector3(field.width, 0, 0);
        }

        public override bool moveCurentFigure(Vector3 moveVector)
        {
            bool isMooved = base.moveCurentFigure(moveVector);
            if (isMooved)
            {
                hideLeftFig.position += moveVector;
                hidenRightFig.position += moveVector;

                if (!isFigureVisible(currentFigure))
                    revisionCurrentFigure();
                updateBlocksVisible(currentFigure);
                updateBlocksVisible(hideLeftFig);
                updateBlocksVisible(hidenRightFig);

            }
            return isMooved;
        }

        protected void updateBlocksVisible(Transform figure)
        {
            var minX = transform.position.x;
            var maxX = minX + field.width;
            foreach (Transform block in figure)
            {
                if (block.position.x < minX || block.position.x >= maxX)
                    block.gameObject.SetActive(false);
                else block.gameObject.SetActive(true);
            }
        }

        protected void revisionCurrentFigure()
        {
            var figure = currentFigure;
            if (isFigureVisible(hideLeftFig))
            {
                currentFigure = hideLeftFig;
                hideLeftFig = figure;
            }
            else
            {
                currentFigure = hidenRightFig;
                hidenRightFig = figure;
            }
            createHidenFigures();
        }

        public override void startGame()
        {
            base.startGame();
            createHidenFigures();
        }

        protected bool isFigureVisible(Transform figure)
        {
            foreach (Transform block in figure)
            {
                var blockX = figure.localPosition.x + block.localPosition.x;
                if (blockX >= 0 && blockX < field.width) return true;
            }
            return false;
        }

        protected override void checkLine()
        {
            var collectedLineFound = 0;
            var isCollectedLine = true;
            for (var y = 0; y < field.height; y++)
            {
                for (var x = 0; x < field.width && isCollectedLine; x++)
                {
                    if (field.getCeilObject(x, y) == null)
                    {
                        collectedLineFound = 0;
                        isCollectedLine = false;
                    }
                }
                if (isCollectedLine) collectedLineFound++;
                if (collectedLineFound == 2)
                {
                    addScore(2);
                    field.removeLine(y - 1);
                    field.removeLine(y - 1);
                    y -= 2;
                }
            }
        }

        protected override bool isValidBlockPosition(Transform block, Vector3 moveVector)
        {
            var newPosition = block.position + moveVector - transform.position;
            if (newPosition.x < 0)
                if (field.getCeilObject((int)(field.width + newPosition.x), (int)newPosition.y) != null)
                    return false;
            if (newPosition.x >= field.width)
                if (field.getCeilObject((int)(newPosition.x - field.width), (int)newPosition.y) != null)
                    return false;
            if (newPosition.x >= 0 && newPosition.x < field.width)
                if (field.getCeilObject(newPosition) != null) return false;
            if (newPosition.y < 0) return false;
            return true;
        }
    }
}
