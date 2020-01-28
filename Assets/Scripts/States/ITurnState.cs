namespace SA
{

    public interface ITurnState
    {

        ITurnState execute();

        bool isEndingState();

    }

}