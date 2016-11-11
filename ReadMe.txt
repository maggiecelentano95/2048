Author: Maggie Celentano
Game: 2048

My game behaves the same way an ordinary 2048 game would (minus the sliding animation of the tiles). So, the number of rows/columns in the grid, the tile spawning system, the matching system, and the scoring system should be the same as in any other ordinary 2048 game. 

If the user can no longer make any moves, the game pauses for a few seconds (in order to show the user the state of the game), and then a Game Over screen is presented. 

When the user reaches 2048, the game automatically ends, telling the user that they have won.

I used Pokemon pictures to make the game look a bit more fun, but the numbers are added onto the pictures to prevent confusion when playing the game. I like the way the color orange looks, so I used all fire-type Pokemon and cascading everything against black. The graphics/UI is a bit different than what you’d see in other 2048 games, but I liked the look of it. 

If you want to test if the winning functionality works (since winning this game takes forever and is a pain), you can go into my SpawnTile function in GridController.cs and change variable “id” to have a value in a random range of higher values. e.g:
id = Random.Range (7, 9);


