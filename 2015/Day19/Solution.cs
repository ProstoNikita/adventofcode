using System.Collections;

namespace AdventOfCode.Y2015.Day19;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("Medicine for Rudolph")]
class Solution : Solver {
    private HashSet<string> m_medicine = [];
    private Dictionary<string, List<string>> m_medicines = new();
    private string m_originalMedicine;
    private Dictionary<string, List<string>> m_medicinesReverse = new();
    private Dictionary<string, int> m_shortestPathToMedicine = new();
    private List<(string, List<string>)> m_medicinePaths = new();

    public object PartOne(string input) {
        foreach (var line in input.Split("\n")) {
            var tokens = line.Split(" => ");
            if (tokens.Length == 2) {
                if (m_medicines.TryGetValue(tokens[0], out var value)) {
                    value.Add(tokens[1]);
                } else {
                    m_medicines.Add(tokens[0], [tokens[1]]);
                }
            }

            if (string.IsNullOrEmpty(line)) {
                continue;
            }

            m_originalMedicine = line;
        }

        foreach (var switchVal in m_medicines) {
            var occurrences = m_originalMedicine.AllIndexesOf(switchVal.Key);

            foreach (var occurrence in occurrences) {
                foreach (var medicineToChange in switchVal.Value) {
                    m_medicine.Add(m_originalMedicine.Remove(occurrence, switchVal.Key.Length)
                        .Insert(occurrence, medicineToChange));
                }
            }
        }

        return m_medicine.Count;
    }

    public object PartTwo(string input) {
        foreach (var medicinePair in m_medicines) {
            foreach (var molecule in medicinePair.Value) {
                if (m_medicinesReverse.TryGetValue(molecule, out var value)) {
                    value.Add(medicinePair.Key);
                } else {
                    m_medicinesReverse.Add(molecule, [medicinePair.Key]);
                }
            }
        }


        foreach (var (key, value) in m_medicinesReverse) {
            m_medicinePaths.Add((key, value));
        }

        m_medicinePaths.Sort((x, y) => x.Item1.Length.CompareTo(y.Item1.Length) * -1);

        return TryCreateMoleculeBackwardsCycle(m_originalMedicine);
    }

    private int TryCreateMoleculeBackwardsCycle(string medicine) {
        var iteration = 0;
        while (medicine.ToCharArray().Any(e => e != 'e')) {
            foreach (var switchVal in m_medicinePaths) {
                var occurrences = medicine.AllIndexesOf(switchVal.Item1);
                if (occurrences.Count == 0) {
                    continue;
                }

                iteration++;
                medicine = medicine.Remove(occurrences[0], switchVal.Item1.Length)
                    .Insert(occurrences[0], switchVal.Item2[0]);
            }
        }
        
        return iteration;
    }


    private int TryCreateMoleculeBackwards(string medicine, int iteration) {
        // Console.WriteLine($"Trying to create molecule {medicine} backwards for {iteration}");

        if (string.IsNullOrEmpty(medicine)) {
            return iteration;
        }

        if (medicine.ToCharArray().All(c => c == 'e')) {
            Console.WriteLine(medicine);
            Console.WriteLine(iteration + 1);
            return iteration + medicine.Length;
        }

        var min = int.MaxValue;
        var used = false;
        foreach (var switchVal in m_medicinePaths) {
            // Console.WriteLine(switchVal.Item1 + ": ");
            var occurrences = medicine.AllIndexesOf(switchVal.Item1);
            if (occurrences.Count != 0) { used = true; }

            foreach (var occurrence in occurrences) {
                foreach (var medicineToChange in switchVal.Item2) {
                    min = Math.Min(min,
                        TryCreateMoleculeBackwards(
                            medicine.Remove(occurrence, switchVal.Item1.Length).Insert(occurrence, medicineToChange),
                            iteration + 1));
                }
            }

            if (used) { break; }
        }

        return min;
    }

    private int TryCreateMolecule(string molecule, int iteration) {
        if (molecule.Length > m_originalMedicine.Length) {
            return int.MaxValue;
        }

        if (molecule == m_originalMedicine) {
            return iteration;
        }

        var min = int.MaxValue;
        foreach (var switchVal in m_medicines) {
            if (switchVal.Key == "e") {
                foreach (var medicineToChange in switchVal.Value) {
                    min = Math.Min(min,
                        TryCreateMolecule(new StringBuilder(molecule).Append(medicineToChange).ToString(),
                            iteration + 1));
                }

                continue;
            }

            if (molecule == "") { continue; }

            var occurrences = molecule.AllIndexesOf(switchVal.Key);

            foreach (var occurrence in occurrences) {
                foreach (var medicineToChange in switchVal.Value) {
                    min = Math.Min(min,
                        TryCreateMolecule(
                            molecule.Remove(occurrence, switchVal.Key.Length).Insert(occurrence, medicineToChange),
                            iteration + 1));
                }
            }
        }

        // Console.WriteLine(min);
        return min;
    }
}

public static class StringExtension {
    public static List<int> AllIndexesOf(this string str, string value) {
        if (String.IsNullOrEmpty(value))
            throw new ArgumentException("the string to find may not be empty", "value");
        List<int> indexes = new List<int>();
        for (int index = 0;; index += value.Length) {
            index = str.IndexOf(value, index, StringComparison.Ordinal);
            if (index == -1)
                return indexes;
            indexes.Add(index);
        }
    }
}
