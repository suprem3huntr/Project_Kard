public class GuardState : State{
    public GuardState(){
        nextState = new DrawState(new PlayState(),2);
        currState = States.DrawState;
    }

    public override State NextState(){
        return nextState;
    }
}