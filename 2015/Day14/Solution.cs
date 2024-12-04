namespace AdventOfCode.Y2015.Day14;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("Reindeer Olympics")]
class Solution : Solver {

    public object PartOne(string input) {
        var race = new Race();
        var iteration = 2503;
        foreach (var reindeerInfo in input.Split('\n')) {
            var tokens = reindeerInfo.Split(' ');
            race.AddReindeer(tokens[0], int.Parse(tokens[3]), int.Parse(tokens[6]), int.Parse(tokens[13]));
        }

        for (int i = 0; i < iteration; i++) {
            race.Tick();
        }
        
        // race.Print();
        return race.GetMaxCurrentKilometer();
    }

    public object PartTwo(string input) {
        var race = new Race();
        var iteration = 2503;
        foreach (var reindeerInfo in input.Split('\n')) {
            var tokens = reindeerInfo.Split(' ');
            race.AddReindeer(tokens[0], int.Parse(tokens[3]), int.Parse(tokens[6]), int.Parse(tokens[13]));
        }

        for (int i = 0; i < iteration; i++) {
            race.Tick();
            race.AwardPoints();
        }
        
        // race.Print();
        return race.GetMaxPoints();
    }
}

public class Race {
    private readonly List<Reindeer> m_reindeers = [];

    public void AddReindeer(string name, int topSpeed, int speedTime, int restTime) {
        var reindeer = new Reindeer {
            Name = name,
            TopSpeed = topSpeed,
            SpeedTime = speedTime,
            RestTime = restTime,

            CurrentKilometer = 0,
            CurrentSecond = 0,
            IsResting = true,
        };
        
        m_reindeers.Add(reindeer);
    }

    public void Print() {
        m_reindeers.ForEach(Console.WriteLine);
    }

    public void Tick() {
        m_reindeers.ForEach(r => r.Tick());
    }

    public void AwardPoints() {
        var groups = m_reindeers.GroupBy(reindeer => reindeer.CurrentKilometer, reindeer => reindeer).ToList();
        groups.Sort((r1, r2) => r1.Key.CompareTo(r2.Key) * -1);
        groups[0].ToList().ForEach(r => r.WinPoints++);
    }

    public int GetMaxCurrentKilometer() {
        return m_reindeers.Select(x => x.CurrentKilometer).Max();
    }

    public int GetMaxPoints() {
        return m_reindeers.Select(x => x.WinPoints).Max();
    }
}

public record Reindeer {
    public string Name { get; set; }
    public int TopSpeed { get; set; }
    public int SpeedTime { get; set; }
    public int RestTime { get; set; }

    public int CurrentKilometer { get; set; }
    public int CurrentSecond { get; set; }
    public bool IsResting { get; set; }
    
    public int WinPoints { get; set; }

    public void Tick() {
        if (CurrentSecond == 0) {
            IsResting = !IsResting;
            CurrentSecond = IsResting ? RestTime : SpeedTime;
        }

        if (!IsResting) {
            CurrentKilometer += TopSpeed;
        }

        CurrentSecond--;
    }
}
