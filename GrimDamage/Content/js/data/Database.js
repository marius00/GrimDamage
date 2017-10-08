

class Database {
    constructor() {
        this.detailedDamageTaken = [];
        this.detailedDamageTakenMaxTimestamp = 0;

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

    getHighestDamageTakenTimestamp() {
        return this.detailedDamageTakenMaxTimestamp;
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
        for (let idx = 0; idx < elements.length; idx++) {
            if (elements[idx].timestamp > this.detailedDamageTakenMaxTimestamp) {
                this.detailedDamageTakenMaxTimestamp = elements[idx].timestamp; // TODO :Linq?
            }
        }
    }

    setEntities(entities) {
        /// <summary>Set a new list of entities which currently exists</summary>
        for (let idx = 0; idx < entities.length; idx++) {
            this.entities[entities[idx].id] = entities[idx];
        }

        this.entitiesRaw = entities;
    }

    setPlayerLocation(location) {
        /// <param name="location" type="Array">The current location of the player</param>
        this.playerLocation = location;
    }

    getDamageTaken(start, end) {
        /// <summary>Get all the damage-taken events in a given timespan</summary>
        /// <param name="start" type="Epoch">The start period (exclusive)</param>
        /// <param name="end" type="Epoch">The start period (inclusive)</param>
        /// <returns type="[{timestamp: 0, amount: 123.1, damageType: 'chaos'}]"></returns>

        return this.detailedDamageTaken.filter((e) => e.timestamp > start && e.timestamp <= end);
    }

    getDamageTakenByEntity(entityId) {
        /// <summary>Get all the damage-taken from a given entity</summary>
        /// <param name="entityId" type="Number">ID of the attacking entity</param>
        /// <returns type="[{timestamp: 0, amount: 123.1, damageType: 'chaos'}]"></returns>

        return this.detailedDamageTaken.filter((e) => e.attackerId === entityId);
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
            || b.filter(p => p.name === 'Player').map(p => p.id)[0] // Type is not enough, sometimes we got the type correct but no name.. weird!?
            || b.map(p => p.id)[0];
    }

    getResists(damageType, timestamp) {
        /// <summary>Get resist of the **player** at a given point in time</summary>
        /// <returns type="String">Damage/resist type</returns>
        /// <returns type="Numeric">Timestamp of the occurance</returns>
        try {
            return Enumerable.From(database.resists)
                .Where((x) => x.type === damageType && x.timestamp < timestamp)
                .MaxBy((x) => x.timestamp).amount;
        } catch (e) {
            return 0; // No clue
        }
    }

    getAllResists(start, end) {
        /// <summary>Get resist of the **player** at a given time period</summary>
        try {
            return Enumerable.From(database.resists)
                .Where((x) => x.timestamp >= start && x.timestamp < end)
                .ToArray();
        } catch (e) {
            return [];
        }
    }

    reset() {
        // NOOP
    }
}