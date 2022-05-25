using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Game_2048.Model
{
    public class Game
    {
        public delegate void UpdateBoardDel(int x, int y, int value);
        public event UpdateBoardDel UpdateBoard;

        public delegate void UpdateCurrentScoreDel(int score);
        public event UpdateCurrentScoreDel UpdateCurrentScore;

        public delegate void UpdHighScoreDel(int score);
        public event UpdHighScoreDel UpdateHighScore;

        public delegate void GenerateErrorDel();
        public event GenerateErrorDel GenerateError;

        public Guid ScoreId;
        public int Score;
        public int ScorePrev;

        public int[,] State = new int[4, 4];

        public int[,] StatePrev = new int[4, 4];

        private int _maxScore;

        private async void InitScore()
        {
            HighScore = await ScoreService.GetMaxAsync();
        }

        public enum MoveControl
        {
            Up, Down, Left, Right, Invalid
        }

        public void Reset()
        {
            ScoreId = Guid.NewGuid();
            InitScore();

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    State[i, j] = 0;
                    UpdateBoard?.Invoke(i, j, State[i, j]);
                }
            }

            Movement.Score += Scoring;

            Scoring(0, true);
            UpdateHighScore?.Invoke(HighScore);

            Generate(true);
        }

        private bool HasEmptyCell()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (State[i, j] == 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsGameOver()
        {
            bool gameOver = true;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if ((i > 1 && State[i, j] == State[i - 1, j]) || (i < 3 && State[i, j] == State[i + 1, j])
                     || (j < 3 && State[i, j] == State[i, j + 1]) || (j > 1 && State[i, j] == State[i, j - 1]))
                    {
                        gameOver = false;
                    }
                }
            }

            return gameOver;
        }

        private void Generate(bool hasChanges)
        {
            var hasEmptyCell = HasEmptyCell();
            if (!hasEmptyCell)
            {
                var isGameOver = IsGameOver();
                if (isGameOver)
                {
                    GenerateError?.Invoke();
                    return;
                }
            }
            else if (hasChanges)
            {
                Random ran = new Random();
                bool work = true;
                int x = -1;
                int y = -1;
                while (work)
                {
                    x = ran.Next(0, 4);
                    y = ran.Next(0, 4);
                    work = State[x, y] != 0;
                }

                if (ran.Next(0, 10) == 0)
                {
                    State[x, y] = 4;
                }
                else
                {
                    State[x, y] = 2;
                }

                UpdateBoard?.Invoke(x, y, State[x, y]);
            }
        }

        public readonly Dictionary<Keys, MoveControl> ControlsMap = new Dictionary<Keys, MoveControl>()
        {
            {Keys.Up, MoveControl.Up},
            {Keys.Down, MoveControl.Down},
            {Keys.Left, MoveControl.Left},
            {Keys.Right, MoveControl.Right}
        };

        public void MoveUp()
        {
            Backup();
            State = Movement.MoveLeftUp(1, 0, State);
            Updates();
        }

        public void MoveDown()
        {
            Backup();
            State = Movement.MoveRightDown(1, 0, State);
            Updates();
        }

        public void MoveLeft()
        {
            Backup();
            State = Movement.MoveLeftUp(0, 1, State);
            Updates();
        }

        public void MoveRight()
        {
            Backup();
            State = Movement.MoveRightDown(0, 1, State);
            Updates();
        }

        private void Updates()
        {
            var hasChanges = false;
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (State[x, y] != StatePrev[x, y])
                    {
                        hasChanges = true;
                        UpdateBoard?.Invoke(x, y, State[x, y]);
                    }
                }
            }

            Generate(hasChanges);
        }

        private void Backup()
        {
            ScorePrev = Score;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    StatePrev[i, j] = State[i, j];
                }
            }
        }

        public void Restore()
        {
            Scoring(ScorePrev, true);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    State[i, j] = StatePrev[i, j];
                    UpdateBoard?.Invoke(i, j, State[i, j]);
                }
            }
        }

        private void Scoring(int points, bool hard)
        {
            if (hard)
            {
                Score = points;
            }
            else
            {
                Score += points;
            }
            UpdateCurrentScore?.Invoke(Score);

            if (Score > 0)
            {
                ScoreService.UpdateCurrent(ScoreId, Score);

                if (Score > HighScore)
                {
                    HighScore = Score;
                }
            }
        }

        public int HighScore
        {
            get
            {
                return _maxScore;
            }
            set
            {
                _maxScore = value;
                UpdateHighScore?.Invoke(value);
            }
        }
    }
}