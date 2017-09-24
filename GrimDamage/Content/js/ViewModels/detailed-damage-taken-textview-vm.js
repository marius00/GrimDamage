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
    }

    setTimeperiod(start, end) {
        this.start = start;
        this.end = end;
    }

    update() {
        this.entries(this.filter(database.getDamageTaken(this.start, this.end)));
    }

    filter(data) {
        if (data.length === 0) {
            return [];
        }

        let result = [];
        const options = { hour: '2-digit', minute: '2-digit', second: '2-digit', hour12: false };

        let total = 0;
        let damageType = data[0].damageType;
        let ts = data[0].timestamp;
        for (let idx = 0; idx < data.length; idx++) {
            if (data[idx].timestamp - ts <= 1 && data[idx].damageType === damageType) {
                total += data[idx].amount;
            } else {
                let date = new Date(ts);
                result.push({
                    amount: Math.round(total),
                    damageType: damageType,
                    timestamp: date.toLocaleTimeString('en-us', options) + '.' + (`000${date.getMilliseconds()}`).slice(-3)
                });
                
                damageType = data[idx].damageType;
                total = data[idx].amount;
                ts = data[idx].timestamp;
            }
        }
        
        // Last hit won't be recorded, since the else never triggers
        {
            let date = new Date(ts);
            result.push({
                amount: Math.round(total),
                damageType: damageType,
                timestamp: date.toLocaleTimeString('en-us', options) + '.' + (`000${date.getMilliseconds()}`).slice(-3)
            });
        }

        return result;
    }
    
}