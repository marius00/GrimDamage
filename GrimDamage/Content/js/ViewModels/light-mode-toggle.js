

class LightModeToggleViewModel {
    constructor(enabled, toggleCallback) {
        this.enabled = ko.observable(enabled);
        const self = this;
        this.toggleCallback = toggleCallback;
        this.label = ko.observable(enabled ? 'Light mode' : 'Dark mode');
        this.defaultTheme = $.extend(true, {}, Highcharts.getOptions(), {});


    
        this.toggleDarkMode = () => {
            if (self.enabled()) {
                console.debug('Loading "Light mode"');
                this.resetOptions();
                self.toggleCallback('light');
                setCsharpLightMode('light'); // Store user setting
                self.enabled(false);
                this.label('Dark mode');
            } else {
                console.debug('Loading "Dark mode"');
                self.toggleCallback('dark');
                setCsharpLightMode('dark'); // Store user setting
                self.enabled(true);
                this.label('Light mode');
            }
        }

        console.log(`Initialize as ${enabled ? 'Dark mode' : 'Light mode'}`);
    }

    resetOptions() {
        // Fortunately, Highcharts returns the reference to defaultOptions itself
        // We can manipulate this and delete all the properties
        const defaultOptions = Highcharts.getOptions();
        for (let prop in defaultOptions) {
            if (typeof defaultOptions[prop] !== 'function') delete defaultOptions[prop];
        }
        // Fall back to the defaults that we captured initially, this resets the theme
        Highcharts.setOptions(this.defaultTheme);
    }
};