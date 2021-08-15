The Order of the scenes should be:

	0. Load or Create new Game Config (set unity audio source, gfx quality, and language)

	1. Disclaimer ("i do not own pokemon"/"nintendo own copyright to ip)
GoTo=> 2. Compiled to .exe
GoTo=> 3. Building in Unity
	
	2. Ping server, check if user is up-to-date or needs to download newer project
*	=> #. Display prompt on screen, telling users to get new download, with url link
GoTo=> 3. If skip (optional) download

	3. Load/Check assets exist:
		* language/localization
		* sounds
		* art/models
		* database/sql
		* fonts
GoTo=> 4. After loading files
*	=> #. Display error, log file, and quit application if unplayable

	4. (optional) play animation intro scene
GoTo=> 5. After animation is finished or player skips

	5. Title Screen (press start)
GoTo=> 4. If no response, or cancel/back
GoTo=> 6a. If press start or action
GoTo=> 6b. If hold start, action, and cancel
	
	6a. Menu (new game/load game)
GoTo=> 7. If press new game
GoTo=> 8. If press load game
GoTo=> 5. If press cancel or back
GoTo=> 6b. If select delete
	6b. Delete Save file
GoTo=> 6a. If press cancel or back
GoTo=> 6c. If select options
	6c. Options (configure game settings)
GoTo=> 6d. If select controls
GoTo=> 6a. If press cancel or back
	6d. Controls (display key layout for users)
GoTo=> 6c. If press cancel or back

	7. New Game Intro
GoTo=> 8. After finished or player skips

	8. Main Game (overworld)
GoTo=> 9. If select Bag
GoTo=> 10. If select Party
GoTo=> 11. If select Pokedex
GoTo=> 12. If select TrainerCard
GoTo=> 13. If select Shop Buy/Sell
GoTo=> 14. If select PC Store/Withrdraw
GoTo=> 15. If encounter battle
GoTo=> 6a. If press quit

	9. Bag Screen
GoTo=> 8. If press cancel or back

	10. Party Screen
GoTo=> 8. If press cancel or back

	11. Pokedex Screen
GoTo=> 8. If press cancel or back

	12. Trainer Card Screen
GoTo=> 8. If press cancel or back

	13. Shop Screen
GoTo=> 8. If press cancel or back

	14. PC Screen
GoTo=> 8. If press cancel or back

	15. Battle Scene
GoTo=> 8. If batle end