// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Classes


class DeathTrackerViewModel {
    constructor() {
        /// <summary>Responsible for rendering the player deaths view</summary>  
        var self = this;
        this.deaths = ko.observableArray([]);
        this.detailedDamagePoints = ko.observableArray([]);

        this.showDeath = function(death) {
            console.log(self.detailedDamagePoints);
            console.log(self.detailedDamagePoints());
            self.detailedDamagePoints([]);
            console.log(death);

            // This is really unfortunate, but i have no better solution yet.
            const hardcodedClassVarName = 'deathTrackerViewModel';
            data.requestData(TYPE_DETAILED_DAMAGE_TAKEN,
                death.timestamp.toString(),
                death.entityId,
                `${hardcodedClassVarName}.detailedDamagePoints`);
        }
    }

    add(death) {
        /// <summary>Add a death</summary>  
        var label = `Died at ${new Date(death.timestamp).toLocaleTimeString()}`;
        this.deaths.push({
            label: label,
            timestamp: death.timestamp,
            entityId: death.entityId
        });
    }
}