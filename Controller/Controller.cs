using Game_2048.Model;

namespace Game_2048.Controllers
{
    public class Controller
    {
        private Game _game;

        public Controller(Game game)
        {
            _game = game;
        }

        internal void restore()
        {
            _game.Restore();
        }

        internal void moveUp()
        {
            _game.MoveUp();
        }

        internal void moveDown()
        {
            _game.MoveDown();
        }

        internal void moveLeft()
        {
            _game.MoveLeft();
        }

        internal void moveRight()
        {
            _game.MoveRight();
        }

        internal void reset()
        {
            _game.Reset();
        }
    }
}