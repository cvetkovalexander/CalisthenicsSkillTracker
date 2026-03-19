namespace CalisthenicsSkillTracker.GCommon.Exceptions;

public class EntityDeleteException : Exception
{
    public EntityDeleteException()
    {
        
    }

    public EntityDeleteException(string message)
        : base(message)
    {
        
    }
}
