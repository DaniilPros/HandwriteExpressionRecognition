using System;
using System.Collections.Generic;
using System.IO;

namespace program
{
    class Node
    {
        public string Val { get; set; }
        public Node LeftNode { get; set; }
        public Node RightNode { get; set; }
        public Node NextNode { get; set; }

        public void Order(Node n)
        {
            if (n == null)
                return;
            Order(n.LeftNode);
            Order(n.RightNode);
            Debug.Write(n.Val);

        }
    }

    class ExpressionParser
    {
        private string _expression;
        public string Value
        {
            get
            {
                if (Validation())
                    return CalculateExpression();
                else
                    return "Wrong expression!";
            }
        }

        private Node _root = new Node();

        public ExpressionParser(string str = "")
        {
            _expression = str;
            _expression = _expression.Replace(" ", string.Empty);
            Debug.WriteLine(_expression);
        }

        public void SetExpression(string expression)
        {
            this._expression = expression;
        }

        private bool Validation()
        {
            int openBracketCount = 0, closeBracketCount = 0;
            bool isOperandPrevious = false;
            for (int i = 0; i < _expression.Length; i++)
            {
                if (_expression[i] == '(')
                    openBracketCount++;
                if (_expression[i] == ')')
                    closeBracketCount++;
                if (isOperandPrevious && GetPriority(_expression[i]) > 1)
                    return false;
                else if (GetPriority(_expression[i]) > 1)
                    isOperandPrevious = true;
                else
                    isOperandPrevious = false;
            }
            if (openBracketCount != closeBracketCount)
                return false;
            return true;
        }

        private int GetPriority(char ch)
        {
            switch (ch)
            {
                case '(': return 0;
                case ')': return 1;
                case '+': return 2;
                case '-': return 2;
                case '*': return 3;
                case '/': return 3;
                case '^': return 4;
                default: return -1;
            }
        }

        private List<string> MakePolishNotation(string str)
        {
            List<string> outStr = new List<string>();
            Stack<char> operatorsStack = new Stack<char>();
            for (int i = 0; i < str.Length; i++)
            {
                char ch = str[i];
                if (ch == '(')
                    operatorsStack.Push(ch);
                else if (ch == ')')
                {
                    while (operatorsStack.Peek() != '(')
                        outStr.Add(operatorsStack.Pop().ToString());
                    operatorsStack.Pop();
                }
                else if (ch <= '9' && ch >= '0')
                {
                    string num = "";
                    while (str[i] <= '9' && str[i] >= '0')
                    {
                        num += str[i];
                        i++;
                        if (i == str.Length)
                            break;
                    }
                    i--;
                    outStr.Add(num);
                }
                else if (ch == '+' || ch == '-' || ch == '*' || ch == '/' || ch == '^')
                {
                    if (operatorsStack.Count == 0)
                        operatorsStack.Push(ch);
                    else
                    {
                        if (GetPriority(operatorsStack.Peek()) < GetPriority(ch))
                            operatorsStack.Push(ch);
                        else
                        {
                            while (operatorsStack.Count != 0 && GetPriority(operatorsStack.Peek()) >= GetPriority(ch))
                                outStr.Add(operatorsStack.Pop().ToString());
                            operatorsStack.Push(ch);
                        }
                    }
                }
            }
            while (operatorsStack.Count != 0)
                outStr.Add(operatorsStack.Pop().ToString());
            return outStr;
        }

        private string CalculateExpression()
        {
            double result = 0;
            List<string> str = MakePolishNotation(_expression);
            Stack<double> operandsStack = new Stack<double>();
            foreach (string element in str)
            {
                if (element == "+")
                {
                    double firstOperand = operandsStack.Pop();
                    double secondOperand = operandsStack.Pop();
                    operandsStack.Push(firstOperand + secondOperand);
                }
                else if (element == "-")
                {
                    double firstOperand = operandsStack.Pop();
                    double secondOperand = operandsStack.Pop();
                    operandsStack.Push(secondOperand - firstOperand);
                }
                else if (element == "*")
                {
                    double firstOperand = operandsStack.Pop();
                    double secondOperand = operandsStack.Pop();
                    operandsStack.Push(firstOperand * secondOperand);
                }
                else if (element == "/")
                {
                    double firstOperand = operandsStack.Pop();
                    double secondOperand = operandsStack.Pop();
                    operandsStack.Push(secondOperand / firstOperand);
                }
                else if (element == "^")
                {
                    double firstOperand = operandsStack.Pop();
                    double secondOperand = operandsStack.Pop();
                    operandsStack.Push(Math.Pow((double)secondOperand, (double)firstOperand));
                }
                else
                    operandsStack.Push(Int32.Parse(element));
            }
            result = operandsStack.Pop();
            return result.ToString();
        }

        public void MakeTree()
        {
            var treeargument = MakePolishNotation(_expression);
            int cnt = 0; Stack<Node> st = new Stack<Node>();
            st.Push(null);
            while (cnt < treeargument.Count)
            {
                var p = new Node();
                p.Val = treeargument[cnt].ToString();
                if (p.Val == "+" || p.Val == "-" || p.Val == "*" || p.Val == "/" || p.Val == "^")
                {
                    p.RightNode = st.Pop();
                    p.LeftNode = st.Pop();
                    st.Push(p);
                }
                else
                {
                    p.LeftNode = null;
                    p.RightNode = null;
                    st.Push(p);
                }
                cnt++;
            }
            _root = st.Peek();
            _root.Order(_root);
        }

        private void AddNode(Node n)
        {
            var el = n.Val;
            if (el == "(")
            {
                var node = new Node();
                AddNode(node.LeftNode);
                node.Val = node.Val;
                AddNode(node.RightNode);
                el = n.Val;
            }
            else
            {
                var node = new Node
                {
                    Val = el,
                    LeftNode = null,
                    RightNode = null
                };
            }
        }
    }
}
