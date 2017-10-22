/*
 Main functionality of this class is:
 1) Get recent pet damage
 2) Add it to the internal listing
 3) Delete old entries from the internal listing
 4) Populate list of recent pets and their damage dealt
 5) Create a total damage dealt by pets
*/
class PetContainerViewModel {
    constructor(database) {
        /// <summary>Responsible for tracking player pets</summary>  
        this.database = database;
        this.pets = ko.observableArray([]);
        this.includePets = ko.observable(false);
        this.petLifetime = 25 * 1000;

        this.petDamage = [];
        this.lastRunTime = 0;
        this.totalPetDamage = 0;

        const self = this;
        this.togglePetInclusion = function() {
            self.includePets(!self.includePets());
        }

        this.dpsCounter = 0;
    }

    update() {
        this.dpsCounter = Math.min(25, this.dpsCounter + 1);

        // Fetch new pet damage
        const newPetDamage = this.database.getPetDamage(this.lastRunTime, TimestampEverything);
        console.debug('Requested pet damage for ', this.lastRunTime, TimestampEverything, 'got', newPetDamage);
        this.petDamage = this.petDamage.concat(newPetDamage);

        // Set new timestamp
        if (newPetDamage.length > 0) {
            try {
                this.lastRunTime = Enumerable.From(newPetDamage).Max(e => e.timestamp) || this.lastRunTime;
            } catch (ex) {
                console.error('Got an error', ex, 'while fetching previous timestamp');
            }
        }

        // Remove old entries
        this.petDamage = this.petDamage.filter(p => p.timestamp > this.lastRunTime - this.petLifetime);

        // Calculate damage per pet
        const self = this;
        const aggregated = Enumerable.From(this.petDamage)
            .GroupBy(
                m => m.entityId,
                null,
                (e, g) => { return { name: self.database.getEntity(e).name, amount: Math.round(Enumerable.From(g.source).Sum(p => p.amount) / this.dpsCounter) }; }
            )
            .OrderByDescending(x => x.amount)
            .Take(6)
            .ToArray();

        this.pets(aggregated);
        

        console.log(aggregated);
        // Calculate total pet damage
        this.totalPetDamage = Enumerable.From(aggregated).Sum(x => x.amount);

        // Insert the damage back in as "player dealt"
        if (this.includePets()) {
            for (let idx = 0; idx < newPetDamage.length; idx++) {
                const d = newPetDamage[idx];
                database.addDetailedDamageDealt([
                    {
                        victimId: d.victimId,
                        damageType: 'Pet/Minion',
                        amount: d.amount,
                        timestamp: d.timestamp
                    }
                ]);
            }
        }
    }


}