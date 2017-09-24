

class Database {
    constructor() {
        this.detailedDamageTaken = [];
    }

    addDetailedDamageTaken(elements) {
        console.log('adding', elements);
        this.detailedDamageTaken = this.detailedDamageTaken.concat(elements);
    }

    getDamageTaken(start, end) {
        /// <summary>Get all the damage-taken events in a given timespan</summary>
        /// <param name="start" type="Epoch">The start period (exclusive)</param>
        /// <param name="end" type="Epoch">The start period (inclusive)</param>
        /// <returns type="[{timestamp: 0, amount: 123.1, damageType: 'chaos'}]"></returns>

        return this.detailedDamageTaken.filter((e) => e.timestamp > start && e.timestamp <= end);
    }


    reset() {
        // NOOP
    }
}