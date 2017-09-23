

class DamageTakenPieHandler {
    constructor(damageTakenPieChart, pieTimespan) {
        this.damageTakenPieChart = damageTakenPieChart;
        this.totalDamageTaken = {};
        this.pieTimespan = pieTimespan;
    }

    addDamageTaken(elements) {
        // Add new data
        for (let idx = 0; idx < elements.length; idx++) {
            const elem = elements[idx];
            const dmg = elem.amount;
            const type = elem.damageType;

            /* Pie chart of total damage recieved */
            if (!isNaN(dmg) && type !== 'Total' && dmg > 0) {
                if (!this.totalDamageTaken.hasOwnProperty(type)) {
                    this.totalDamageTaken[type] = [];
                }

                this.totalDamageTaken[type].push(elem);
            }
        }


        // Sum data
        let totals = {};
        let sumTotal = 0.01;
        for (let type in this.totalDamageTaken) {
            let sum = 0;
            for (let idx = 0; idx < this.totalDamageTaken[type].length; idx++) {
                sum += this.totalDamageTaken[type][idx].amount;
            }

            totals[type] = Math.round(sum);
            sumTotal += Math.round(sum);
        }
        console.log('totals', totals, sumTotal);
        

        // Create chart points and update chart
        let data = [];
        for (let type in totals) {
            data.push({
                name: type,
                y: 100 * totals[type] / sumTotal,
                label: totals[type]
            });
        }
        console.log('setting data', data);
        this.damageTakenPieChart.series[0].setData(data, true);


        // Delete old entries
        const expiration = new Date().getTime() - this.pieTimespan;
        console.log("Exp:", expiration);
        for (let type in this.totalDamageTaken) {
            console.log(this.totalDamageTaken[type]);
            let currentLength = this.totalDamageTaken[type].length;
            this.totalDamageTaken[type] = this.totalDamageTaken[type].filter((e) => e.timestamp < expiration);
            if (this.totalDamageTaken[type].length < currentLength) {
                console.debug(`Removed ${currentLength - this.totalDamageTaken[type].length} expired entries for ${type}`);
            }
        }
    }
}