class Colors {

    constructor() {
        this.colors = [];
        this.colors['total'] = '#000000';
        this.colors['physical'] = '#eaeaea';
        this.colors['internalTrauma'] = '#4a4a4a';
        this.colors['fire'] = '#ff6300';
        this.colors['burn'] = '#9e3d00';
        this.colors['cold'] = '#00beff';
        this.colors['frostburn'] = '#0088b6';
        this.colors['lightning'] = '#00fffe';
        this.colors['electrocute'] = '#00bfbe';
        this.colors['acid'] = '#26ff00';
        this.colors['poison'] = '#179a00';
        this.colors['vitality'] = '#fcff00';
        this.colors['vitalityDecay'] = '#969800';
        this.colors['pierce'] = '#ff009f';
        this.colors['bleeding'] = '#ff7567';
        this.colors['aether'] = '#89ffbb';
        this.colors['chaos'] = '#720b00';
        this.colors['lifeLeech'] = '#ffbd89';
        this.colors['hitpoints'] = '#ff0000';
        this.colors['raw total'] = '#EBEBEB';
    }

    color(typeRaw) {
        const type = typeRaw.toLowerCase();
        if (this.colors.hasOwnProperty(type)) {
            return this.colors[type];
        }
        else {
            console.log('Unknown color: ' + type);
            return '#000000'.replace(/0/g, function () { return (~~(Math.random() * 16)).toString(16); });
        }
    }
}