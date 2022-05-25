using System;
using System.Windows.Forms;
using Game_2048.Controllers;
using Game_2048.Model;

namespace Game_2048
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Game _game = new Game();
            Controller controller = new Controller(_game);
            Form1 form1 = new Form1(controller);
            _game.UpdateBoard += form1.UpdDisp;
            _game.UpdateCurrentScore += form1.Score;
            _game.UpdateHighScore += form1.HScore;
            _game.GenerateError += form1.GenErr;

            controller.reset();

            Application.Run(form1);
        }
    }
}