 namespace DecisionTree
{
    public delegate void ActionDelegate();
    public class ActionNode : INode
    {
        private ActionDelegate _action;

        public ActionNode(ActionDelegate action)
        {
            _action = action;
        }

        public void SubAction(ActionDelegate newAction)
        {
            _action += newAction;
        }
    
        public void Execute()
        {
            _action();
        }
    }
}

