

class LightModeToggleViewModel {
    constructor(enabled, toggleCallback) {
        this.enabled = ko.observable(enabled);
        let self = this;
        this.toggleCallback = toggleCallback;
    
        this.toggleDarkMode = () => {
            if (self.enabled()) {
                console.debug('Loading "Light mode"');
                self.toggleCallback('light');
                self.enabled(false);
            } else {
                console.debug('Loading "Dark mode"');
                self.toggleCallback('dark');
                self.enabled(true);
            }
        }

        this.label = ko.observable(enabled ? 'Light mode' : 'Dark mode');
        console.log(`Initialize as ${enabled ? 'Dark mode' : 'Light mode'}`);
    }
};