using System;
using System.Collections.Generic;
using System.Linq;

namespace PokemonUnity
{
	/// <summary>
	/// Used to convert strings into various types.
	/// </summary>
	public class ScriptConversion
	{
		enum Associativity
		{
			Right,
			Left
		}

		/// <summary>
		/// Converts an expression into a <see cref="Double"/>.
		/// </summary>
		public static double ToDouble(object expression)
		{
			string input = expression.ToString();
			bool retError = false;
			double retDbl = 0d;

			retDbl = InternalToDouble(input, ref retError);

			if (!retError)
				return retDbl;
			else if (IsArithmeticExpression(expression))
			{
				string postFix = ToPostfix(expression.ToString(), ref retError);

				return EvaluatePostfix(postFix, ref retError);
			}
			else
				return 0d;
		}

		/// <summary>
		/// If the input can be evaluated into a number.
		/// </summary>
		public static bool IsArithmeticExpression(object expression)
		{
			bool retError = false;
			string postfix = ToPostfix(expression.ToString(), ref retError);
			if (!retError)
			{
				EvaluatePostfix(postfix, ref retError);
				return !retError;
			}
			else
				return false;
		}

		/// <summary>
		/// Evaluates a math expression in the postfix notation. Example: "2 3 +".
		/// </summary>
		/// <param name="input">The postfix string.</param>
		private static double EvaluatePostfix(string input, ref bool hasError)
		{
			List<double> stack = new List<double>();
			List<char> tokens = input.ToCharArray().ToList();

			string cNumber = "";

			while (tokens.Count > 0)
			{
				char token = tokens[0];
				tokens.RemoveAt(0);

				if (IsNumber(token))
					cNumber += token.ToString();
				else if (cNumber.Length > 0)
				{
					stack.Insert(0, InternalToDouble(cNumber, ref hasError));
					cNumber = "";
				}

				if (cNumber.Length > 0 & tokens.Count == 0)
				{
					stack.Insert(0, InternalToDouble(cNumber, ref hasError));
					cNumber = "";
				}

				if (IsOperator(token))
				{
					if (stack.Count >= 2)
					{
						double v2 = stack[0];
						double v1 = stack[1];

						stack.RemoveAt(0);
						stack.RemoveAt(0);

						double result = 0;

						switch (token.ToString())
						{
							case "+":
								{
									result = v1 + v2;
									break;
								}
							case "-":
								{
									result = v1 - v2;
									break;
								}
							case "*":
								{
									result = v1 * v2;
									break;
								}
							case "/":
								{
									if (v2 == 0)
									{
										Game.DebugLog("Script.vb: Cannot evaluate \"" + input.ToString() + "\" as an arithmetic expression.", false);
										hasError = true;
										return 0;
									}
									else
										result = v1 / v2;
									break;
								}
							case "^":
								{
									result = Math.Pow(v1, v2);
									break;
								}
							case "%":
								{
									result = v1 % v2;
									break;
								}
						}

						stack.Insert(0, result);
					}
					else
					{
						Game.DebugLog("Script.vb: Cannot evaluate \"" + input.ToString() + "\" as an arithmetic expression.", false);
						hasError = true;
						return 0;
					}
				}
			}

			if (stack.Count == 1)
				return stack[0];
			else
			{
				Game.DebugLog("Script.vb: Cannot evaluate \"" + input.ToString() + "\" as an arithmetic expression.", false);
				hasError = true;
				return 0;
			}
		}

