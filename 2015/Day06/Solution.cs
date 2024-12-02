namespace AdventOfCode.Y2015.Day06;

using System;
using System.Linq;

[ProblemName("Probably a Fire Hazard")]
class Solution : Solver {
    public object PartOne(string input) {
        var manager = new LightManagement();
        var lights = input.Split("\n");
        foreach (var light in lights) {
            manager.ApplyCommand(light);
        }

        return manager.LightCount();
    }

    public object PartTwo(string input) {
        var manager = new LightManagement();
        var lights = input.Split("\n");
        foreach (var light in lights) {
            manager.ApplyCommand(light);
        }

        return manager.LightGlobalIntensity();
    }
}

public class LightManagement {
    private readonly bool[,] m_lights = new bool[1000, 1000];
    private readonly int[,] m_lightIntensity = new int[1000, 1000];

    public void ApplyCommand(string command) {
        var commandPar = ParseCommand(command);

        switch (commandPar.Type) {
            case LightSetType.Toggle:
                for (int i = commandPar.x1; i < commandPar.x2 + 1; i++) {
                    for (int j = commandPar.y1; j < commandPar.y2 + 1; j++) {
                        m_lights[i, j] = !m_lights[i, j];
                        m_lightIntensity[i, j] += 2;
                    }
                }

                break;

            case LightSetType.TurnOn:
                for (int i = commandPar.x1; i < commandPar.x2 + 1; i++) {
                    for (int j = commandPar.y1; j < commandPar.y2 + 1; j++) {
                        m_lights[i, j] = true;
                        m_lightIntensity[i, j]++;
                    }
                }

                break;

            case LightSetType.TurnOff:
                for (int i = commandPar.x1; i < commandPar.x2 + 1; i++) {
                    for (int j = commandPar.y1; j < commandPar.y2 + 1; j++) {
                        m_lights[i, j] = false;
                        if (m_lightIntensity[i, j] >= 1) {
                            m_lightIntensity[i, j]--;
                        }
                    }
                }

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(command));
        }
    }

    public LightSet ParseCommand(string command) {
        var lightSet = new LightSet();
        var tokens = command.Split(" ");
        int currentToken;

        if (tokens[0] == "toggle") {
            lightSet.Type = LightSetType.Toggle;
            currentToken = 0;
        } else {
            lightSet.Type = tokens[1] == "on" ? LightSetType.TurnOn : LightSetType.TurnOff;
            currentToken = 1;
        }

        currentToken++;
        var xy1 = tokens[currentToken].Split(",");
        lightSet.x1 = int.Parse(xy1[0]);
        lightSet.y1 = int.Parse(xy1[1]);

        currentToken++;
        currentToken++;

        var xy2 = tokens[currentToken].Split(",");
        lightSet.x2 = int.Parse(xy2[0]);
        lightSet.y2 = int.Parse(xy2[1]);

        return lightSet;
    }

    public int LightCount() {
        return m_lights.Cast<bool>().Count(light => light);
    }

    public int LightGlobalIntensity() {
        return m_lightIntensity.Cast<int>().Sum();
    }
}

public enum LightSetType {
    TurnOn,
    TurnOff,
    Toggle
}

public record LightSet {
    public LightSetType Type { get; set; }
    public int x1 { get; set; }
    public int x2 { get; set; }
    public int y1 { get; set; }
    public int y2 { get; set; }
}
