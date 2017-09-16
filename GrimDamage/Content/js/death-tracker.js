// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Classes


class DeathTracker {

    constructor(playerTracker) {
        /// <summary>Responsible for tracking player deaths</summary>  
        this.deaths = [];
        this.minInterval = 8000;
        this.playerTracker = playerTracker;
    }

    process(events) {
        /// <summary>Processes events and checks for player deaths, if the player has died the entry is stored</summary>  
        /// <param name="events" type="Array">Events retrieved from Grim Dawn [{timestamp:0, event: 'Death'}]</param>  

        for (let i = 0; i < events.length; i++) {
            const event = events[i];
            if (event.event === 'Dead') {
                // Minor cooldown on deaths, since the alert can come multiple times
                if (event.timestamp > this.lastDeath + this.minInterval) {
                    let entityId = this.playerTracker.mainPlayerId;
                    if (entityId) {
                        this.deaths.push({
                            timestamp: event.timestamp,
                            entityId: this.playerTracker.mainPlayerId
                        });
                    } else {
                        console.log('Death detected but no player found');
                    }
                }
            }
        }
    }

    get lastDeath() {
        /// <returns type="Number">The most recent timestamp of a player death.</returns>  
        return Math.max.apply(Math, this.deaths.map(function (o) { return o.timestamp; }));
    }

}