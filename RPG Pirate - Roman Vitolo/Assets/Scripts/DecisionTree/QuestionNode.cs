using FSM;

namespace DecisionTree
{
    public delegate bool QuestionDelegate();
    
    public class QuestionNode : INode
    {
        private QuestionDelegate _question;
        private INode _trueNode;
        private INode _falseNode;     
        
        public QuestionNode(QuestionDelegate question, INode trueNode, INode falseNode)
        {
            _question = question;
            _trueNode = trueNode;
            _falseNode = falseNode;
        }    

        public void Execute()
        {
            if (_question()) 
                _trueNode.Execute();  
            else 
                _falseNode.Execute(); 
        }
    }
}