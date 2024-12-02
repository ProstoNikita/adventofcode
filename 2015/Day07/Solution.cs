namespace AdventOfCode.Y2015.Day07;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("Some Assembly Required")]
class Solution : Solver {
    public object PartOne(string input) {
        var manager = new CircuitManager();
        var lines = input.Split('\n');
        foreach (var line in lines) {
            var instruction = manager.ParseInstruction(line);
            Console.WriteLine(instruction);
            manager.AddToTree(instruction);
        }
        return manager.GetValue("a");
    }

    public object PartTwo(string input) {
        var manager = new CircuitManager();
        var lines = input.Split('\n');
        foreach (var line in lines) {
            var instruction = manager.ParseInstruction(line);
            if (instruction.Output == "b") {
                instruction.A.Value = 16076;
            }
            manager.AddToTree(instruction);
        }
        return manager.GetValue("a");
    }
}

public class CircuitManager {
    private readonly Dictionary<string, ushort> circuit = new();
    private readonly InstructionTree m_instructionTree;
    
    public CircuitManager() {
        m_instructionTree = new InstructionTree();
    }

    public void PrintCircuit() {
        foreach (var wire in circuit) {
            Console.WriteLine(wire.Key + ": " + wire.Value);
        }
    }

    public void AddToTree(Instruction instruction) {
        m_instructionTree.AddInstruction(instruction);
    }
    
    public ushort GetValue(string wireName) {
        // Check if the value is already computed
        if (circuit.TryGetValue(wireName, out var value)) {
            return value;
        }

        // Retrieve the instruction corresponding to the wire
        if (!m_instructionTree.TryGetInstruction(wireName, out var instruction)) {
            throw new ArgumentException($"Wire {wireName} has no associated instruction.");
        }

        unchecked {
            // Recursively evaluate the instruction based on the operation
            ushort result = instruction.Instruction.Operation switch {
                Operation.NOT => (ushort)~GetValue(instruction.Instruction.A.Name),
                Operation.AND => (ushort)(ResolveInput(instruction.Instruction.A) &
                                          ResolveInput(instruction.Instruction.B)),
                Operation.OR => (ushort)(ResolveInput(instruction.Instruction.A) |
                                         ResolveInput(instruction.Instruction.B)),
                Operation.LSHIFT => (ushort)(GetValue(instruction.Instruction.A.Name) <<
                                             instruction.Instruction.B.Value),
                Operation.RSHIFT => (ushort)(GetValue(instruction.Instruction.A.Name) >>
                                             instruction.Instruction.B.Value),
                Operation.DIRECTVALUE => instruction.Instruction.A.Value,
                Operation.DIRECTWIRE => GetValue(instruction.Instruction.A.Name),
                _ => throw new InvalidOperationException("Unknown operation.")
            };
            circuit[wireName] = result;
            return result;
        }
    }

    private ushort ResolveInput(InstructionToken token) {
        return token.Name == null ? token.Value : GetValue(token.Name);
    }

