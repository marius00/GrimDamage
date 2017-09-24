﻿

class Database {
    constructor() {
        this.detailedDamageTaken = [];
        this.playerLocation = [
            {
                timestamp: new Date().getTime(),
                location: 'Unknown'
            }
        ];

        this.entities = {};
    }

    addDetailedDamageTaken(elements) {
        this.detailedDamageTaken = this.detailedDamageTaken.concat(elements);
    }

    setEntities(entities) {
        for (let idx = 0; idx < entities.length; idx++) {
            this.entities[entities[idx].id] = entities[idx].name;
        }
    }

    setPlayerLocation(location) {
        /// <param name="location" type="String">The current location of the player</param>
        if (this.playerLocation.length === 0 || this.playerLocation[this.playerLocation.length - 1].location !== location) {
            this.playerLocation.push({
                timestamp: new Date().getTime(),
                location: location
            });
        }
    }

    getDamageTaken(start, end) {
        /// <summary>Get all the damage-taken events in a given timespan</summary>
        /// <param name="start" type="Epoch">The start period (exclusive)</param>
        /// <param name="end" type="Epoch">The start period (inclusive)</param>
        /// <returns type="[{timestamp: 0, amount: 123.1, damageType: 'chaos'}]"></returns>

        return this.detailedDamageTaken.filter((e) => e.timestamp > start && e.timestamp <= end);
    }

    getPlayerLocation(start, end) {
        /// <summary>Get all the player locations for a given timespan</summary>
        /// <param name="start" type="Epoch">The start period (exclusive)</param>
        /// <param name="end" type="Epoch">The start period (inclusive)</param>
        /// <returns type="Array">[{timestamp: 0, location: 'Crossroads'}]</returns>

        return this.playerLocation.filter((e) => e.timestamp > start && e.timestamp <= end);
    }

    getEntity(entityId) {
        /// <summary>Get the entity for the given entityId</summary>
        /// <param name="entityId" type="Numeric">The entity ID</param>
        /// <returns type="String">Peter Pan</returns>

        return this.entities[entityId] || 'Unknown';
    }


    reset() {
        // NOOP
    }
}