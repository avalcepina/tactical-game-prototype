using System.Collections;

namespace SA
{
    public class Turn
    {

        private enum State
        {
            Idle,
            Pathfinding,
            Moving,
            Moved
        }

        Character character;

        bool isMoving = false;

        public Turn(Character character)
        {
            this.character = character;
        }

        public bool Execute(TurnManager turnManager)
        {
            return false;
        }

    }
}

