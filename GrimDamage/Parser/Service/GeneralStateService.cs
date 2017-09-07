using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrimDamage.Parser.Model;

namespace GrimDamage.Parser.Service {
    class GeneralStateService {
        private List<GrimState> _states = new List<GrimState>();

        public void PushState(GrimState state) {
            _states.Add(state);
        }

        public List<GrimState> GetAndClearStates() {
            var states = _states;
            _states = new List<GrimState>();
            return states;
        }
    }
}
