# Description of Beat Blaster
This is a difficult rhythm based 2D shooting game. Your player only moves and shoots when the beat hits! Battle with different enemy types and master various music beats. Try your skills to survive all 6 levels.

# Beat Blaster Final Improvements

1. **Audio Start Bugfixes:** We noticed some audio synchronization errors during beta play tests, especially during the first measure. We discovered that this was because we were using game time instead of the digital sound processing (DSP) time. Swapping over and tweaking some other sound management systems helped iron out the synchronization issues
2. **Audio Slider Bugfixes:** The start menu's audio slider was not correctly affecting the rest of the game. Assigning clips to audio sources and feeding them into a mixer managed by the slider helps to correct this.
3. **Dash Animation:** The dash animation was greatly improved with a new fading animation and updated sprite art!
4. **New Levels:** We added in two new levels, with new enemies and two new beats/action pairings. One of these is an especially challenging final boss battle.
5. **Background/ambience Updates:** The plain backgrounds are now varied and pulsate in response to the music.
6. **Font Updates:** All text components are replaced with TMP equivalents and several hard to read blocks of text have had their fonts replaced for clarity. Additionally, we added in some new snappy fonts for titling.
7. **Movement Animation Smoothing:** Per our beta feedback, instead of simply teleporting towards the destination, both the player and enemies will smoothly LERP to their destination.
8. **Rework Health UI:** Instead of being dislpayed in a text element as in our beta, the player's health is now displayed in a bar above their character much like the bars reflecting enemy health.
9. **New Health Restoration and Animations:** To compensate for increasing difficulties later on, we added in a health restore after wave, accompanied by a new restorative animation for the player.
10. **More Complex Enemy Spawning Logic:** The enemy AI and spawning has been improved so within a given level, there is a ramping difficulty curve (larger and harder waves of enemies as the level progresses and the player acclimates to the pattern)
11. **Weapon Attack Bugfixes:** Previously, the blaster attack visually penetrated multiple enemies, but failed to correctly damage them. Updates to the hitbox, penetration, and damage of the blaster should improve it's feel. 


