using AngleSharp.Html;

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
        manager.CalculateCircuit();
        manager.PrintCircuit();

        return manager.GetValue("a");
    }

    public object PartTwo(string input) {
        return 0;
    }
}

public class CircuitManager {
    private Dictionary<string, ushort> circuit = new();
    private InstructionTree m_instructionTree;
    
    public CircuitManager() {
        m_instructionTree = new InstructionTree(this);
    }

    public ushort GetValue(string wireName) {
        circuit.TryGetValue(wireName, out var value);
        return value;
    }

    public void PrintCircuit() {
        foreach (var wire in circuit) {
            Console.WriteLine(wire.Key + ": " + wire.Value);
        }
    }

    public void AddToTree(Instruction instruction) {
        m_instructionTree.AddInstruction(instruction);
    }

    public void CalculateCircuit() {
        m_instructionTree.CalculateWires();
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
    private readonly CircuitManager m_manager;
    private readonly IDictionary<string, WireNode> m_nodes = new Dictionary<string, WireNode>();
    private readonly IList<string> m_rootWires = new List<string>();

    public InstructionTree(CircuitManager circuitManager) {
        m_manager = circuitManager;
    }

    public void AddInstruction(Instruction instruction) {
        if (instruction.Operation == Operation.DIRECTVALUE) {
            m_rootWires.Add(instruction.Output);
        }

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


    public void CalculateWires() {
        var queue = new Queue<string>();
        foreach (var wire in m_rootWires) {
            queue.Enqueue(wire);
        }

        while (queue.Count > 0) {
            var wire = queue.Dequeue();
            Console.WriteLine(wire);
            var node = m_nodes[wire];

            if (node.Instruction == null) {
                node.IsVisited = true;
                continue;
            }

            if (node.Parents.Any(p => !m_nodes[p].IsVisited)) {
                queue.Enqueue(node.Name);
                continue;
            }
            
            foreach (var childWire in node.Children) {
                queue.Enqueue(childWire);
            }
            
            m_manager.ApplyInstruction(node.Instruction);
            node.IsVisited = true;
        }
    }
}

public record WireNode {
    public bool IsVisited { get; set; } = false;
    public string Name { get; set; }
    public Instruction Instruction { get; set; }
    public IList<string> Children { get; set; }  = new List<string>();
    
    public IList<string> Parents { get; set; } = new List<string>();
    
}
