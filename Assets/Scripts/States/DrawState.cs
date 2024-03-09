public class DrawState : State{
    private int draws;
    public DrawState(State s, int d){
        currState = States.DrawState;
        nextState = s;
        draws = d;
    }

    public override void doAction(){
        draws--;
    }

    public override bool isCondition(){
        return draws==0;
    }

}