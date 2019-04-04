# 2D-Robot-Facility
2D Robot Escape Platformer

2D ‘Robot’ Platformer
(Title TBD)

Game Design Document

LCC Capstone productions
Corey | Travis | Jack | Gordon


























Table of Contents 


Section I - Game Overview………………………………………………………………Pg. 5
Game Concept
Features
Genre
Target Audience
Game Flow Summary
User interface
Look and Feel
Project Scope
Locations
Levels
Enemies
Weapons

Section II - Gameplay and Mechanics……………………………………………………Pg. 7
Gameplay
Game Progression
Puzzle Structure
Objective
Play Flow
Mechanics
Physics
Controls
Objects
Picking Up Objects
Moving Objects 
Actions
Switches and Buttons
Combat
Economy
Replaying and Saving

Section III – Story, Setting and Character………………………………………………Pg. 11
Story
Back Story
Plot Elements
Game Progression
Cut Scenes
Game World

Section IV – Technical…………………………………………………………………….Pg.12
Target Hardware
Development Hardware and Software
Development Procedures and Standards
Game Engine
Scripting Language

Section V – Game Art……………………………………………………………………..Pg.13
Concept Art
Style Guides
Characters
Environments
Equipment
Cut Scenes
Miscellaneous












Section I - Game Overview

Game Concept
The game will have the player traveling through a hostile research facility full of sentient AI robots. As you fight your way out you come to learn the reality of your situation. The game’s action will take place in a vertical 2D plane and in real time.


Features
The game will feature numerous different robotic enemies including normal commonly seen enemies and unique boss enemies, various hazards that must be destroyed or avoided, and different environments in multiple levels. The player will acquire a variety of weapons and tool with which to defeat enemies and navigate through levels.

Genres
2D
Platformer
Shooter
Action
Sci-Fi

Target Audience
People who enjoy 2D combat platformers on the PC and possibly consoles. Especially those that grew up playing games like Metroid or Contra.

Game Flow Summary 
The player will go explore a series of levels in the game to get to the end of the game. During their exploration, they will avoid traps and face enemy encounters. 

User Interface
 The user interface will feature a main menu from which you can start a new game, load your game, access the options menu or exit the application. Loading the game will place the player directly into the game world at the location of the save point. During gameplay the player can access the pause menu by pressing ESC. From the pause menu, the player can access the options menu, return to the main menu, or exit the application. 
	The ingame user interface will feature some method of tracking the player’s health or durability, your current weapon, and potentially timers and/or score.



Look and Feel 
The game will start out being bright and fun, like a cartoon, then as the player moves on and discovers more about the post apocalyptic state of the world and that there are no humans left, as well as when they start to encounter more enemies,  it’ll start to get a darker feel to it.

Project Scope 
	The game will take place predominantly in a single building, the research facility as the player discovers their reality and makes their way to the surface. The game will serve as an introduction to a larger concept, though it will be a self contained experience in both narrative and gameplay. 

Locations
The locations will be contained in a larger environment the research facility. The levels will vary from the early depths of the facility that contain pristine surroundings and end at the surface with the facility being reclaimed by overgrown nature.

Levels
The number of levels are undetermined at this point. They will be based on the levels of the building that you explore on your way to the outside.

Enemies
There will be a base of enemies both grounded and flying. We plan to have both light and heavy types of each with varying attacks. There will also be a final boss type that will serve as the finale and reveal the next environment.

Weapons
Weapons will be based on mechanical abilities of the player character. As you progress through the game you will discover upgrades to your default equipment, alternate weapons and additions to your arsenal.




Section II - Gameplay and Mechanics

Gameplay
Gameplay primarily consists of platforming and combat with AI controlled enemies. The player will be challenged by enemies and environmental obstacles and traps impeding their progress. 

Game Progression
As you progress through the game you will find tools and upgrade your equipment in preparation to tackle ever tougher challenges. Each level will have checkpoints at points where significant progress has been made over the course of the level.

Puzzle Structure
Puzzles will consist of traversing environmental traps and challenging platformer obstacles. Timing and planning your route to avoid dangerous hazards will be the largest portion of environmental puzzles. 

