namespace AdventOfCode.Y2024.Day05;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("Print Queue")]
class Solution : Solver {
    private Dictionary<int, List<int>> m_queue = new();
    private List<List<int>> m_toCheck = [];
    private bool[] m_incorrect;


    public object PartOne(string input) {
        foreach (var line in input.Split('\n')) {
            if (line.Length == 0) continue;
            var tokens = line.Split('|');
            if (tokens.Length != 2) {
                m_toCheck.Add(line.Split(",").Select(int.Parse).ToList());
            } else {
                if (m_queue.ContainsKey(int.Parse(tokens[0]))) {
                    m_queue[int.Parse(tokens[0])].Add(int.Parse(tokens[1]));
                } else {
                    m_queue.Add(int.Parse(tokens[0]), [int.Parse(tokens[1])]);
                }
            }
        }

        m_incorrect = new bool[m_toCheck.Count];
        foreach (var line in m_toCheck) {
            for (var i = 0; i < line.Count; i++) {
                if (!m_queue.ContainsKey(line[i])) {
                    continue;
                }

                var order = m_queue[line[i]];
                for (int j = i - 1; j >= 0; j--) {
                    if (!order.Contains(line[j])) {
                        continue;
                    }

                    m_incorrect[m_toCheck.IndexOf(line)] = true;
                    break;
                }

                if (m_incorrect[m_toCheck.IndexOf(line)]) {
                    break;
                }
            }
        }


        return m_incorrect.Select((t, i) => t ? 0 : m_toCheck[i][(m_toCheck[i].Count) / 2]).Sum();
    }

    public object PartTwo(string input) {
        // PartOne(input);
        for (int l = 0; l < m_toCheck.Count; l++) {
            if (!m_incorrect[l]) {
                continue;
            }
            
            
            var fix = false;
            while (!fix) {
                for (var i = 0; i < m_toCheck[l].Count; i++) {
                    if (!m_queue.ContainsKey(m_toCheck[l][i])) {
                        continue;
                    }

                    var order = m_queue[m_toCheck[l][i]];
                    fix = true;
                    for (int j = i - 1; j >= 0; j--) {
                        if (!order.Contains(m_toCheck[l][j])) {
                            continue;
                        }

                        (m_toCheck[l][i], m_toCheck[l][j]) = (
                            m_toCheck[l][j], m_toCheck[l][i]);
                        fix = false;
                        break;
                    }
                }
            }
            
        }

        
        return m_incorrect.Select((t, i) => !t ? 0 : m_toCheck[i][(m_toCheck[i].Count) / 2]).Sum();
    }
}
