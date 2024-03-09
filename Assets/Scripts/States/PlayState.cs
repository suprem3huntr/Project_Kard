public class PlayState : State{
    private bool isSelected;
    public PlayState(){
        nextState = new AttackState();
        currState = States.PlayState;
    }

    public override bool isCondition(){
        return isSelected;
    }

    public override State NextState(){
        return nextState;
    }
}