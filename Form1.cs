using Game_2048.Controllers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Game_2048
{
    public partial class Form1 : Form
    {
        private Controller _controller;
        public Form1(Controller controller)
        {
            InitializeComponent();
            _controller = controller;
            Init();
        }

        //Плитки
        public readonly Dictionary<int, Color> Colors = new Dictionary<int, Color>()
        {
            { 0, Color.FromArgb(204, 192, 178) },
            { 2, Color.FromArgb(238, 228, 218) },
            { 4, Color.FromArgb(237, 224, 200) },
            { 8, Color.FromArgb(242, 177, 121) },
            { 16, Color.FromArgb(245, 149, 99) },
            { 32, Color.FromArgb(246, 124, 95) },
            { 64, Color.FromArgb(246, 94, 59) },
            { 128, Color.FromArgb(237, 207, 114) },
            { 256, Color.FromArgb(237, 204, 97) },
            { 512, Color.FromArgb(237, 200, 80) },
            { 1024, Color.FromArgb(237, 197, 63) },
            { 2048, Color.FromArgb(237, 194, 46) },
            { 4096, Color.FromArgb(110, 204, 19) }
        };
        public readonly Color ColorPlus = Color.FromArgb(89, 137, 247);

        public Panel[,] GameBoard;
        public Label[,] GameLabels;

        //Инициализация доски
        private void Init()
        {
            GameBoard = new[,]
            {
                { PA1, PB1, PC1, PD1},
                { PA2, PB2, PC2, PD2},
                { PA3, PB3, PC3, PD3},
                { PA4, PB4, PC4, PD4}
            };
            GameLabels = new[,]
            {
                { LA1, LB1, LC1, LD1},
                { LA2, LB2, LC2, LD2},
                { LA3, LB3, LC3, LD3},
                { LA4, LB4, LC4, LD4}
            };
        }

        //Текущий счет
        public void Score(int score)
        {
            LScore.Text = $@"Текущий счет: {score}";
        }

        //Лучший счет
        public void HScore(int score)
        {
            LHScore.Text = $@"Лучший счет: {score}";
        }

        //Обновление доски
        public void UpdDisp(int x, int y, int value)
        {
            Color col;
            col = Colors.TryGetValue(value, out col) ? col : ColorPlus;
            GameBoard[x, y].BackColor = col;

            if (value == 0)
            {
                GameLabels[x, y].Text = "";
            }
            else
            {
                GameLabels[x, y].Text = value.ToString();
            }
        }

        public enum MoveControl
        {
            Up, Down, Left, Right, Invalid
        }

        public readonly Dictionary<Keys, MoveControl> ControlsMap = new Dictionary<Keys, MoveControl>()
        {
            {Keys.Up, MoveControl.Up},
            {Keys.Down, MoveControl.Down},
            {Keys.Left, MoveControl.Left},
            {Keys.Right, MoveControl.Right}
        };

        //Отслеживание кнопок
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            MoveControl cont;
            cont = ControlsMap.TryGetValue(e.KeyData, out cont) ? cont : MoveControl.Invalid;
            Key(cont);
        }

        private void Key(MoveControl key)
        {
            switch (key)
            {
                case MoveControl.Up:
                    _controller.moveUp();
                    break;
                case MoveControl.Down:
                    _controller.moveDown();
                    break;
                case MoveControl.Left:
                    _controller.moveLeft();
                    break;
                case MoveControl.Right:
                    _controller.moveRight();
                    break;
            }
        }

        //Ход назад
        private void Undo(object sender, EventArgs e)
        {
            _controller.restore();
        }

        //Новая игра
        private void NewGame(object sender, EventArgs e)
        {
            Init();
            _controller.reset();
        }

        //Окно с проигрышем
        public void GenErr()
        {
            MessageBox.Show(@"Больше нет свободных ходов!", @"Игра завершена", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}