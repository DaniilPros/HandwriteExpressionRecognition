using System;
using System.Collections.Generic;

namespace HWRE.Core
{
    class ExpressionParser
    {
        private  string _expression;
        //private string PolishNotation;
        private int _value;
        private Dictionary<char,int> _priority = new Dictionary<char, int>()
        {
            {')',0},
            {')',0}
        };

        public ExpressionParser(string str = "")
        {
            _expression = str;
            _expression = _expression.Replace(" ", string.Empty);
            Console.WriteLine(_expression);
            if (Validation())
                _value = CalculateExpression(MakePolishNotation(_expression));
            else
                Console.WriteLine("Error in expression!!!");
        }

        private bool Validation()
        {
            int openBracketCount = 0, closeBracketCount = 0;
            var isOperandPrevious = false;
            foreach (var t in _expression)
            {
                if (t == '(')
                    openBracketCount++;
                if (t == ')')
                    closeBracketCount++;
                if (isOperandPrevious && GetPriority(t) > 1)
                    return false;
                else if (GetPriority(t) > 1)
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

        private int StringToInt(string str)
        {
            int Result = 0;
            for (int i = 0; i < str.Length; i++)
                Result += (str[i] - '0') * (int)Math.Pow(10, str.Length - 1 - i);
            return Result;
        }

        private List<string> MakePolishNotation(string str)
        {
            List<string> OutStr = new List<string>();
            Stack<char> OperatorsStack = new Stack<char>();
            for (int i = 0; i < str.Length; i++)
            {
                char ch = str[i];
                if (ch == '(')
                    OperatorsStack.Push(ch);
                else if (ch == ')')
                {
                    while (OperatorsStack.Peek() != '(')
                        OutStr.Add(OperatorsStack.Pop().ToString());
                    OperatorsStack.Pop();
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
                    OutStr.Add(num);
                }
                else if (ch == '+' || ch == '-' || ch == '*' || ch == '/' || ch == '^')
                {
                    if (OperatorsStack.Count == 0)
                        OperatorsStack.Push(ch);
                    else
                    {
                        if (GetPriority(OperatorsStack.Peek()) < GetPriority(ch))
                            OperatorsStack.Push(ch);
                        else
                        {
                            while (OperatorsStack.Count != 0 && GetPriority(OperatorsStack.Peek()) >= GetPriority(ch))
                                OutStr.Add(OperatorsStack.Pop().ToString());
                            OperatorsStack.Push(ch);
                        }
                    }
                }
            }
            while (OperatorsStack.Count != 0)
                OutStr.Add(OperatorsStack.Pop().ToString());
            return OutStr;
        }

        private int CalculateExpression(List<string> str)
        {
            int Result = 0;
            Stack<int> OperandsStack = new Stack<int>();
            foreach (string Element in str)
            {
                if (Element == "+")
                {
                    int FirstOperand = OperandsStack.Pop();
                    int SecondOperand = OperandsStack.Pop();
                    OperandsStack.Push(FirstOperand + SecondOperand);
                }
                else if (Element == "-")
                {
                    int FirstOperand = OperandsStack.Pop();
                    int SecondOperand = OperandsStack.Pop();
                    OperandsStack.Push(SecondOperand - FirstOperand);
                }
                else if (Element == "*")
                {
                    int FirstOperand = OperandsStack.Pop();
                    int SecondOperand = OperandsStack.Pop();
                    OperandsStack.Push(FirstOperand * SecondOperand);
                }
                else if (Element == "/")
                {
                    int FirstOperand = OperandsStack.Pop();
                    int SecondOperand = OperandsStack.Pop();
                    OperandsStack.Push(SecondOperand / FirstOperand);
                }
                else
                    OperandsStack.Push(StringToInt(Element));
            }
            Result = OperandsStack.Pop();
            return Result;
        }

        public int GetExpressionValue()
        {
            return _value;
        }

    }
}
