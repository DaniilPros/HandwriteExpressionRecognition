using System;
using System.Collections.Generic;
using System.IO;

namespace program
{
    class ExpressionParser
    {
        private string Expression;
        public int Value;
        private bool IsCorrect;

        public ExpressionParser(string str = "")
        {
            Expression = str;
            Expression = Expression.Replace(" ", string.Empty);
            Console.WriteLine(Expression);
            IsCorrect = Validation();
        }

        public void SetExpression(string Expression)
        {
            this.Expression = Expression;
        }

        private bool Validation()
        {
            int OpenBracketCount = 0, CloseBracketCount = 0;
            bool IsOperandPrevious = false;
            for (int i = 0; i < Expression.Length; i++)
            {
                if (Expression[i] == '(')
                    OpenBracketCount++;
                if (Expression[i] == ')')
                    CloseBracketCount++;
                if (IsOperandPrevious && GetPriority(Expression[i]) > 1)
                    return false;
                else if (GetPriority(Expression[i]) > 1)
                    IsOperandPrevious = true;
                else
                    IsOperandPrevious = false;
            }
            if (OpenBracketCount != CloseBracketCount)
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

        private string CalculateExpression()
        {
            int Result = 0;
            List<string> str = MakePolishNotation(Expression);
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
                else if (Element == "^")
                {
                    int FirstOperand = OperandsStack.Pop();
                    int SecondOperand = OperandsStack.Pop();
                    OperandsStack.Push(Math.Pow((double)SecondOperand, (double)FirstOperand));
                }
                else
                    OperandsStack.Push(Int32.Parse(Element));
            }
            Result = OperandsStack.Pop();
            return Result;
        }

        void GetValue()
        {
                    if (Validation())
                        return CalculateExpression().ToString();
                    else return "Error in expression";
        }
    }
}
