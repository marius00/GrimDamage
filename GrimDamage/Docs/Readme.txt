Discord:
	https://discord.gg/PJ87Ewa
	Will be used for beta testers, feature suggestions etc.
	Don't intend to share it widely on the forum until its fleshed out

Unmapped / uncharted territory:
* Player name would be useful if we start saving parses
* Get player heals from in-game [useful?]		-- unknown
* Get player class[es] from in-game? [for pets]		-- unknown, suspect very tricky  // mastery

TODO:
* [PHP] Finish the upload functionality
* [JS] Maybe add a list of pets with checkboxes, where the player can choose which are his, and merge those into his aoe/st damage? [obs: per name? pets die pretty often]
* [JS] Automatically show what killed you
* [JS] show one graph with pets? 
* [JS] Now possible to save/load parses from .JS ==> Need the JS integration
* [JS] Use knockout for something? global states?
* [JS] An issue with switching to NULL for consecutive 0s is that the sequence will start from null, not from 0, creating odd clipped graphs
* [C#] Linklabel to download IA, if its not already installed.
* [C#/PHP] Was thinking maybe a news thing, just a link to gog/steam when there's a sale or wtv.. unsure if it provides any value, but being able to announce stuff to users (while being non intrusive) might be nice.

Nice to have:
* HP line on damage taken graph, max and current life.
* Show damage blocked (shield)
* show stats of enemies, off, def, etc..
* How much dps from racial?
* In .js, if you 've gone X updates without doing any damage, might as well tag him as out-of-combat, and stop updating the graph
* "War Cry" reduction % to health does not register at all, should it?
* Finding a way to show defensive actions like absorbs and heals would be "nice to have"
* Showing which zone the player enters / leaves  ( https://www.highcharts.com/demo/combo-timeline )

Graphs planned:
* What killed me? (area / line graphs?)
* My damage (aoe/total and single-target)
* Comparison between players in MP ?


Rant:
Could these be useful?
^bRacial Bonus Damage:  (0.000000)
criticalStrike = 1.000000

What about these? Is dodge useful? Dodge doesn't contain a damage rating, but maybe some builds depends on it? (??)
2017-09-05 20:39:43,654 [9] DEBUG GrimDamage.GD.Processors.MessageProcessorCore Unrecognized log:     PTH Missed Hit
2017-09-05 20:39:43,654 [9] DEBUG GrimDamage.GD.Processors.MessageProcessorCore Unrecognized log:     ^Attacker Missed Attack

What about this? It does not show which debuff, but we know "some debuff" has been applied:
combatType = Debuff Attack