Objectives
The objective of the game is to progress through the environment making your way to the surface to escape the building and discover what lies ahead. 

Play Flow
The game will play at a moderate speed of equal parts combat and exploration. The player will traverse both horizontally and vertically with jumping and grappling. Enemy characters will be defeated with melee and/or ranged attacks and environment interaction. The player will encounter some rooms and special areas off the main level.
 
Physics
Some projectiles, the player, and non-flying enemies will be subject to gravity while in the air, pulling them down to the floor. Some attacks will apply knockback to the affected character. If the player uses the grappling hook to tether himself to the ceiling, the player will be able to swing across the room. Moving platforms will physically move the player with them while the player is standing on top of them.


Mechanics
	Player Damage
Enemies damage the player on contact.
Enemies damage the player with projectile attacks.
Enemies damage the player with area-of-effect hitbox based attacks.
Enemies will perform an attack after animating the attack.
After taking damage the player will be invulnerable for a short time. This invulnerability will be indicated to the player through an effect on the character.
When the player takes enough damage it will cause a game over, which will require the player to restart from their last checkpoint or save.
	Enemy Damage
Enemies will take damage from the player’s attacks.
When an enemy takes enough damage they will die.
	Hazards
Some hazards will damage the player on contact.
Some hazards will damage enemies on contact.
Some hazards will have an effect on the player as long as the player is inside them.
Miscellaneous
Basic projectiles will come to a stop upon hitting a wall
Thin platforms will allow the player to jump through them and fall through them while the player is standing on them.

Combat
The game will have the player battle various different enemies in a 2D environment using the robotic character’s unique tool and weapon set. Enemies will often make aggressive use of movement in an attempt to cover the different options the player has and damage the player. Enemies will often be blocking the player’s path forward. Some enemies may be bypassed without combat, but some enemies must be defeated to progress.

Objects
	There will be several objects to interact with in the environment including power-ups, temporary weapons and physics objects that can be grappled and moved within the scene. Objects that are too large to be placed in the players inventory will be lifted overhead, pushed along the ground, moved by some aspect of the player’s equipment, interacted with by pressing the D key, or simply not be a target of player interactions.

Pick-Up Objects
Pick-ups will include health boosts, consumable combat items and a currency/material used for crafting or buying items. Pick-up items may be dropped my defeated enemies, found placed within the level, found by interacting with objects in the level, and bought.

Moving Objects
Objects that are too large to be placed in the players inventory will be lifted overhead, pushed along the ground, moved by some aspect of the player’s equipment, or simply not be capable of being moved by the player.

Switches and Buttons
There will be switches that can deactivate certain hazards once you’ve passed them successfully. This will allow the player to explore the environment safely after completing particular obstacles.

Economy
The player will collect currency in the form of salvaged parts and hardware from defeated enemies. When the player has collected a sufficient amount they can use the currency towards building upgrades to their character. There will be upgrade stations at strategic locations to apply the changes before entering a new area.

Game Options
	The game options will include a audio menu, a video menu, and controller/keyboard hotkey binding menus. The audio menu will have sliders for the different sounds in the game. The video menu will include different screen resolutions as well as a full screen toggleable option.

Saving
Throughout the game there will be various charging stations, which act as save points, and they also allow the player to recover to full health. Then if a player should be defeated, they can restart at the most recently accessed charging station.



Controls
The player controls their character through use of the arrow keys, the Spacebar and the A, S, X and D keys. 

The left and right arrow keys control the player’s horizontal movement.
The Spacebar and X keys cause the player to jump. The player will be able to unlock upgrades which allow a second jump to be performed in the air by pressing the key again, and which allow the player to glide by holding down the key.
The down key causes the player to crouch while on the ground, which makes their hitbox smaller and slows their movement. Crouching for an extended period of time will shift the camera down.
A will attack with the players equipped primary weapon in the direction that the player is currently facing. Depending on keys held the player can attack in different directions.
If the up key is held the player will attack upwards.
If the player is crouching they will attack lower than normal. 
If the player holds the up key along with the right or left arrow key they will attack upwards at an angle.
If the player holds the down key while in the air they will attack straight down.
If the player holds the down key along with the right or left arrow key while in the air they will attack downwards at an angle.
Holding the up arrow will cause the character to look up, and shift the camera up.
The player will be able to deploy a grappling hook with S that will allow the player to swing from it after it has attached itself to an object.
The D key will be reserved for interactions with objects, which might include picking up or lowering heavy objects, pulling a lever, or accessing a terminal.

