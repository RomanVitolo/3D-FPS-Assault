using FSM;

namespace DecisionTree
{
    public delegate bool QuestionDelegate();
    
    public class QuestionNode : INode
    {
        private QuestionDelegate _question;
        /*private INode _trueNode;
        private INode _falseNode;*/

        private FSMState<string> _trueNode;
        private FSMState<string> _falseNode;
        
        /*public QuestionNode(QuestionDelegate question, INode trueNode, INode falseNode)
        {
            _question = question;
            _trueNode = trueNode;
            _falseNode = falseNode;
        }*/

        public QuestionNode(QuestionDelegate question, FSMState<string> trueNode, FSMState<string> falseNode)
        {
            _question = question;
            _trueNode = trueNode;
            _falseNode = falseNode;
        }

        public void Execute()
        {
            if (_question()) 
                _trueNode.Enter();  
            else 
                _falseNode.Enter(); 
        }
    }
}