using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artisan.Tools.Calculator
{
    public class Analizer
    {
        private string code;
        private int index;
        private Dictionary<string, double> vars;
        private Dictionary<string, Func<Stack, double>> functions;
        private Token token = null;
        private char[] delimiters;
        private List<CalculationHistoryItem> history;


        public Analizer()
        {
            vars = new Dictionary<string, double>();
            functions = new Dictionary<string, Func<Stack, double>>();
            history = new List<CalculationHistoryItem>();
            delimiters = "+-*/%^=(),<>".ToCharArray();
            SetFunctions();
        }

        public double Calculate(string code)
        {
            this.index = 0;
            this.code = code;
            GetToken();
            if (token != null)
            {
                double retVal = ProcessVariable();
                history.Add(new CalculationHistoryItem(code, retVal));
                return retVal;
            }
            else
            {
                history.Add(new CalculationHistoryItem(code, 0));
                return 0;
            }
        }

        private double ProcessVariable()
        {
            double retVal = 0;
            if (token.Type == TokenType.Variable)
            {
                Token left = token;
                int start = index;
                GetToken();
                if (token.Value != "=") // no es una asignacion
                {
                    index = start; // Devuelvo a la posicion anterior
                    token = left;
                    retVal = ProcessFunction();
                }
                else // es una asignacion
                {
                    GetToken();
                    retVal = ProcessFunction();
                    vars[left.Value.ToUpper()] = retVal;
                }
            }
            else
            {
                retVal = ProcessFunction();
                vars["X"] = retVal;
            }
            return retVal;
        }

        private double ProcessFunction()
        {
            double retVal = 0;
            if (token.Type == TokenType.Function)
            {
                Stack stack = new Stack();
                string function = token.Value;

                GetToken();

                if (token.Value == "(") // no es una funcion
                {
                    GetToken();
                    stack.Push(ProcessFunction());
                }
                while (token.Value == ",")
                {
                    GetToken();
                    stack.Push(ProcessFunction());
                }
                if (token.Value == ")")
                {
                    GetToken();
                    retVal = Execute(function.ToUpper(), stack);
                }
            }
            else
            {
                retVal = ProcessOr();
            }
            return retVal;
        }

        //cond or
        private double ProcessOr()
        {
            double retVal = ProcessAnd();
            while (token.Value == "||")
            {
                string op = token.Value;
                GetToken();
                double hold = ProcessAnd();

                retVal = ((retVal != 0) || (hold != 0)) ? 1 : 0;
            }
            return retVal;
        }

        //cond and
        private double ProcessAnd()
        {
            double retVal = ProcessComparision();

            while (token.Value == "&&")
            {
                string op = token.Value;
                GetToken();
                double hold = ProcessComparision();

                retVal = ((retVal != 0) && (hold != 0)) ? 1 : 0;
            }
            return retVal;
        }

        //comparission
        private double ProcessComparision()
        {
            double retVal = ProcessEquality();
            string op = token.Value;
            if ((op == "<") || (op == ">") || (op == "<=") || (op == ">="))
            {
                GetToken();
                double hold = ProcessEquality();
                switch (op)
                {
                    case "<": retVal = (retVal < hold) ? 1 : 0; break;
                    case ">": retVal = (retVal > hold) ? 1 : 0; break;
                    case "<=": retVal = (retVal <= hold) ? 1 : 0; break;
                    case ">=": retVal = (retVal >= hold) ? 1 : 0; break;
                }
            }
            return retVal;
        }

        //equality
        private double ProcessEquality()
        {
            double retVal = ProcessSum();
            string op = token.Value;
            if ((op == "==") || (op == "<>"))
            {
                GetToken();
                double hold = ProcessSum();
                switch (op)
                {
                    case "==": retVal = (retVal == hold) ? 1 : 0; break;
                    case "<>": retVal = (retVal != hold) ? 1 : 0; break;
                }
            }
            return retVal;
        }

        private double ProcessSum()
        {
            double retVal = ProcessMultiply();
            while ((token.Value == "+") || (token.Value == "-"))
            {
                string op = token.Value;
                GetToken();
                double hold = ProcessMultiply();
                if (op == "+")
                    retVal += hold;
                else
                    retVal -= hold;
            }
            return retVal;
        }

        private double ProcessMultiply()
        {
            double retVal = ProcessExp();
            while ((token.Value == "*") || (token.Value == "/") || (token.Value == "%"))
            {
                string op = token.Value;
                GetToken();
                double hold = ProcessExp();
                if (op == "*")
                    retVal *= hold;
                else if (op == "/")
                    retVal /= hold;
                else
                    retVal %= hold;
            }
            return retVal;
        }

        private double ProcessExp()
        {
            double retVal = ProcessUnary();
            char op = (token.Value.Length > 0) ? token.Value[0] : '\0';
            if (op == '^')
            {
                GetToken();
                double hold = ProcessUnary();
                retVal = Math.Pow(retVal, hold);
            }
            return retVal;
        }

        private double ProcessUnary()
        {
            Token cand = token;
            double retVal = ProcessParenthesis();
            char op = (cand.Value.Length > 0) ? cand.Value[0] : '\0';
            if ((cand.Type == TokenType.Delimiter) && ((op == '+') || (op == '-')))
            {
                GetToken();
                double hold = ProcessParenthesis();
                if (op == '-')
                    retVal = -hold;
            }
            return retVal;
        }

        private double ProcessParenthesis()
        {
            double retVal = 0;
            char op = (token.Value.Length > 0) ? token.Value[0] : '\0';
            if ((token.Type == TokenType.Delimiter) && (op == '('))
            {
                GetToken();
                retVal = ProcessFunction();
                char op2 = (token.Value.Length > 0) ? token.Value[0] : '\0';
                if ((op2 != ')') && (op2 != ','))
                {
                    throw new ApplicationException("Unbalanced parentehesis");
                }
                GetToken();
            }
            else
            {
                retVal = Primitive();
            }
            return retVal;
        }

        private double Primitive()
        {
            double retVal = 0;
            switch (token.Type)
            {
                case TokenType.Variable:
                    if (!vars.TryGetValue(token.Value.ToUpper(), out retVal))
                    {
                        retVal = 0;
                    }
                    GetToken();
                    break;
                case TokenType.Number:
                    if (!double.TryParse(token.Value, out retVal))
                    {
                        retVal = 0;
                    }
                    GetToken();
                    break;
                case TokenType.Function:
                    retVal = ProcessFunction();
                    break;
            }
            return retVal;
        }

        private double Execute(string function, Stack parameters)
        {
            return functions[function](parameters);
        }

        private Token GetToken()
        {
            token = new Token() { Value = "", Type = TokenType.End };

            while ((index < code.Length) && char.IsWhiteSpace(code[index])) index++;

            if (index < code.Length)
            {
                if (code[index].IsIn(delimiters))
                {
                    int start = index++;
                    switch (code[start])
                    {
                        case '<':
                            switch (code[index])
                            {
                                case '=': index++; break;
                                case '>': index++; break;
                            }
                            break;
                        case '>':
                        case '=':
                            if (code[index] == '=')
                            {
                                index++;
                            }
                            break;
                        case '&':
                            if (code[index] == '&')
                            {
                                index++;
                            }
                            break;
                        case '|':
                            if (code[index] == '|')
                            {
                                index++;
                            }
                            break;
                    }
                    string op = code.Substring(start, index - start);
                    token = new Token() { Value = op, Type = TokenType.Delimiter };

                }
                else if (char.IsLetter(code[index]))
                {
                    int start = index++;
                    while ((index < code.Length) && !code[index].IsDelim(delimiters))
                    {
                        index++;
                    }
                    string name = code.Substring(start, index - start);
                    TokenType type = TokenType.Variable;
                    if (functions.ContainsKey(name.ToUpper()))
                        type = TokenType.Function;

                    token = new Token() { Value = name, Type = type };
                }
                else if (char.IsDigit(code[index]))
                {
                    int start = index++;
                    while ((index < code.Length) && (char.IsDigit(code[index]) || (code[index] == '.')))
                    {
                        index++;
                    }
                    token = new Token() { Value = code.Substring(start, index - start), Type = TokenType.Number };
                }
            }
            return token;
        }

        private void SetFunctions()
        {
            functions.Add("IF", (s) =>
            {
                var par3 = (double)s.Pop();
                var par2 = (double)s.Pop();
                var par1 = (double)s.Pop();
                return (par1 != 0) ? par2 : par3;
            });
            functions.Add("ABS", (s) =>
            {
                var par1 = s.Pop();
                return Math.Abs((double)par1);
            });
            functions.Add("ACOS", (s) =>
            {
                var par1 = s.Pop();
                return Math.Acos((double)par1);
            });
            functions.Add("ASIN", (s) =>
            {
                var par1 = s.Pop();
                return Math.Asin((double)par1);
            });
            functions.Add("ATAN", (s) =>
            {
                var par1 = s.Pop();
                return Math.Atan((double)par1);
            });
            functions.Add("ATAN2", (s) =>
            {
                var par2 = s.Pop();
                var par1 = s.Pop();
                return Math.Atan2((double)par1, (double)par2);
            });
            functions.Add("CEIL", (s) =>
            {
                var par1 = s.Pop();
                return Math.Ceiling((double)par1);
            });
            functions.Add("COS", (s) =>
            {
                var par1 = s.Pop();
                return Math.Cos((double)par1);
            });
            functions.Add("COSH", (s) =>
            {
                var par1 = s.Pop();
                return Math.Cosh((double)par1);
            });
            functions.Add("DEG", (s) =>
            {
                var par1 = (double)s.Pop();
                return par1 * 180 / Math.PI;
            });
            functions.Add("EXP", (s) =>
            {
                var par1 = s.Pop();
                return Math.Exp((double)par1);
            });
            functions.Add("FLOOR", (s) =>
            {
                var par1 = s.Pop();
                return Math.Floor((double)par1);
            });
            functions.Add("LOG", (s) =>
            {
                var par1 = s.Pop();
                return Math.Log((double)par1);
            });
            functions.Add("LOG10", (s) =>
            {
                var par1 = s.Pop();
                return Math.Log10((double)par1);
            });
            functions.Add("MAX", (s) =>
            {
                var par2 = (double)s.Pop();
                var par1 = s.Pop();
                return Math.Max((double)par1, (double)par2);
            });
            functions.Add("MIN", (s) =>
            {
                var par2 = (double)s.Pop();
                var par1 = s.Pop();
                return Math.Min((double)par1, (double)par2);
            });
            functions.Add("POW", (s) =>
            {
                var par2 = s.Pop();
                var par1 = s.Pop();
                return Math.Pow((double)par1, (double)par2);
            });
            functions.Add("RAD", (s) =>
            {
                var par1 = (double)s.Pop();
                return par1 * Math.PI / 180;
            });
            functions.Add("ROUND", (s) =>
            {
                var par1 = s.Pop();
                return Math.Round((double)par1);
            });
            functions.Add("SIGN", (s) =>
            {
                var par1 = s.Pop();
                return Math.Sign((double)par1);
            });
            functions.Add("SIN", (s) =>
            {
                var par1 = s.Pop();
                return Math.Sin((double)par1);
            });
            functions.Add("SINH", (s) =>
            {
                var par1 = s.Pop();
                return Math.Sinh((double)par1);
            });
            functions.Add("SQRT", (s) =>
            {
                var par1 = s.Pop();
                return Math.Sqrt((double)par1);
            });
            functions.Add("TAN", (s) =>
            {
                var par1 = s.Pop();
                return Math.Tan((double)par1);
            });
            functions.Add("TANH", (s) =>
            {
                var par1 = s.Pop();
                return Math.Tanh((double)par1);
            });
            functions.Add("TRUNC", (s) =>
            {
                var par1 = s.Pop();
                return Math.Truncate((double)par1);
            });

            vars["PI"] = Math.PI;
            vars["E"] = Math.E;
            vars["PI180"] = Math.PI / 180;
        }

        public void AddFunction(string name, Func<Stack, double> func)
        {
            functions[name] = func;
        }

        public void SetVariable(string name, double value)
        {
            vars[name] = value;
        }

        public double GetVariable(string name)
        {
            return vars[name];
        }

        public Dictionary<string, double> GetVariables()
        {
            return vars;
        }

        public List<CalculationHistoryItem> GetHistory()
        {
            return history;
        }
    }

}
