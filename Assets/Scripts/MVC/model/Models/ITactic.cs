namespace FootTactic
{
    public interface ITactic
    {
        IFTTeam[] Teams { get; set; }
        
        void InitializeAnimations(AnimStates animStates);
        
    }
}