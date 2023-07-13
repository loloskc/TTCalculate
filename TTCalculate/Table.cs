using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTCalculate
{
    internal class Table:MathLogic
    {

        private int CountLetter(string ex)
        {
            char[] letters = ex.ToCharArray();
            int countLetter = 0;
            for (int i = 0; i < letters.Length; i++)
            {
                if (char.IsLetter(letters[i])) countLetter++;
            }
            return countLetter;
        }

        private void MakeTableForControl(DataGridView table, string exp)
        {
            table.Rows.Clear();
            table.Columns.Clear();
            table.RowCount = Convert.ToInt32(Math.Pow(2, Convert.ToDouble(CountLetter(exp))));
            table.ColumnCount = CountLetter(exp) + 1;
            for (int i = 0; i < table.ColumnCount; i++)
            {
                table.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                table.Columns[i].HeaderText = Convert.ToChar('A' + i).ToString();

            }
            table.Columns[table.ColumnCount - 1].HeaderText = "Итог";
        }

        private void PrintExpressionResult(string expression, bool[,] truthTable,DataGridView table,string ex)
        {
            int numRows = truthTable.GetLength(0);
            int numVariables = truthTable.GetLength(1);
            for(int i = 0;i<numRows;i++)
            {
                for(int j = 0; j < numVariables; j++)
                {
                    char variable = (char)('A' + j);
                    expression = expression.Replace(variable.ToString(), truthTable[i,j]? "T":"F");
                }
                bool result = EvaluateExpression(expression);
                table[table.ColumnCount - 1, i].Value = Convert.ToInt32(result);
                expression = ex;
            }
        }

        private bool[,] BuildTruthTable(int numVariables)
        {
            int numRows = (int)Math.Pow(2, numVariables);
            bool[,] table = new bool[numRows, numVariables];
            for(int i =0;i<numRows;i++)
            {
                for(int j = 0; j < numVariables; j++)
                {
                    int cells = 1 << (numVariables - j - 1);
                    table[i, j] = (i & cells) != 0;
                }
            }
            return table;
        }

        public void Maketable(DataGridView table, string expression)
        {
            MakeTableForControl(table, expression);
            int numVariables = CountLetter(expression);
            bool[,] truthTable = BuildTruthTable(numVariables);
            LoadTable(table, expression);
            PrintExpressionResult(expression, truthTable, table, expression);
        }

        private void LoadTable(DataGridView table, string exp)
        {

            for (int i = 0; i < table.RowCount; i++)
            {
                bool[] result = new bool[CountLetter(exp)];
                for (int j = 0; j < CountLetter(exp); j++)
                {
                    table.Rows[i].Cells[j].Value = Convert.ToInt32(result[j] = (i & (1 << (CountLetter(exp) - j - 1))) > 0);

                    table.Rows[i].Cells[j].ReadOnly = true;
                    table.Rows[i].Cells[j + 1].ReadOnly = true;
                }
            }
        }

    }
}
