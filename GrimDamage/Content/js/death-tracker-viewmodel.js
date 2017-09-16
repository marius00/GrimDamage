// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Classes


class DeathTrackerViewModel {
    constructor() {
        /// <summary>Responsible for rendering the player deaths view</summary>  
        this.deaths = ko.observableArray([]);

    }
    
    showDeath(death) {
        console.log(death);
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