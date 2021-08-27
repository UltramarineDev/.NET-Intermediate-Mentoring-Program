/*
 * Create a class based on ExpressionVisitor, which makes expression tree transformation:
 * 1. converts expressions like <variable> + 1 to increment operations, <variable> - 1 - into decrement operations.
 * 2. changes parameter values in a lambda expression to constants, taking the following as transformation parameters:
 *    - source expression;
 *    - dictionary: <parameter name: value for replacement>
 * The results could be printed in console or checked via Debugger using any Visualizer.
 */
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Expression Visitor for increment/decrement.");
            Console.WriteLine();

            ProcessFirstTask();
            ProcessSecondTask();
        }

        static void ProcessFirstTask()
        {
            var expressions = GetSourceExpressions();
            var translator = new IncDecExpressionVisitor();

            foreach (var expr in expressions)
            {
                var resultExpression = translator.Translate(expr);

                Console.WriteLine($"Source expression: {expr}");
                Console.WriteLine($"Result expression: {resultExpression}");
            }
        }

        static Expression<Func<int, int>>[] GetSourceExpressions()
        {
            return new Expression<Func<int, int>>[]
            {
                x => x + 1,
                x => x - 1,
                x => (x + 1) + (x - 1)
            };
        }

        static void ProcessSecondTask()
        {
            Expression<Func<int, int, int>> expression = (x, y) => (x - 10) + (y + 5);
            var translator = new IncDecExpressionVisitor();

            var pairsToReplace = new Dictionary<string, int>();
            pairsToReplace.Add("x", 10);

            var result = translator.Translate(expression, pairsToReplace);
            Console.WriteLine($"Source expression: {expression}");
            Console.WriteLine($"Result expression: {result}");
        }
    }
}
