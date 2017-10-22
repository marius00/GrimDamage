using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrimDamage.GUI.Browser {
    public enum DataRequestType {
        States = 1,
        DetailedDamageTaken = 2,
        DetailedDamageDealt = 3,
        SimpleDamageTaken = 4, // WARNING: 
        SimpleDamageDealt = 5, // WARNING: 
        EntityHealth = 6,
        FetchResists = 7,
        FetchEntities = 8,
        FetchLocations = 9,
        FetchPetDamageDealt = 10,
    }
}
