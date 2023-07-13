using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TTCalculate
{
    internal class MathLogic
    {
        private int GetPrecedence(char token)
        {
            switch(token)
            {
                case '|':
                    return 1;
                case '&':
                    return 2;
                case '!':
                    return 3;
                default:
                    return 0;
            }
        }
        private List<string> ConvertToRPN(string expression)
        {
            List<string> output = new List<string>();
            Stack<string> stack = new Stack<string>();
            for (int i = 0; i < expression.Length; i++)
            {
                char c = expression[i];
                if (c == 'T' || c == 'F')
                {
                    output.Add(c.ToString());
                }
                else if (c == '(')
                {
                    stack.Push(c.ToString());
                }
                else if (c == ')')
                {
                    while (stack.Peek() != "(")
                    {
                        output.Add(stack.Pop());
                    }
                    stack.Pop();
                }
                else if (c == '!' || c == '&' || c == '|')
                {
                    while (stack.Count > 0 && GetPrecedence(c) <= GetPrecedence(stack.Peek()[0]))
                    {
                        output.Add(stack.Pop());
                    }
                    stack.Push(c.ToString());
                }
            }
            while (stack.Count > 0)
            {
                output.Add(stack.Pop());
            }
            return output;
        }

        private protected bool EvaluateExpression(string expression)
        {
            expression = expression.Replace(" ", "");
            List<string> tokens = ConvertToRPN(expression);
            Stack<bool> stack = new Stack<bool>();
            foreach (string token in tokens)
            {
                if (token == "T")
                {
                    stack.Push(true);
                }
                else if (token == "F")
                {
                    stack.Push(false);
                }
                else if (token == "!")
                {
                    bool operand = stack.Pop();
                    stack.Push(!operand);
                }
                else if (token == "&")
                {
                    bool operand1 = stack.Pop();
                    bool operand2 = stack.Pop();
                    stack.Push(operand1 && operand2);
                }
                else if (token == "|")
                {
                    bool operand1 = stack.Pop();
                    bool operand2 = stack.Pop();
                    stack.Push(operand1 || operand2);
                }
            }
            return stack.Pop();
        }




    }
}
