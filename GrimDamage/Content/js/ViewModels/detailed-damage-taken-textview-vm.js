//

/// <reference path="data/Database.js" />

class DetailedDamageTakenTextViewModel {
    constructor(database) {
        /// <summary>Responsible for rendering the player deaths view</summary>  
        var self = this;
        this.entries = ko.observableArray([]);
        this.database = database;
        this.start = 0;
        this.end = 2527282800000;
        this.timestampOrderAscending = true;
        this.swapTimestampOrder = function() {
            self.timestampOrderAscending = !self.timestampOrderAscending;
            self.update();
        }
    }

    setTimeperiod(start, end) {
        this.start = start;
        this.end = end;
    }

    update() {
        const damageEntries = database.getDamageTaken(this.start, this.end);
        const locations = database.getPlayerLocation(this.start, this.end);
        const result = this.filter(damageEntries, locations);

        if (this.timestampOrderAscending)
            this.entries(result);
        else
            this.entries(result.reverse());

    }


    createEntry(locations, total, damageType, timestamp) {
        const options = { hour: '2-digit', minute: '2-digit', second: '2-digit', hour12: false };
        const date = new Date(timestamp);

        const locationCandidates = locations.filter(x => x.timestamp <= timestamp);
        const location = locationCandidates[locationCandidates.length - 1] || 'Unknown';

        return {
            amount: Math.round(total),
            damageType: damageType,
            timestamp: date.toLocaleTimeString('en-us', options) + '.' + (`000${date.getMilliseconds()}`).slice(-3),
            location: location.location
        }
    }

    filter(data, locations) {
        if (data.length === 0) {
            return [];
        }

        const result = [];

        let total = 0;
        let damageType = data[0].damageType;
        let ts = data[0].timestamp;


        for (let idx = 0; idx < data.length; idx++) {
            if (data[idx].timestamp - ts <= 1 && data[idx].damageType === damageType) {
                total += data[idx].amount;
            } else {
                result.push(this.createEntry(locations, total, damageType, ts));
                
                damageType = data[idx].damageType;
                total = data[idx].amount;
                ts = data[idx].timestamp;
            }
        }
        
        // Last hit won't be recorded, since the else never triggers
        result.push(this.createEntry(locations, total, damageType, ts));

        return result;
    }
    
}