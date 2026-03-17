namespace CalisthenicsSkillTracker.GCommon.Exceptions;

public class EntityCreatePersistException : Exception
{
    public EntityCreatePersistException()
    {
        
    }

    public EntityCreatePersistException(string message)
        : base(message)
    {
        
    }
}
