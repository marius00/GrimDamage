// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Classes
class DamageParser {
    constructor(damageDoneStepChart, database) {
        this.damageDoneStepChart = damageDoneStepChart;
        /*let dataTable = $('#bosstable').DataTable({
            "columns": [
                { "data": "encountered"},
                { "data": "name" }
            ]
        });*/
        //$('#bosstable tbody').on('click', 'tr', this.showboss);
        //this.dataTable = dataTable;
        this.players = [];
        this.bosses = {};
        this.modals = new Modals();
        this.database = database;

        this.bosschart = this.modals.addBossModal();


        this.previousTimestampDamageDealt = new Date().getTime();
        this.previousTimestampDamageTaken = new Date().getTime();

        this.bossList = ko.observableArray([{
            type: 'Boss',
            name: 'Test data',
            encountered: moment(new Date()).format('HH:mm:ss')
        }]);

        let self = this;
        this.showboss = function(entry) {
            console.log(entry);
            const bossid = entry.id;
            if (bossid === undefined || bossid == "" || !self.bosses.hasOwnProperty(bossid)) {
                console.debug('showboss cancelled');
                return;
            }

            const dealtTemp = self.bosses[bossid]['dealt'];
            const dealt = Object.keys(dealtTemp).map(function (data) {
                return { name: data, y: dealtTemp[data], color: colors.color(data) };
            });

            let takenTemp = self.bosses[bossid]['taken'];
            const taken = Object.keys(takenTemp).map(function (data) {
                return {name: data, y: takenTemp[data], color: colors.color(data)};
            });
            self.bosschart.setTitle({ text: self.bosses[bossid].name });
            self.bosschart.series[0].setData(dealt);
            self.bosschart.series[1].setData(taken);
            self.modals.show('#bossmodal');
            console.debug('Showing boss');
        }
    }

    tick(damageDealt) {
        this.dataReceived(damageDealt); // TODO: OBSOLETE!

        const detailedDamageDealt = this.database.getDamageDealt(this.previousTimestampDamageDealt, TimestampEverything);
        if (detailedDamageDealt.length > 0) {
            try {
                this.previousTimestampDamageDealt = Enumerable.From(detailedDamageDealt).Max(e => e.timestamp) || this.previousTimestampDamageDealt;
            } catch (ex) {
                console.error('Got an error', ex, 'while fetching previous timestamp');
            }
        }
        
        const detailedDamageTaken = this.database.getDamageTaken(this.previousTimestampDamageTaken, TimestampEverything);
        if (detailedDamageTaken.length > 0) {
            try {
                this.previousTimestampDamageTaken = Enumerable.From(detailedDamageTaken).Max(e => e.timestamp) || this.previousTimestampDamageTaken;
            } catch (ex2) {
                console.error('Got an error', ex2, 'while fetching previous timestamp');
            }
        }

        this.handleEntitiesList();
        this.handleDetailedDamageTaken(detailedDamageTaken);
        this.addDamageDealtToBosses(detailedDamageDealt || []);
    }

    update() {
        const playerId = this.database.getMainPlayerEntityId();
        if (playerId && damageDealt[playerId]) { // TODO:
            this.addDamageDealt(playerId, damageDealt); // TODO:
        } // TODO:

        const detailedDamageDealt = this.database.getDamageDealt(this.previousTimestampDamageDealt, TimestampEverything);
        if (detailedDamageDealt.length > 0) {
            try {
                this.previousTimestampDamageDealt = Enumerable.From(detailedDamageDealt).Max(e => e.timestamp) || this.previousTimestampDamageDealt;
            } catch (ex) {
                console.error('Got an error', ex, 'while fetching previous timestamp');
            }
        }
        

        const detailedDamageTaken = this.database.getDamageTaken(this.previousTimestampDamageTaken, TimestampEverything);
        if (detailedDamageTaken.length > 0) {
            try {
                this.previousTimestampDamageTaken = Enumerable.From(detailedDamageTaken).Max(e => e.timestamp) || this.previousTimestampDamageTaken;
            } catch (ex2) {
                console.error('Got an error', ex2, 'while fetching previous timestamp');
            }
        }

        this.handleEntitiesList();
        this.handleDetailedDamageTaken(detailedDamageTaken);
        this.addDamageDealtToBosses(detailedDamageDealt);
    }

    


    isBossEntity(entity) {
        return entity.type !== 'Monster' &&
            entity.type !== 'Player' &&
            entity.type !== 'Environmental' &&
            entity.type !== 'Pet';
    }

    isNewEntity(entity) {
        return !this.bosses.hasOwnProperty(entity.id);
    }

    /*
     * When a new entitiy is found, and it's a boss type we need to do:
     * 1. Add row to the database
     * 2. Add entry in this.boss
     * 3. Sum data for this.boss
     */
    handleEntitiesList() {
        let entities = this.database.entitiesRaw
            .filter(e => this.isBossEntity(e))
            .filter(e => this.isNewEntity(e))
        ;

        const now = moment(new Date());
        for (let c = 0; c < entities.length; c++) {
            /* check if id is already set */
            let entity = entities[c];
            this.bosses[entity.id] = [];
            this.bosses[entity.id]['type'] = entity.type;
            this.bosses[entity.id]['name'] = entity.name;
            this.bosses[entity.id]['dealt'] = [];
            this.bosses[entity.id]['taken'] = [];
            //this.dataTable.row.add({ "DT_RowId": entity.id, "encountered": now.format('HH:mm:ss'), "name": entity.name }).draw(false);

            this.bossList.push({
                id: entity.id,
                type: entity.type,
                name: entity.name,
                encountered: now.format('HH:mm:ss')
            });
        }
    }

    handleDetailedDamageTaken(data) {
        const length = data.length;
        for (let c = 0; c < length; c++) {
            const dmg = data[c];
            /* Is it a boss? */
            if (this.bosses.hasOwnProperty(dmg.attackerId)) {
                /* First time we're seeing this damage-type on boss? */
                if (!this.bosses[dmg.attackerId]['taken'].hasOwnProperty(dmg.damageType)) {
                    this.bosses[dmg.attackerId]['taken'][dmg.dmamageType] = 0;
                }
                this.bosses[dmg.attackerId]['taken'][dmg.damageType] += Math.round(dmg.amount);
            }
        }
    }


    addDamageDealtToBosses(data) {
        /// <summary>Bosses, add damage dealt for the player</summary>

        const length = data.length;
        for (let c = 0; c < length; c++) {
            const entry = data[c];

            /* Is it a boss? */
            if (this.bosses.hasOwnProperty(entry.victimId)) {
                /* First time we're seeing this damage-type on boss? */
                if (!this.bosses[entry.victimId]['dealt'].hasOwnProperty(entry.damageType)) {
                    this.bosses[entry.victimId]['dealt'][entry.damageType] = 0;
                }

                this.bosses[entry.victimId]['dealt'][entry.damageType] += Math.round(entry.amount);
            }
        }
    }

    addDamageDealt(id, damageDealt) {
        if (damageDealt[id]) {
            /* Add to stepChart */
            let timestamp = (new Date()).getTime();
            for (let c = 0; c < damageDealt[id].length; c++) {
                this.damageDoneStepChart.addPoint(damageDealt[id][c].damageType, timestamp, damageDealt[id][c].amount);
            }
            this.damageDoneStepChart.redraw();
        }
    }



    dataReceived(damageDealt) {
        const playerId = this.database.getMainPlayerEntityId();
        if (playerId && damageDealt[playerId]) {
            this.addDamageDealt(playerId, damageDealt);
        }
    }

}