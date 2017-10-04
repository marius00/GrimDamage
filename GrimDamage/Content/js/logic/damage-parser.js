// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Classes
class DamageParser {
    constructor(damageDoneStepChart) {
        this.damageDoneStepChart = damageDoneStepChart;
        let dataTable = $('#bosstable').DataTable({
            "columns": [
                { "data": "encountered"},
                { "data": "name" }
            ]
        });
        $('#bosstable tbody').on('click', 'tr', this.showboss);
        this.dataTable = dataTable;
        this.players = [];
        this.bosses = {};
        this.modals = new Modals();

        this.bosschart = this.modals.addBossModal();
    }

    tick(players, damageDealt, detailedDamageDealt, detailedDamageTaken, entitiesList) {
        this.players = players; // Just a convenience so we don't need to call getMainPlayerId(players)

        this.dataReceived(damageDealt); // TODO: OBSOLETE!

        this.handleEntitiesList(entitiesList);
        this.handleDetailedDamageTaken(detailedDamageTaken);
        this.handleDetailedDamageDealt(detailedDamageDealt);
    }

    showboss() {
        let bossid = this.id;
        if (bossid === undefined || bossid == "" || !p.bosses.hasOwnProperty(bossid)) {
            return;
        }

        var dealtTemp = p.bosses[bossid]['dealt'];
        var dealt = Object.keys(dealtTemp).map(function (data) {
            return { name: data, y: dealtTemp[data], color: colors.color(data) };
        });

        var takenTemp = p.bosses[bossid]['taken'];
        var taken = Object.keys(takenTemp).map(function (data) {
            return {name: data, y: takenTemp[data], color: colors.color(data)};
        });
        p.bosschart.setTitle({ text: p.bosses[bossid].name });
        p.bosschart.series[0].setData(dealt);
        p.bosschart.series[1].setData(taken);
        p.modals.show('bossmodal');
    }

    /*
     * When a new entitiy is found, and it's a boss type we need to do:
     * 1. Add row to the database
     * 2. Add entry in this.boss
     * 3. Sum data for this.boss
     */
    handleEntitiesList(list) {
        let length = list.length;
        for (let c = 0; c < length; c++) {
            /* check if id is already set */
            let entity = list[c];
            if (entity.type !== 'Monster' && entity.type !== 'Player' && entity.type !== 'Environmental' && entity.type !== 'Pet') {
                if (!this.bosses.hasOwnProperty(entity.id)) {
                    this.bosses[entity.id] = [];
                    this.bosses[entity.id]['type'] = entity.type;
                    this.bosses[entity.id]['name'] = entity.name;
                    this.bosses[entity.id]['dealt'] = [];
                    this.bosses[entity.id]['taken'] = [];
                    const now = moment(new Date());
                    this.dataTable.row.add({ "DT_RowId": entity.id, "encountered": now.format('HH:mm:ss'), "name": entity.name }).draw(false);
                }
            }
        }
    }

    handleDetailedDamageTaken(data) {
        for (let playerid in data) {
            if (!data.hasOwnProperty(playerid) || playerid != this.mainPlayerId) {
                continue;
            }
            const length = data[playerid].length;
            for (let c = 0; c < length; c++) {
                const dmg = data[playerid][c];
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
    }


    handleDetailedDamageDealt(data) {
        //console.log("Playerid: " + this.mainPlayerId);
        //console.log(data);
        for (let playerid in data) {
            if (!data.hasOwnProperty(playerid) || playerid != this.mainPlayerId) {
                continue;
            }

            const length = data[playerid].length;
            for (let c = 0; c < length; c++) {
                const entry = data[playerid][c];

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
    }
    // TODO: Move to database
    get mainPlayerId() {
        return this.players.filter(p => p.isPrimary).map(p => p.id)[0] || this.players.map(p => p.id)[0];
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
        const playerId = this.mainPlayerId;
        if (playerId && damageDealt[playerId]) {
            this.addDamageDealt(playerId, damageDealt);
        }
    }

}