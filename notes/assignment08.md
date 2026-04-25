# ASSIGNMENT 08 README

## Feedback from Alpha Release and Response
For our alpha release, four playtesters took on our alpha release, each given 15 minutes to playtest and provide feedback. While testing, we noted feedback and commentary from each playtester, the time they took to complete each floor, and any observations from the developers on points of struggles and strengths. All the players noted that our character movement was too slow and the jumps were too floaty, making gameplay progress feel sluggish; the same was said about some of our enemies, with the chase speed of our dustbunny and binarybug being too slow and easy to outrun and the vacuum projectiles being really slow. The other major issue that was pointed out was the dustbunny AI, which was stationary the whole time and didn't attack the player. With most of our playtesters, they were able to successfully make it past the first floor within the first ten minutes of playtesting. The exception is one player, who managed to find a bug with our phasable walls forcing players upwards, and they were able to climb out of the levels and completed the entire game in under those ten minutes. Players mentioned that they thoroughly enjoyed the creative aspects of our game, such as the music, the level design, and mob variety. They also left some fun ideas on how to turn certain bugs into comprehensible features.
    
As observers, we noticed a few smaller details about where the playtesters struggled. First, when jumping onto the platforms, players were getting stuck onto the walls, edges, and corners, especially on the stairs; this ties into how a couple players were able to spam jump while stuck on the walls, which allowed for cheesing the levels. Another issue that came up was the difficulty of the parkour, which the slow player speed and jump height contributed to; players were a bit confused by the phasable walls and the overall lack of instruction in the maze-like structure. Another thing we noticed is that the camera clips through the wall, which made some parts of the parkour more difficult that it should have been, namely the staircases. The aggression range for the bunny needed to be increased, as they were pretty stationary for the whole time. All the playtesters were curious as to what the signs leading up to the trap room could possibly mean, despite the signs spelling out the possibility of the danger. Following the feedback from our alpha playtesting session, we divided up tasks based on what needs improvement and who has preference and the strengths to tackle that implementation.


## Beta Release
### Shaders.
#### Josephine.
The shader construct implementation I did was changing the material of the phasable wall whenever players gain the ability to phase through it. Previously, the wall would just disappear and then reappear two seconds later. Now, the material of the wall goes from solid into a more transparent, glass like material. This allows players to look through and see the other side before fully committing to the "phase." This material is more reflective and transparent, which takes advantage of the lighting around the map.

The additional shaders added were to the walls, where I added more textures and three-dimensionalness to the environment. The shaders help the walls act like fur, so the map looks more like the inside of a cat tree. 
#### Hunter.


#### Johnnie.

### Writing.
#### Opening Screen.

#### Credits Screen.


### UI Improvements.
#### Josephine.

The UI improvements I worked on was improving general gameplay and fixing bugs. Specifically, for one, I fixed the issue with the dustbunny AI malfunctioning; the pathfinding when chasing the player and the shooting mechanism needed to be fixed, and I fixed the aggression range for the bunnies on top of the sound levels for the binary bugs. I also changed some of the parkour so they aren't as difficult; in continuation of this, I also modified the player' speed and jump height, which helps speed up the progress of the game and makes the parkour easier. Another improvement is the scaling for the user interface of the canvas containing the hearts and the identifier for the energy orb; this also included adding the number counter for the lives, so there is a visual for the number of lives players have left. Another addition is adding more textures and instructions on the walls for the players to follow, as well as adding more sound effects to the game, such as sounds for sounds for when the players are losing a life, using the energy orb, and jumping. These textures and sounds add a bit more life into the game. Another small addition that makes the damage more noticeable for players is adding a damage overlay that flashes the screen red for a moment, and paired with the cat hissing when damaged. 
#### Hunter.


#### Johnnie.
