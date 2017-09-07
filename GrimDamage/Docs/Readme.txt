Discord:
	https://discord.gg/PJ87Ewa
	Will be used for beta testers, feature suggestions etc.
	Don't intend to share it widely on the forum until its fleshed out

Unmapped / uncharted territory:
* Get player name from ID [ingame name]
* Get mob name from record string
* Get player death from in-game
* Get player heals from in-game [useful?]
* Get player class form in-game? [for pets]


TODO:
* Map pets to players (ex: records/skills/playerclass06/pets/winddevil_05.dbr) .. stat name "description"
* Detect death and automatically show what killed you
* Is the dps correct? verify with dot etc (initial reports says yes, for lowbie toons)
<Discord link in form
<Try to read all bytes from gd.arc and load names on start? FIle.readallbytes -- if not, hardcode?
<json.pets
: show one graph with pets? 
save/load from js, in json format?
import knockoutjs?


Nice to have:
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