There are also plans to implement the use of directinput via commonly supported joypads.







Actions
Jump
Double Jump
Glide?
Dash?
Air Dash?
Grapple for movement
Grapple to pull things to you?
Attack (shoot)
Attack (melee?)
Interact
Pick up objects?
Pause/open menu
Save
Load
Return to main menu
Exit
Upgrade character
Swap weapons























Section III – Story, Setting and Character  

Story
Our main character is abruptly awakened at the start of the game after being in sleep mode for an undetermined duration of time because of damage to the facility that forces him to leave the area. The player will encounter various types of robotic enemies on their quest to find out what exactly is going on in the world.

Back story
Our game world exists in a not too distant future where man is all but extinct. Robotic technology has come to rule the world after the rise of sentient AI. Our story takes place many years into this new world.

Plot Elements
The plot involves the player character waking up or powering on in a mysterious environment. The player comes to discover they are in a research facility that has become lost to time. In the process of exploration they are met with hostile robotic remnants of the facility. 

Game Progression
The player progresses the story by discovering clues about its surroundings and the meaning of the world in which it has been “born”.

Cut Scenes
Cutscenes will consist of simple camera focus or breaks in gameplay. Narration in the form of text subtitles will accompany the discovery of new plot elements. We will be considering opening and closing cut scenes to introduce the game world as well as end on a cliffhanger.


Game World
The game world will show the post-apocalyptic state of your environment. The facility will progress from a moderately broken down environment to its complete destruction and reclamation by nature. 




Section IV – Technical 


Target Hardware
The game will be built for PC but with the versatility of our development platform could be ported to many devices. The game will be designed for keyboard and mouse controls with joypad controller support secondary.

Development Hardware and Software
	Our development will be done primarily on Windows PC using Unity game engine. Other software will include Autodesk Maya, Mudbox and additional 3D modeling utilities for asset creation. 

Development Procedures and Standards
We will be using Unity collaboration tools for version control in addition to GitHub. Detailed procedures are to be determined at a later date. 


Game Engine
The game engine will be exclusively done in Unity.

Scripting Language
Programming will be done in C# 














Section V – Game ArtConcept Art


Style Guides
We will use a technique known as cel shading on our characters and assets to make the 3d assets appear more 2d.





Characters
    Characters will all be robots. The main character will be more humanoid in appearance and movement, and enemies will be robotic animals. Smaller animals such as wolves and birds will be throughout the levels, with a large animal such as a bear or gorilla for a boss.

Environments
	The setting of our game will be in a science facility, that gradually becomes more and more overgrown with vegetation as the player moves closer to the end of the game, with the end/ boss fight being almost entirely a forest. We will implement small bonus rooms throughout the game that will vary in appearance depending on where the character is in the facility, like a chem lab or manufacturing line. There will be artistic backgrounds as well, which are made in layers to show some movement while the player is running around, and possibly some extra NPC’s running around in the background.


Equipment
	The characters weapons and equipment will all be extensions of the character’s arms themselves, with the the characters hands being used as the grappling hooks, and them being able to change into a gun or melee weapon like a sword.

Cut Scenes
.	Minimal cutscenes, with small camera movements here and there like zooming in on the character as he is being activated at the beginning of the game, and the boss smashing through a wall as he enters the boss fight. Possibly some small dialogue boxes within the scenes.


Miscellaneous
Possibly some art for projectiles and explosions, maybe some moving aspects in the background like creatures moving around in the distance while the player is moving along. There will be various items throughout the game that are all themed to the game. We will create some floating batteries around the game that act as health packs, and replenish your health a certain amount. We Will also create some sort of charging stations that replenish the players health completely, and act as a save point that the player can come back to in the event of the game over.
