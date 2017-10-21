Discord:
	https://discord.gg/PJ87Ewa
	Will be used for beta testers, feature suggestions etc.

Third party devs:
	Launch with '-devtools' to get JS dev tools

Unmapped / uncharted territory:
* Player name would be useful if we start saving parses
* Get player heals from in-game [useful?]		-- unknown
* Get player class[es] from in-game? [for pets]		-- unknown, suspect somewhat tricky  // mastery

TODO:
* Life line not shown on damage taken
* Resists a bit buggy
* Graph with the players and boss resist, per second (to track debuffs)
* Make the damage taken graph include armor for figuring out physical damage taken
* Minimize to tray
* Don't display times as UTC; use pc local time
** Find out how to fetch resists, and combine resist+blocked+absorbed data into showing real damage taken
** Stop updating graphs and text when the tab is closed
* http://gridstackjs.com/ Would be very flexible
* [JS] Maybe add a list of pets with checkboxes, where the player can choose which are his, and merge those into his aoe/st damage? [obs: per name? pets die pretty often]
* [JS] show one graph with pets? 
* [JS] Now possible to save/load parses from .JS ==> Need the JS integration
* [JS] Stop parsing upon pausing the game (what about entering crossroads? and what about training dummies if so?)
* [VS] Fix the need for config changes to compile
** A player has suggested we show the players current resist when taking damage. Ex 'took 1500 fire dmg, resist was 40%' -- how would we display this in a useful manner?

What killed me popup:
* [JS] if you tab or pause while dying, it will register as another death.


Nice to have:
* Show damage blocked (shield)
* show stats of enemies, off, def, etc.. -- while this is "neat", does it provide ANY value beyond the ingame "chance to hit/crit"?
* How much dps from racial?
* How much damage absorbed from racial?
* In .js, if you 've gone X updates without doing any damage, might as well tag him as out-of-combat, and stop updating the graph
* Finding a way to show defensive actions like absorbs and heals would be "nice to have"
* Fine grained data of the individual attacks [skills], see their portion of your overall damage.
* Fine grained data of the individual defensive procs/heals, see their relevance in keeping you alive.
* Comparison between players in MP ?
** In the future: See how much of your damage dealt was resisted by the enemy?
** -------------: See how much damage you resisted from the enemy?
** [evil]: dont add / start removing damage types not dealt for 'what killed me?' [dont show irrelevant types]
* [PHP] Finish the upload functionality

Rant:
Could these be useful?
^bRacial Bonus Damage:  (0.000000)
criticalStrike = 1.000000

What about these? Is dodge useful? Dodge doesn't contain a damage rating, but maybe some builds depends on it? (??)
2017-09-05 20:39:43,654 [9] DEBUG GrimDamage.GD.Processors.MessageProcessorCore Unrecognized log:     PTH Missed Hit
2017-09-05 20:39:43,654 [9] DEBUG GrimDamage.GD.Processors.MessageProcessorCore Unrecognized log:     ^Attacker Missed Attack

What about this? It does not show which debuff, but we know "some debuff" has been applied:
combatType = Debuff Attack