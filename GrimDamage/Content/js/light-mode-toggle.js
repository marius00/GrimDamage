

class LightModeToggleViewModel {
    constructor(enabled) {
        this.enabled = ko.observable(enabled);
        let self = this;
    
        this.toggleDarkMode = () => {
            if (self.enabled()) {
                console.log('Loading "Light mode"');
                setCsharpLightMode('light');
                document.location = 'index.html';
            } else {
                console.log('Loading "Dark mode"');
                setCsharpLightMode('dark');
                document.location = 'index.html?DarkMode=1';
            }
        }

        this.label = ko.observable(enabled ? 'Light mode' : 'Dark mode');
        console.log(`Initialize as ${enabled ? 'Dark mode' : 'Light mode'}`);
    }
};