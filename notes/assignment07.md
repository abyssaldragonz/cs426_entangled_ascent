# ASSIGNMENT 07 README

## SOUND DESIGN 

Josephine:

* Dust Bunny. 
  * When idle and wandering, the dust bunny plays a rabbit squeaking sound; this sound emits from a distance where the player is close to the bunny, but not close enough to trigger an attack. When it is in range of the player and ready to attack, the dust bunny plays a chomping sound to signify danger. Specifically, the sound is from the "killer bunny" from Minecraft.
* Binary Bug. 
  * When idle and chasing, the binary bug plays a buzzing sound with a low mechanical sound underneath it. When in range of the player, it plays a more aggressive buzzing sound when attacking the player. This sound is a lot more sudden and louder than the default sound to signify danger.
* Vacuum Sentry. 
  * The vacuum sentry plays an idle sound of a mechanical hum that matches the crystal model. When in range of the player, it plays a laser shooting sound as it attempts to fire at the player.


Hunter:

* 

Johnnie: 

* 


## UI DESIGN

Josephine:

* The lives billboard breaks the principle of clarity. The lives are displayed with a number in a small font, which does not create a good interface for users to see.

  * The fix for this is to use hearts in place of the small number, to clearly showcase to the players that this is important to keep track of.

* The instructions billboard breaks the principle of feedback. When collecting the coin, the player is unsure about how or where to use it. There are no associations that guide the player to where the object can be used.

  * The fix for this is adding more coins that create the association between the artifact and action. For example, every tunnelable wall has coin particles coming out of it and when collecting the coin, the UI containing the instructions on how to use the artifact has a coin icon to create that connection.

* The player animation continuously moves even when no input keys are pressed down; this breaks the principle of feedback and consistency. If the player animations continuously moves, then the does not receive feedback for when the player stops moving, it creates the wrong association for movement and may confuse the player. 

  * The fix for this was removing the condition "has exit time" on both the trigger conditions for transitioning between the two animation states. An additional condition was needed to be added to the PlayerMovement.cs script to set the trigger using the Animator.SetBool() function, which checks to see which keys are currently held down. 

Hunter:

* 

Johnnie: 

* 
