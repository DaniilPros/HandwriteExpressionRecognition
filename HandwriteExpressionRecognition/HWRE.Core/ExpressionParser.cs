using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HWRE.Core
{
    class ExpressionParser
    {
        private  string _expression;
        private double _value;

        public ExpressionParser(string str = "")
        {
            _expression = str;
            _expression = _expression.Replace(" ", string.Empty);
            Debug.WriteLine(_expression);
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

        private double CalculateExpression(List<string> str)
        {
            Stack<double> operandsStack = new Stack<double>();
            foreach (string element in str)
            {
                double firstOperand, secondOperand;
                if (element == "+")
                {
                    firstOperand = operandsStack.Pop();
                    secondOperand = operandsStack.Pop();
                    operandsStack.Push(firstOperand + secondOperand);
                }
                else if (element == "-")
                {
                    firstOperand = operandsStack.Pop();
                    secondOperand = operandsStack.Pop();
                    operandsStack.Push(secondOperand - firstOperand);
                }
                else if (element == "*")
                {
                    firstOperand = operandsStack.Pop();
                    secondOperand = operandsStack.Pop();
                    operandsStack.Push(firstOperand * secondOperand);
                }
                else if (element == "/")
                {
                    firstOperand = operandsStack.Pop();
                    secondOperand = operandsStack.Pop();
                    operandsStack.Push(secondOperand / firstOperand);
                }
                else if (element == "^")
                {
                    firstOperand = operandsStack.Pop();
                    secondOperand = operandsStack.Pop();
                    operandsStack.Push((int)Math.Pow(secondOperand, firstOperand));
                }
                else
                    operandsStack.Push(int.Parse(element));
            }
            var result = operandsStack.Pop();
            return result;
        }

        public string GetExpressionValue()
        {
            if (!Validation()) return "Error in expression";
            _value = CalculateExpression(MakePolishNotation(_expression));
            return _value.ToString();
        }

    }
}
