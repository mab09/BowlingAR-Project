# BowlingAR: An AR Bowling Alley Experience
<img src="https://github.com/user-attachments/assets/a1b05c0d-577d-4dc9-8bec-8139674c2cc9" width=20% height=20%>

[BowlingAR Demo](https://youtu.be/D4AHHF2YXxo)

BowlingAR is an AR implementation of a fun and simple Bowling Alley experience, developed as a course project for the Meta AR Developer Certification. This engaging application allows players to spawn a bowling lane in their environment using their phone camera and throw bowling balls to knock out pins using screen swipes.

## Key Features
- **AR Integration:** Utilizes Vuforia Engine along with a phone camera for proper plane detection and an immersive augmented reality experience.
- **Gameplay Mechanics:** Players spawn a bowling lane by pointing their phone camera to a flat surface and tapping the screen. They can then play five rounds of knocking down pins by aiming with the phone camera and swiping the screen to throw the ball (The longer the swipe the, harder the throw). One point for one pin knocked, and double points for a strike.
- **Ball and Pins:** The ball and the pins are rigidbody enabled physics objects, giving realistic knocking, rolling and toppling effects.
- **User Interface:** Title screen, gameover screen, instructions, round indicator, strike indicator and HUD that includes current score, remaining balls and a reset button.
- **Sound and Particle Effects:** Enhance the gameplay experience with various sound effects for ball collisions, resetting pin deck and strike achieved along with visual effects for ball hits.
- **Target Platform:** AR-enabled Android devices.
   
## Techniques and Coding
- **Scriptable Objects:** Utilized to maintain game state.
- **Coroutines:** Used for handling asynchronous processes without blocking the main game loop.
- **UnityEvents:** Facilitated interaction between different scripts, such as updating the game score UI, remaining balls UI and coordinating different elements between rounds.

## Extra Features 
- **PinDeck Mechanics:** According to the design specifications in the course, when a pin is toppled it would remain there, but here, when a pin is toppled it is cleaned up, only loading the un-toppled pins in the next round(unless all pins are toppled in which case the entire deck is reset).
- **Restart:** There is a small restart button at top center of the HUD, this reloads the scene in case the bowling lane fails to setup properly, which may happen on non-flat, uneven surfaces.

## Assets Used 
- **3D Models and Particle Effects:** Provided in the course.
- **Vuforia Engine SDK 10.27.3:** For improved plane detection and tracking.
- **Particle Effects:** Sourced from [SoundBits | Free Sound FX Collection](https://assetstore.unity.com/packages/audio/sound-fx/soundbits-free-sound-fx-collection-31837) and [Free SFX Package](https://assetstore.unity.com/packages/audio/sound-fx/free-sfx-package-5178) on the Unity Asset Store.

BowlingAR is an immersive AR experience, integrating advanced coding techniques and enhancing user interaction with thoughtful UI and gameplay elements.

Made in Unity 6000.0.23f1<br />
Tested on Moto Razr+, Android version: 14.
