public class TargetState : State{
    private int targets;
    public TargetState(State s, int t){
        currState = States.TargetState;
        targets = t;
    }

    public override void doAction(){
        targets--;
    }

    public override bool isCondition(){
        return targets==0;
    }

}