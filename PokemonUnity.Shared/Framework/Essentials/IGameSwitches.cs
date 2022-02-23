using PokemonUnity.UX;

namespace PokemonEssentials.Interface
{
	public interface IGameSwitches
	{
		bool this[int switch_id] { get; set; }
	}
}

/*
1	Starting over	Set to ON when the player blacks out, and allows special things to happen after this. Should be turned OFF again afterwards.
2	Seen Pokérus in Poké Center	Set to ON once Pokérus has been identified in the Poké Center, and prevents the explanation of what it is from showing again.
3	Choosing starter	Used to determine whether the player is in the middle of choosing a starter.
4	Defeated Gym 1	Used to determine whether the player has defeated the first Gym Leader.
5	Defeated Gym 2	Used to determine whether the player has defeated the second Gym Leader.
6	Defeated Gym 3	Used to determine whether the player has defeated the third Gym Leader.
7	Defeated Gym 4	Used to determine whether the player has defeated the fourth Gym Leader.
8	Defeated Gym 5	Used to determine whether the player has defeated the fifth Gym Leader.
9	Defeated Gym 6	Used to determine whether the player has defeated the sixth Gym Leader.
10	Defeated Gym 7	Used to determine whether the player has defeated the seventh Gym Leader.
11	Defeated Gym 8	Used to determine whether the player has defeated the eighth Gym Leader.
12	Defeated Elite Four	Used to determine whether the player has defeated the Elite Four.
13	Fossil revival in progress	Used to determine whether the Fossil Reviver is currently busy reviving a fossil which the player has given him. Typically turned OFF again in the door event that leads out of the Fossil Reviver's lab.
14	*s:PBDayNight.isDay?	Is ON during the day (i.e. between 5am and 8pm), and OFF otherwise (i.e. during the night).
15	*s:PBDayNight.isNight?	Is ON during the night (i.e. between 8pm and 5am), and OFF otherwise (i.e. during the day).
16	*s:PBDayNight.isMorning?	Is ON during the morning (i.e. between 5am and 10am), and OFF otherwise.
17	*s:PBDayNight.isAfternoon?	Is ON during the afternoon (i.e. between 2pm and 5pm), and OFF otherwise.
18	*s:PBDayNight.isEvening?	Is ON during the evening (i.e. between 5pm and 8pm), and OFF otherwise.
19	*s:pbIsWeekday(-1,2,4,6)	Is ON if the current day is Tuesday, Thursday or Saturday, and OFF otherwise.
20	*s:!pbIsWeekday(-1,2,4,6)	The opposite of Switch 19. Is ON if the current day is Monday, Wednesday, Friday or Sunday, and OFF otherwise.
21	*s:tsOn?("A")	Is ON if the event's temporary switch A is ON, and OFF if the event's temporary switch A is OFF.
22	*s:tsOff?("A")	Is ON if the event's temporary switch A is OFF, and OFF if the event's temporary switch A is ON.
23	*s:cooledDown?(86400)	Is ON if 24 hours (86400 seconds) have passed since the event's variable was set (to the current time then), and if the event's Self Switch A is ON.
24	*s:cooledDownDays?(1)	Is ON if the current day is different to the day on which the event's variable was set (to the current time then), and if the event's Self Switch A is ON.
25	*s:pbInSafari?	Is ON if the player is currently in the Safari Zone, and OFF if they are not.
26	*s:pbBugContestUndecided?	Is ON during a Bug Catching Contest.
27	*s:pbBugContestDecided?	Is ON after a Bug Catching Contest has finished. It is used to begin the judging and to show other Contest participants standing around afterwards.
28	*s:pbInChallenge?	Is ON if the player is currently participating in a Battle Frontier challenge.
29	Has National Dex	Used to determine whether the player has the National Dex. This Switch does not cause the player to have the National Dex, though.
30	*s:pbNextMysteryGiftID>0	Is ON if there is a downloaded Mystery Gift which has not yet been collected, and OFF if there are none.
31	Shiny wild Pokémon	While this Switch is ON, all wild Pokémon encountered will be shiny.
32	Fateful encounters	While this Switch is ON, all Pokémon created will be fateful encounters.
33	No money lost in battle	While this Switch is ON, the player will not lose any money if they lose a wild battle or a trainer battle. They can still gain money from winning trainer battles/using Pay Day, though.
34	No Mega Evolution	While this Switch is ON, no Pokémon can Mega Evolve in battle, not even if they normally could.
35-50	----RESERVED-----	These switches are not currently used, but are marked as reserved in case they need to be used by Essentials in future.
51 onwards	various	Other switches used specifically in the Essentials example maps, which can be overwritten as required.*/