# ASSIGNMENT 06 README

[VIDEO](https://drive.google.com/file/d/1z9mtuwO5YByXTfsnjEdD66ob2nzm_jty/view?usp=sharing)

## AI CONSTRUCTS 

The design of these AI constructs contribute to the gameplay and theme by implementing both stationary and mobile enemies for the player to avoid. By utilizing the different strategies of these enemies, players have a wider range of circumstances to avoid other than the general parkour and artifact collection. The models and animations are used to represent items that follow the 'cat in a cattree' idea, and each enemy has unique systems that pose different challenges.   

Josephine:

* Binary Bug AI - digital scent that follows the player, with only two modes: chase or wait
* Binary Bug mecanim - three different animations that play when the bug is waiting/idle, chasing the player, and attacking within range of the player.
* Player mecanim - player does a running animation when walking and goes idle when no button is pressed

Hunter:

* Vacuum Sentry AI - FSM that shoots a projectile at the player and runs a randomization script of what to do if it hits the player
* Vacuum Sentry mecanim - sentry has an animation when idle

Johnnie: 

* Dust Bunny AI - A* pathfinding, random roaming, chasing player when detected, and shooting dustballs at the player when at range
* Dust Bunny mecanim - bunny has an animation when idle and chasing the player


## PHYSICS CONSTRUCTS

* energy orbs have a particle to highlight their location
* the bouncepads bounce players (and other entities) upwards
* particles coming out of the sentry projectiles to trace their path


## LIGHTS 

* lights inside the yarnball and energy orbs signify their importance to collect
* lights inside the dustballs are red to signify danger and to avoid them
* lights inside the sentries are to highlight their locations

## TEXTURES

* textures on the cylinders are used to fit with the theme of scratchposts in a cattree
* textures on the walls and the danger sign dictate the correct/incorrect direction that the players should continue/avoid
* the energy orbs are coins with catfaces to signify their usefulness but also a skull to represent the two-sidedness of quantum
* the yarnball is added to differentiate the main objective from others
* bouncepads have a different texture to differentiate them from regular parkour platforms (and are a nod to slime blocks, which are bouncy)