using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GrimDamage.Parser.Model;
using GrimDamage.Statistics.dto;
using Newtonsoft.Json;

namespace GrimDamage.GUI.Browser {
    public class WebViewJsInteractor {
        private readonly JsonSerializerSettings _settings;
        private readonly WebViewJsPojo _js;

        public WebViewJsInteractor(WebViewJsPojo js) {
            _settings = new JsonSerializerSettings {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Culture = System.Globalization.CultureInfo.InvariantCulture,
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            };

            _js = js;

            var docEntity = new EntityJson {
                Id = 123,
                Name = "Peter pan",
                Type = "Player",
                Health = 123,
                IsPrimary = false
            };

            var docDamageEntryJson = new DamageEntryJson {
                DamageType = "Lightning",
                Amount = 9381
            };

            List<string> documentation = new List<string>();
            documentation.Add($"Example values for {nameof(_js.stateChangesJson)}:");
            documentation.Add(Serialize(new List<string> { GrimState.Unpause.ToString(), GrimState.BeginStun.ToString() }));
            documentation.Add("");

            documentation.Add($"Example values for {nameof(_js.playerLocationName)}:");
            documentation.Add(Serialize("Peter fucking griffin"));
            documentation.Add("");

            documentation.Add($"Example values for {nameof(_js.damageTakenJson)}:");
            documentation.Add(Serialize(
                new Dictionary<int, List<DamageEntryJson>> {
                {default(int), new List<DamageEntryJson> { docDamageEntryJson } }
            }));
            documentation.Add("");

            documentation.Add($"Example values for {nameof(_js.damageDealtJson)}:");
            documentation.Add(Serialize(
                new Dictionary<int, List<DamageEntryJson>> {
                    {default(int), new List<DamageEntryJson> { docDamageEntryJson } }
                })
            );
            documentation.Add("");

            documentation.Add($"Example values for {nameof(_js.detailedDamageTakenJson)}:");
            documentation.Add(Serialize(
                new Dictionary<int, List<DetailedDamageEntryJson>> {
                    {default(int), new List<DetailedDamageEntryJson> { new DetailedDamageEntryJson {
                        DamageType = "Chaos",
                        Amount = 123,
                        AttackerId = 555
                    } } }
                })
            );
            documentation.Add("");

            documentation.Add($"Example values for {nameof(_js.damageDealtToSingleTargetJson)}:");
            documentation.Add(Serialize(
                new Dictionary<int, List<DamageEntryJson>> {
                    {default(int), new List<DamageEntryJson> { docDamageEntryJson } }
                })
            );
            documentation.Add("");

            documentation.Add($"Example values for {nameof(_js.playersJson)}:");
            documentation.Add(Serialize(new List<EntityJson> { docEntity }));
            documentation.Add("");

            documentation.Add($"Example values for {nameof(_js.entitiesJson)}:");
            documentation.Add(Serialize(new List<EntityJson> { docEntity }));
            documentation.Add("");

            documentation.Add($"Example values for {nameof(_js.petsJson)}:");
            documentation.Add(Serialize(new List<EntityJson> { docEntity }));
            documentation.Add("");

            documentation.Add("\r\n\r\nThe following methods are exposed:");
            MethodInfo[] methodInfos = typeof(WebViewJsPojo).GetMethods(BindingFlags.Public | BindingFlags.Instance);
            foreach (var method in methodInfos) {
                var parameters = method.GetParameters();
                var parameterDescriptions = string.Join
                (", ", method.GetParameters()
                    .Select(x => x.ParameterType + " " + x.Name)
                    .ToArray());

                if (!method.Name.Contains('_')) {
                    documentation.Add($"{method.ReturnType} {method.Name}({parameterDescriptions})");
                }
            }

            documentation.Add("\r\n=== END OF DOCUMENTATION ===");

            _js.api = string.Join("\r\n\t", documentation);
        }

        private string Serialize(object o) {
            return JsonConvert.SerializeObject(o, _settings);
        }

        public void SetStateChanges(List<GrimState> value) {
            _js.stateChangesJson = JsonConvert.SerializeObject(value.Select(m => m.ToString()).ToList(), _settings);
        }

        public void SetPlayerLocation(string location) {
            _js.playerLocationName = location;
        }

        public void SetDamageTaken(Dictionary<int, List<DamageEntryJson>> value) {
            _js.damageTakenJson = JsonConvert.SerializeObject(value, _settings);
        }


        public void SetDamageDealt(Dictionary<int, List<DamageEntryJson>> value) {
            _js.damageDealtJson = JsonConvert.SerializeObject(value, _settings);
        }

        public void SetDetailedDamageTaken(Dictionary<int, List<DetailedDamageEntryJson>> value) {
            _js.detailedDamageTakenJson = JsonConvert.SerializeObject(value, _settings);
        }

        public void SetDamageDealtToSingleTarget(Dictionary<int, List<DamageEntryJson>> value) {
            _js.damageDealtToSingleTargetJson = JsonConvert.SerializeObject(value, _settings);
        }

        public void SetPlayers(List<EntityJson> value) {
            _js.playersJson = JsonConvert.SerializeObject(value, _settings);
        }

        public void SetEntities(List<EntityJson> value) {
            _js.entitiesJson = JsonConvert.SerializeObject(value, _settings);
        }

        public void SetPets(List<EntityJson> value) {
            _js.petsJson = JsonConvert.SerializeObject(value, _settings);
        }


    }
}
