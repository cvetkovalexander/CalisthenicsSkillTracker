namespace CalisthenicsSkillTracker.GCommon.Exceptions;

public class EntityEditPersistException : Exception
{
    public EntityEditPersistException()
    {
        
    }

    public EntityEditPersistException(string message)
        :base(message)
    {
        
    }
}
