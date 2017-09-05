using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GrimDamage.Parser.Config;
using GrimDamage.Parser.Model;
using GrimDamage.Parser.Service;

namespace GrimDamage.GD.Logger {
    public class CombatFileReader {
        private readonly List<string> _events;
        private readonly DamageParsingService _damageParsingService;
        private Dictionary<int, List<string>> _processedEvents;
        private int _currentIndex;

        public CombatFileReader(DamageParsingService damageParsingService, List<string> events) {
            _events = events;
            _damageParsingService = damageParsingService;
        }

        public static bool IsValid(string[] dataset) {
            int _;
            foreach (var line in dataset) {
                string[] result = Regex.Split(line, @"^(\d+) ");
                if (result.Length != 3)
                    return false;


                if (!int.TryParse(result[1], out _))
                    return false;
            }

            return true;
        }

        public void Next() {
            // Initialize
            if (_processedEvents == null) {
                _processedEvents = new Dictionary<int, List<string>>();
                foreach (var line in _events) {
                    string[] result = Regex.Split(line, @"^(\d+) ");
                    int idx = int.Parse(result[1]);

                    if (!_processedEvents.ContainsKey(idx))
                        _processedEvents[idx] = new List<string>();

                    _processedEvents[idx].Add(result[2]);
                }
            }

            // Process
            if (_processedEvents.ContainsKey(_currentIndex)) {
                foreach (var entry in _processedEvents[_currentIndex]) {
                    Process(entry);
                }
            }

            _currentIndex++;
        }

        private void Process(string data) {
            var match = EventMapping.RegexMap[EventType.DamageDealt].Match(data);
            if (match.Success) {
                double dmg = double.Parse(match.Groups[1].Value.Replace(".", ","));
                int victim = int.Parse(match.Groups[2].Value, NumberStyles.HexNumber);
                string damageType = match.Groups[3].Value;
                _damageParsingService.ApplyDamage(dmg, victim, damageType);
                return;
            }

            match = EventMapping.RegexMap[EventType.SetAttackerName].Match(data);
            if (match.Success) {
                string name = match.Groups[1].Value;
                _damageParsingService.SetAttackerName(name);
            }

            match = EventMapping.RegexMap[EventType.SetDefenderName].Match(data);
            if (match.Success) {
                string name = match.Groups[1].Value;
                _damageParsingService.SetDefenderName(name);
            }

            match = EventMapping.RegexMap[EventType.SetAttackerId].Match(data);
            if (match.Success) {
                int id = int.Parse(match.Groups[1].Value);
                _damageParsingService.SetAttackerId(id);
            }

            match = EventMapping.RegexMap[EventType.SetDefenderId].Match(data);
            if (match.Success) {
                int id = int.Parse(match.Groups[1].Value);
                _damageParsingService.SetDefenderId(id);
            }
        }

        
    }
}
