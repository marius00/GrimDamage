// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Classes


class PauseTracker {
    constructor() {
        /// <summary>Responsible for tracking game pause state</summary>  
        
        this.state = PauseTracker.Unpaused;
        this.highestTimestamp = 0;
    }

    static get Paused() {
        return 'paused';
    }

    static get Unpaused() {
        return 'unpaused';
    }

    process(events) {
        /// <summary>Processes events and checks for game pause events</summary>  
        /// <param name="events" type="Array">Events retrieved from Grim Dawn [{timestamp:0, event: 'Unpause'}]</param>  

        for (let i = 0; i < events.length; i++) {
            const event = events[i];

            if (event.timestamp > this.highestTimestamp) {
                if (event.event === 'Unpause') {
                    this.state = PauseTracker.Unpaused;
                } else if (event.event === 'Pause') {
                    this.state = PauseTracker.Paused;
                }
            }

            this.highestTimestamp = Math.max(this.highestTimestamp, event.timestamp);
        }
    }

    get isPaused() {
        /// <returns type="Boolean">If parsing should be paused</returns>  
        return this.state === PauseTracker.Paused;
    }
    get isActive() {
        /// <returns type="Boolean">If parsing should continue as normal</returns>  
        return this.state === PauseTracker.Unpaused;
    }

}