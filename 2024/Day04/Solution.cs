namespace AdventOfCode.Y2024.Day04;

using System.Collections.Generic;
using System.Linq;

[ProblemName("Ceres Search")]
class Solution : Solver {
    public char[] WordToCheck = ['X', 'M', 'A', 'S'];

    public object PartOne(string input) {
        var array = input.Split('\n').Select(x => x.ToCharArray()).ToArray();
        return FindAllXmas(array);
    }

    public object PartTwo(string input) {
        var array = input.Split('\n').Select(x => x.ToCharArray()).ToArray();
        return FindAllX_Mas(array);
    }

    public int FindAllXmas(char[][] input) {
        var count = 0;
        for (int i = 0; i < input.Length; i++) {
            for (int j = 0; j < input[0].Length; j++) {
                if (input[i][j] == 'X') {
                    count += FindXmasAt(input, i, j);
                }
            }
        }

        return count;
    }

    public int FindAllX_Mas(char[][] input) {
        var count = 0;
        for (int i = 0; i < input.Length; i++) {
            for (int j = 0; j < input[0].Length; j++) {
                if (input[i][j] == 'A') {
                    count += FindX_MasAt(input, i, j);
                }
            }
        }

        return count;
    }

    private int FindX_MasAt(char[][] input, int i, int j) {
        var chars = new List<char?> {
            GetSymbol(input, i - 1, j + 1),
            GetSymbol(input, i + 1, j + 1),
            GetSymbol(input, i - 1, j - 1),
            GetSymbol(input, i + 1, j - 1),
        };

        if (chars.Any(x => x == null))
            return 0;
        
        if (chars.Count(x => x == 'M') != 2 || chars.Count(x => x == 'S') != 2)
            return 0;
        
        return input[i-1][j-1] != input[i + 1][j + 1] ? 1 : 0;
    }

    public char? GetSymbol(char[][] input, int i, int j) {
        if (i >= input.Length || i < 0 || j >= input[i].Length || j < 0) {
            return null;
        }
        
        return input[i][j];
    }

    public int FindXmasAt(char[][] input, int i, int j) {
        return NextSymbolCheck(input, i, j, 1, 0, 1) +
               NextSymbolCheck(input, i, j, -1, 0, 1) +
               NextSymbolCheck(input, i, j, 1, -1, 1) +
               NextSymbolCheck(input, i, j, -1, -1, 1) +
               NextSymbolCheck(input, i, j, 1, 1, 1) +
               NextSymbolCheck(input, i, j, -1, 1, 1) +
               NextSymbolCheck(input, i, j, 0, -1, 1) +
               NextSymbolCheck(input, i, j, 0, 1, 1);
    }

    public int NextSymbolCheck(char[][] input, int i, int j, int dirX, int dirY, int symbolPos) {
        while (true) {
            if (i + dirX >= input.Length || i + dirX < 0 || j + dirY >= input[i].Length || j + dirY < 0) {
                return 0;
            }

            if (input[i + dirX][j + dirY] != WordToCheck[symbolPos]) {
                return 0;
            }

            if (symbolPos == WordToCheck.Length - 1) {
                return 1;
            }

            i += dirX;
            j += dirY;
            symbolPos += 1;
        }
    }
}
