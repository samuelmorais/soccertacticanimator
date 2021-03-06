namespace NG.CommandPatternExample
{
    /// <summary>
    /// The 'Command' abstract class that we will inherit from
    /// </summary>
    public abstract class Command
    {
        public abstract void Execute();
        public abstract void UnExecute();
    }
}
