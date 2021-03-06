namespace Game_2048.Model
{
    internal class Movement
    {
        public delegate void UpdSDel(int score, bool hard);
        public static event UpdSDel Score;

        public static int[,] MoveLeftUp(int x, int y, int[,] state)
        {
            var helper = new bool[4, 4];
            for (int i = 3; i - x >= 0; i--)
            {
                for (int j = 3; j - y >= 0; j--)
                {
                    var isEmptySpace = state[i, j] != 0 && state[i - x, j - y] == 0;
                    if (isEmptySpace)
                    {
                        state[i - x, j - y] = state[i, j];
                        state[i, j] = 0;
                    }
                    else
                    {
                        var isMergePossible = state[i, j] == state[i - x, j - y] && state[i, j] != 0 && !helper[i, j];
                        if (isMergePossible)
                        {
                            int x2 = x;
                            int y2 = y;

                            if ((i - 2 * x == 1 && x != 0 || j - 2 * y == 1 && y != 0) && state[i - 2 * x, j - 2 * y] == state[i, j] && state[i - 3 * x, j - 3 * y] != state[i, j])
                            {
                                x2 *= 2;
                                y2 *= 2;
                            }

                            else if ((i - 2 * x == 0 && x != 0 || j - 2 * y == 0 && y != 0) && state[i - 2 * x, j - 2 * y] == state[i, j])
                            {
                                x2 *= 2;
                                y2 *= 2;
                            }

                            helper[i - x2, j - y2] = true;
                            state[i - x2, j - y2] *= 2;
                            Score?.Invoke(state[i - x2, j - y2], false);
                            state[i, j] = 0;
                        }
                    }

                    if (x == 1 && i < 3 && state[i + x, j] != 0 && state[i, j] == 0)
                    {
                        state[i, j] = state[i + x, j];
                        state[i + x, j] = 0;
                    }
                    if (y == 1 && j < 3 && state[i, j + y] != 0 && state[i, j] == 0)
                    {
                        state[i, j] = state[i, j + y];
                        state[i, j + y] = 0;
                    }
                }
            }

            for (int i = 0; i < 4; i++)
            {
                if (x == 1 && state[3, i] != 0 && state[2, i] == 0)
                {
                    state[2, i] = state[3, i];
                    state[3, i] = 0;
                }
                if (y == 1 && state[i, 3] != 0 && state[i, 2] == 0)
                {
                    state[i, 2] = state[i, 3];
                    state[i, 3] = 0;
                }
            }

            return state;
        }

        public static int[,] MoveRightDown(int x, int y, int[,] state)
        {
            bool[,] helper = new bool[4, 4];
            for (int i = 0; i + x < 4; i++)
            {
                for (int j = 0; j + y < 4; j++)
                {
                    var isEmptySpace = state[i, j] != 0 && state[i + x, j + y] == 0;
                    if (isEmptySpace)
                    {
                        state[i + x, j + y] = state[i, j];
                        state[i, j] = 0;
                    }
                    else
                    {
                        var isMergePossible = state[i, j] == state[i + x, j + y] && state[i, j] != 0 && !helper[i, j];
                        if (isMergePossible)
                        {
                            int x2 = x;
                            int y2 = y;

                            if ((i + 2 * x == 2 && x != 0 || j + 2 * y == 2 && y != 0) && state[i + 2 * x, j + 2 * y] == state[i, j] && state[i + 3 * x, j + 3 * y] != state[i, j])
                            {
                                x2 *= 2;
                                y2 *= 2;
                            }
                            else if ((i + 2 * x == 3 && x != 0 || j + 2 * y == 3 && y != 0) && state[i + 2 * x, j + 2 * y] == state[i, j])
                            {
                                x2 *= 2;
                                y2 *= 2;
                            }

                            helper[i + x2, j + y2] = true;
                            state[i + x2, j + y2] *= 2;
                            Score?.Invoke(state[i + x2, j + y2], false);
                            state[i, j] = 0;
                        }
                    }

                    if (x == 1 && i > 1 && state[i - x, j] != 0 && state[i, j] == 0)
                    {
                        state[i, j] = state[i - x, j];
                        state[i - x, j] = 0;
                    }
                    if (y == 1 && j > 1 && state[i, j - y] != 0 && state[i, j] == 0)
                    {
                        state[i, j] = state[i, j - y];
                        state[i, j - y] = 0;
                    }
                }
            }

            for (int i = 0; i < 4; i++)
            {
                if (x == 1 && state[0, i] != 0 && state[1, i] == 0)
                {
                    state[1, i] = state[0, i];
                    state[0, i] = 0;
                }
                if (y == 1 && state[i, 0] != 0 && state[i, 1] == 0)
                {
                    state[i, 1] = state[i, 0];
                    state[i, 0] = 0;
                }
            }

            return state;
        }
    }
}