		/// <summary>
		/// Converts an infix notation to postfix notation.
		/// </summary>
		/// <param name="input">The infix notation. Example: "2+3".</param>
		private static string ToPostfix(string input, ref bool hasError)
		{
			if (input.Trim().StartsWith("-"))
				input = "0" + input;

			List<char> tokens = input.ToCharArray().ToList();
			List<char> stack = new List<char>();

			string output = "";
			string cNumber = "";

			while (tokens.Count > 0)
			{
				char token = tokens[0];
				tokens.RemoveAt(0);

				// Token is a number:
				if (IsNumber(token))
					cNumber += token.ToString();
				else if (cNumber.Length > 0)
				{
					output += cNumber.ToString() + " "; // Add to the output.
					cNumber = "";
				}

				if (cNumber.Length > 0 & tokens.Count == 0)
				{
					output += cNumber.ToString() + " ";
					cNumber = "";
				}

				// Token is an operator:
				if (IsOperator(token))
				{
					char o1 = token;

					while (stack.Count > 0 && IsOperator(stack[0]) && ((GetAssociativity(o1) == Associativity.Left & GetPrecedence(o1) <= GetPrecedence(stack[0])) | (GetAssociativity(o1) == Associativity.Right & GetPrecedence(o1) < GetPrecedence(stack[0]))))
					{
						output += stack[0].ToString() + " ";
						stack.RemoveAt(0);
					}

					stack.Insert(0, o1);
				}
				// Token is a left parenthesis:
				if (token == '(')
					stack.Insert(0, token);
				// Token is a right parenthesis:
				if (token == ')')
				{
					if (stack.Count > 0)
					{
						while (stack.Count > 0)
						{
							if (stack[0] == '(')
							{
								stack.RemoveAt(0);
								break;
							}
							else
							{
								output += stack[0].ToString() + " ";
								stack.RemoveAt(0);
							}
						}
					}
					else
					{
						Game.DebugLog("Script.vb: Cannot convert \"" + input.ToString() + "\" to an arithmetic expression.", false);
						hasError = true;
						return "0";
					}
				}
			}

			while (stack.Count > 0)
			{
				if (stack[0] == '(' | stack[0] == ')')
				{
					Game.DebugLog("Script.vb: Cannot convert \"" + input.ToString() + "\" to an arithmetic expression.", false);
					hasError = true;
					return "0";
				}
				else
				{
					output += stack[0].ToString() + " ";
					stack.RemoveAt(0);
				}
			}

			return output;
		}

		/// <summary>
		/// If the token is a number or part of one.
		/// </summary>
		/// <param name="token">The token.</param>
		private static bool IsNumber(char token)
		{
			return "0123456789.,".ToCharArray().Contains(token);
		}

		/// <summary>
		/// If the token is an operator.
		/// </summary>
		/// <param name="token">The token.</param>
		private static bool IsOperator(char token)
		{
			return "+-*/^%".ToCharArray().Contains(token);
		}

		/// <summary>
		/// Returns the precedence for an operator.
		/// </summary>
		/// <param name="[Operator]">The operator.</param>
		private static int GetPrecedence(char Operator)
		{
			switch (Operator)
			{
				case '+':
				case '-':
					{
						return 2;
					}
				case '*':
				case '/':
				case '%':
					{
						return 3;
					}
				case '^':
					{
						return 4;
					}
			}

			return -1;
		}

		/// <summary>
		/// Returns if an operator has a Left or Right Associativity.
		/// </summary>
		/// <param name="[Operator]">The operator</param>
		private static Associativity GetAssociativity(char Operator)
		{
			switch (Operator)
			{
				case '^':
					{
						return Associativity.Right;
					}
				default:
					{
						return Associativity.Left;
					}
			}
		}

		/// <summary>
		/// Tries to convert a single number into a <see cref="Double"/>.
		/// </summary>
		private static double InternalToDouble(string expression, ref bool hasError)
		{
			expression = expression.Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

			//if (StringHelper.IsNumeric(expression)) 
			//    return System.Convert.ToDouble(expression);
			double i = double.NaN;// null;
			double.TryParse(expression, out i);
			if (!double.IsNaN(i))
				return i;
			else if (expression.ToLower() == "false")
				return 0;
			else if (expression.ToLower() == "true")
				return 1;
			else
			{
				hasError = true;
				return 0;
			}
		}

		/// <summary>
		/// Converts an expression into an <see cref="Integer"/>.
		/// </summary>
		/// <param name="expression">The expression to convert.</param>
		public static int ToInteger(object expression)
		{
			return System.Convert.ToInt32(Math.Round(ToDouble(expression)));
		}

		/// <summary>
		/// Converts an expression to a <see cref="Single"/>.
		/// </summary>
		/// <param name="expression">The expression to convert.</param>
		public static float ToSingle(object expression)
		{
			return System.Convert.ToSingle(ToDouble(expression));
		}

		/// <summary>
		/// Converts an expression into a <see cref="Boolean"/>.
		/// </summary>
		/// <param name="expression">The expression to convert.</param>
		public static bool ToBoolean(object expression)
		{
			switch (expression.ToString().ToLower())
			{
				case "true":
				case "1":
					{
						return true;
					}
				default:
					{
						return false;
					}
			}
		}

		/// <summary>
		/// Performs a check if an expression is a valid <see cref="Boolean"/>.
		/// </summary>
		/// <param name="expression">The expression to perform the check on.</param>
		public static bool IsBoolean(object expression)
		{
			string s = expression.ToString();
			string[] validBools = new[] { "0", "1", "true", "false" };
			return validBools.Contains(s.ToLower());
		}
	}
}