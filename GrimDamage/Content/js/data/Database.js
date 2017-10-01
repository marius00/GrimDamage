

class Database {
    constructor() {
        this.detailedDamageTaken = [];

        this.detailedDamageDealt = [];
        this.detailedDamageDealtMaxTimestamp = 0;

        this.resists = [];
        this.resistsMaxTimestamp = 0;

        this.playerLocation = [
            {
                timestamp: new Date().getTime(),
                location: 'Unknown'
            }
        ];

        this.entities = {};
        this.entitiesRaw = [];
    }

    addResists(elements) {
        /// <summary>Add new "resist" entries to the DB</summary>
        this.resists = this.resists.concat(elements);
        for (let idx = 0; idx < elements.length; idx++) {
            if (elements[idx].timestamp > this.resistsMaxTimestamp) {
                this.resistsMaxTimestamp = elements[idx].timestamp;
            }
        }
    }

    getHighestResistTimestamp() {
        return this.resistsMaxTimestamp;
        
    }

    getHighestDamageDealtTimestamp() {
        return this.detailedDamageDealtMaxTimestamp;
    }

    addDetailedDamageDealt(elements) {
        /// <summary>Add new "detailed damage dealt" entries to the DB</summary>
        this.detailedDamageDealt = this.detailedDamageDealt.concat(elements);
        for (let idx = 0; idx < elements.length; idx++) {
            if (elements[idx].timestamp > this.detailedDamageDealtMaxTimestamp) {
                this.detailedDamageDealtMaxTimestamp = elements[idx].timestamp;
            }
        }
    }

    addDetailedDamageTaken(elements) {
        /// <summary>Add new "detailed damage taken" entries to the DB</summary>
        this.detailedDamageTaken = this.detailedDamageTaken.concat(elements);
    }

    setEntities(entities) {
        /// <summary>Set a new list of entities which currently exists</summary>
        for (let idx = 0; idx < entities.length; idx++) {
            this.entities[entities[idx].id] = entities[idx];
        }

        this.entitiesRaw = entities;
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

    getDamageDealt(start, end) {
        /// <summary>Get all the damage-dealt events in a given timespan</summary>
        /// <param name="start" type="Epoch">The start period (exclusive)</param>
        /// <param name="end" type="Epoch">The start period (inclusive)</param>
        /// <returns type="[{timestamp: 0, amount: 123.1, damageType: 'chaos'}]"></returns>

        return this.detailedDamageDealt.filter((e) => e.timestamp > start && e.timestamp <= end);
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
        /// <returns type="Object">{name: 'Peter Pan'}</returns>

        return this.entities[entityId] || { name: 'Unknown' };
    }

    getMainPlayerEntityId() {
        /// <summary>Get the entity ID for the main player</summary>
        /// <returns type="Numeric">ID of the player, or best guess if uncertain.</returns>
        const b = this.entitiesRaw;
        return b.filter(p => p.isPrimary).map(p => p.id)[0]
            || b.filter(p => p.type === 'Player').map(p => p.id)[0]
            || b.map(p => p.id)[0];
    }


    reset() {
        // NOOP
    }
}