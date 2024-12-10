namespace AdventOfCode.Y2015.Day18;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("Like a GIF For Your Yard")]
class Solution : Solver {
    private bool[][] m_lights;

    public object PartOne(string input) {
        m_lights = input.Split('\n').Select(l => l.ToCharArray().Select(c => c == '#').ToArray()).ToArray();
        for (var i = 0; i < 100; i++) {
            Tick();
        }
        
        return m_lights.SelectMany(x => x).Count(x => x);
    }

    public object PartTwo(string input) {
        m_lights = input.Split('\n').Select(l => l.ToCharArray().Select(c => c == '#').ToArray()).ToArray();
        m_lights[0][0] = true;
        m_lights[0][^1] = true;
        m_lights[^1][0] = true;
        m_lights[^1][^1] = true;
        for (var i = 0; i < 100; i++) {
            Tick(true);
        }
        
        return m_lights.SelectMany(x => x).Count(x => x);
    }

    private void Tick(bool corners = false) {
        var lights = m_lights.Select(x => x.Select(b => b).ToArray()).ToArray();
        for (int x = 0; x < m_lights.Length; x++) {
            for (int y = 0; y < m_lights[x].Length; y++) {
                lights[x][y] = GetLight(x, y, corners);
            }
        }
        m_lights = lights;
    }

    private bool GetLight(int x, int y, bool corners = false) {
        if (corners) {
            if ((x == 0 || x == m_lights.Length - 1) && (y == 0 || y == m_lights.Length - 1)) {
                return true;
            } 
        }
        
        
        var countNeighbors = 0;
        var xm1 = x - 1 >= 0;
        var ym1 = y - 1 >= 0;
        var xp1 = x + 1 < m_lights.Length;
        var yp1 = y + 1 < m_lights.Length;
        
        if (xm1 && ym1 && m_lights[x - 1][y - 1]) countNeighbors++;
        if (xm1 && m_lights[x - 1][y]) countNeighbors++;
        if (xm1 && yp1 && m_lights[x - 1][y + 1]) countNeighbors++;
        
        if (xp1 && ym1 && m_lights[x + 1][y - 1]) countNeighbors++;
        if (xp1 && m_lights[x + 1][y]) countNeighbors++;
        if (xp1 && yp1 && m_lights[x + 1][y + 1]) countNeighbors++;
        
        if (ym1 && m_lights[x][y - 1]) countNeighbors++;
        if (yp1 && m_lights[x][y + 1]) countNeighbors++;

        if (m_lights[x][y] && countNeighbors is 2 or 3) {
            return true;
        }

        return !m_lights[x][y] && countNeighbors is 3;
    }
}
