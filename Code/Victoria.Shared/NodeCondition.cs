using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Victoria.Shared
{
    public class NodeCondition : Node
    {
        public string Code { get; set; }
        
        public Node ChildNodeFalse { get; set; }

        public Node ChilNodeTrue { get; set; }

        public override bool canBeDebugged
        {
            get { return true; }
        }

        public override Node Execute(IList<StageVariable> variables, Delegate NotifyUIMethod)
        { 
            try
            {
                Debug.Debug.instance().execute(this, NotifyUIMethod, variables);

                var cultureInfo = new CultureInfo("en-US");
                
                string sentence = ExpressionResolver.GetSentenceToEvaluate(variables, cultureInfo, this.Code);

                var result = ExpressionResolver.ResolveBoolen(sentence);

                return result ? this.ChildNodeFalse.Execute(variables, NotifyUIMethod) : this.ChilNodeTrue.Execute(variables, NotifyUIMethod);
            }
            catch (Exception exception)
            {
                throw new Exception("Nodo condicion", exception);
            }
        }



      }
}

