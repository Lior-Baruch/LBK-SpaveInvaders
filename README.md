# LBK-SpaceInvaders

LBK-SpaceInvaders is an open-source implementation of the classic arcade game Space Invaders, developed in C#. The game involves a player-controlled spaceship that must protect the earth from an invading force of aliens. The player's objective is to destroy all of the aliens before they reach the bottom of the screen and evade their missiles.

## Project Structure

The repository contains two main directories: `infastructure` and `invaderss`.

### `infastructure`

The `infastructure` directory houses the core infrastructure of the game, including the game engine and various utility classes. The main game engine class (`BaseGame.cs`) extends the `Game` class from the `Microsoft.Xna.Framework` library and is responsible for initializing and running the game. It also manages input, sound, and collision detection.

In addition, the `ExtensionMethods.cs` file provides utility methods for the game, and `Program.cs` serves as the entry point for the infrastructure code.

### `invaderss`

The `invaderss` directory contains the game-specific code. The main class (`SpaceInvaders.cs`) extends the `BaseGame` class from the `infastructure` directory and implements the game logic. It initializes the game and the screens manager, handles user input (such as toggling sound), and manages game updates and rendering.

The game also loads various sound effects in the `LoadContent` method, which are used throughout the gameplay.

## How to Run

To run the game, you should have a C# IDE (like Microsoft Visual Studio) installed. You can follow these steps:

1. Clone this repository.
2. Open the solution file (`C20 Ex03 Ido 205892383 Lior 327156998.sln`) in Microsoft Visual Studio.
3. Press the "Start" button or press F5 to build and run the project.

## Game Features

The game includes the following features:

- Player-controlled spaceship: The player can move the spaceship left and right and fire missiles to destroy aliens.
- Alien invasion: Multiple rows of aliens move left and right on the screen, dropping down one row each time they hit the edge of the screen. They also fire missiles at the player.
- Sound effects: Various sound effects enhance the gaming experience, such as firing missiles, alien destruction, and background music.

## Contributing

Contributions to this project are welcome. If you find a bug or have an idea for a new feature, feel free to open an issue or submit a pull request.

## License

This project is licensed under the terms of the MIT license.