    public void ApplyInstruction(Instruction instruction) {
        unchecked {
            switch (instruction?.Operation) {
                case Operation.NOT:
                    circuit.TryGetValue(instruction.A.Name, out var circuitValue);
                    circuit.TryAdd(instruction.Output, (ushort)~circuitValue);
                    break;
                case Operation.OR:
                    var orCircuitA = instruction.A.Name == null ? instruction.A.Value : circuit[instruction.A.Name];
                    var orCircuitB = instruction.B.Name == null ? instruction.B.Value : circuit[instruction.B.Name];
                    
                    circuit.TryAdd(instruction.Output, (ushort)(orCircuitA | orCircuitB));
                    break;
                case Operation.LSHIFT:
                    circuit.TryGetValue(instruction.A.Name, out var lShiftCircuitA);
                    circuit.TryAdd(instruction.Output, (ushort)(lShiftCircuitA << instruction.B.Value));
                    break;
                case Operation.RSHIFT:
                    circuit.TryGetValue(instruction.A.Name, out var rShiftCircuitA);
                    circuit.TryAdd(instruction.Output, (ushort)(rShiftCircuitA >> instruction.B.Value));
                    break;
                case Operation.AND:
                    var andCircuitA = instruction.A.Name == null ? instruction.A.Value : circuit[instruction.A.Name];
                    var andCircuitB = instruction.B.Name == null ? instruction.B.Value : circuit[instruction.B.Name];
                    circuit.TryAdd(instruction.Output, (ushort)(andCircuitA & andCircuitB));
                    break;
                case Operation.DIRECTVALUE:
                    circuit.Add(instruction.Output, instruction.A.Value);
                    break;
                case Operation.DIRECTWIRE:
                    circuit.TryGetValue(instruction.A.Name, out var directCircuitA);
                    circuit.TryAdd(instruction.Output, directCircuitA);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public Instruction ParseInstruction(string line) {
        var tokens = line.Split(" ");
        switch (tokens[1])
        {
            case "->" when ushort.TryParse(tokens[0], out _):
                return new Instruction {
                    A = new InstructionToken {
                        Value = ushort.Parse(tokens[0]),
                    },
                    Output = tokens[2],
                    Operation = Operation.DIRECTVALUE
                };
            case "->":
                return new Instruction {
                    A = new InstructionToken {
                        Name = tokens[0]
                    },
                    Output = tokens[2],
                    Operation = Operation.DIRECTWIRE
                };
            case "LSHIFT":
                return new Instruction {
                    A = new InstructionToken {
                        Name = tokens[0],
                    },
                    B = new InstructionToken() {
                        Value = ushort.Parse(tokens[2]),
                    },
                    Output = tokens[4],
                    Operation = Operation.LSHIFT
                };
            case "RSHIFT":
                return new Instruction {
                    A = new InstructionToken {
                        Name = tokens[0],
                    },
                    B = new InstructionToken() {
                        Value = ushort.Parse(tokens[2]),
                    },
                    Output = tokens[4],
                    Operation = Operation.RSHIFT
                };
            case "AND":
            {
                InstructionToken A, B;
            
                if (ushort.TryParse(tokens[0], out _)) {
                    A = new InstructionToken {
                        Value = ushort.Parse(tokens[0]),
                    };
                } else {
                    A = new InstructionToken {
                        Name = tokens[0],
                    };
                }
            
                if (ushort.TryParse(tokens[2], out _)) {
                    B = new InstructionToken {
                        Value = ushort.Parse(tokens[2]),
                    };
                } else {
                    B = new InstructionToken {
                        Name = tokens[2],
                    };
                }
            
                return new Instruction {
                    A = A,
                    B = B,
                    Output = tokens[4],
                    Operation = Operation.AND
                };
            }
            case "OR":
            {
                InstructionToken A, B;
            
                if (ushort.TryParse(tokens[0], out _)) {
                    A = new InstructionToken {
                        Value = ushort.Parse(tokens[0]),
                    };
                } else {
                    A = new InstructionToken {
                        Name = tokens[0],
                    };
                }
            
                if (ushort.TryParse(tokens[2], out _)) {
                    B = new InstructionToken {
                        Value = ushort.Parse(tokens[2]),
                    };
                } else {
                    B = new InstructionToken {
                        Name = tokens[2],
                    };
                }
            
                return new Instruction {
                    A = A,
                    B = B,
                    Output = tokens[4],
                    Operation = Operation.OR
                };
            }
        }

        if (tokens[0] == "NOT") {
            return new Instruction {
                A = new InstructionToken {
                    Name = tokens[1],
                },
                Output = tokens[3],
                Operation = Operation.NOT
            };
        }

        return null;
    }
}

public record Instruction {
    public InstructionToken A { get; set; }
    public InstructionToken B { get; set; } 
    public string Output { get; set; }
    public Operation Operation { get; set; }
}

public enum Operation {
    NOT,
    OR,
    LSHIFT,
    RSHIFT,
    AND,
    DIRECTVALUE,
    DIRECTWIRE
}

public record InstructionToken {
    public string Name { get; set; }
    public ushort Value { get; set; }
}

public class InstructionTree {
    private readonly Dictionary<string, WireNode> m_nodes = new();

    public void AddInstruction(Instruction instruction) {
        if (m_nodes.TryGetValue(instruction.Output, out var node)) {
            node.Instruction = instruction;
        } else {
            node = new WireNode {
                Instruction = instruction,
                Name = instruction.Output,
            };
            m_nodes.Add(instruction.Output, node);
        }

        if (instruction.A?.Name != null) {
            if (!m_nodes.TryGetValue(instruction.A.Name, out _)) {
                m_nodes.Add(instruction.A.Name, new WireNode() {
                    Name = instruction.A.Name,
                });
            }
            m_nodes[instruction.A.Name].Children.Add(node.Name);
            node.Parents.Add(instruction.A.Name);
        }
        
        if (instruction.B?.Name != null) {
            if (!m_nodes.TryGetValue(instruction.B.Name, out _)) {
                m_nodes.Add(instruction.B.Name, new WireNode() {
                    Name = instruction.B.Name,
                });
            }
            m_nodes[instruction.B.Name].Children.Add(node.Name);
            node.Parents.Add(instruction.B.Name);
        }
    }

    public bool TryGetInstruction(string wireName, out WireNode o) {
        return m_nodes.TryGetValue(wireName, out o);
    }
}

public record WireNode {
    public string Name { get; set; }
    public Instruction Instruction { get; set; }
    public List<string> Children { get; } = [];
    public List<string> Parents { get; } = [];
}
