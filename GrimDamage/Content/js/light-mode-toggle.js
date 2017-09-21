

class LightModeToggleViewModel {
    constructor(enabled) {
        this.enabled = ko.observable(enabled);
        let self = this;
    
        this.toggleDarkMode = () => {
            if (self.enabled()) {
                setCsharpLightMode('light');
                document.location = 'index.html';
            } else {
                setCsharpLightMode('dark');
                document.location = 'index.html?DarkMode=1';
            }
        }

        this.label = ko.observable(enabled ? 'Light mode' : 'Dark mode');
    }
};