# PokemonUnitySaveFeature
A start for the creation of a flexible save mechanic inside PokemonUnity.
<p align="center">
  <img alt="Pokemon Unity Logo" src="https://styles.redditmedia.com/t5_39moy/styles/bannerPositionedImage_6is405sk53j01.png" />
  <h1 align="center">Pokemon Unity Expansion Project</h3>
  <p align="center">
    <a href="https://opensource.org/licenses/BSD-3-Clause"><img alt="License" src="https://img.shields.io/badge/license-New%20BSD-blue.svg"></a>
    <a href="https://discord.gg/SggHcXP"><img alt="Discord Server" src="https://img.shields.io/badge/join%20us%20on-discord-7289DA.svg"></a>
    <a href="https://trello.com/b/BXmwFOBt/pokemon-unity-expansion"><img alt="Trello" src="https://img.shields.io/badge/view%20progress%20on-trello-026AA7.svg"></a>
    <a href="https://pokemonunity.gitbooks.io/pokemon-unity/content/"><img alt="GitBook" src="https://img.shields.io/badge/view%20docs%20on-gitbook-blue.svg"></a>
  </p>
</p>

<p align="center">
  This repositorie is dedicated to implementing a flexible save mechanic in Pokemon Unity that uses BinarryFormatting to encrypt the data.
  The mechanics are fairly simple: an EventListener that listens to the PlayerMovement script to see if the Player is doing something that needs to be saved.
  GlobalSaveManager which is a script that saves all the data into a .PKU file and can load the data back in.
  CustomSaveEvent which is a System.Serializable class that saves an event (like picking up an Item).
</p>

<br></br>
<p align="center">
  The Develop branch is for the public repository of PokemonUnity and the Beta branch is for herbertmillhome's PokemonUnity Fork
</p>

## Credits

* PKUE Project Lead: [Kisu-Amare](https://www.furaffinity.net/user/teampopplio/)
* PKUE Assistance: Brain_Face#8657


* PKUnity Base author: [IIcolour Spectrum](https://www.reddit.com/user/IIcolour_Spectrum)
* PKUnity Maintainer: [superusercode](https://www.reddit.com/user/Lucas_One/)
* PKUnity Logo artist: [Kaihatsu](https://twitter.com/KaihatsuYT)

## Links
* Pokemon Unity: https://github.com/PokemonUnity/PokemonUnity
* Trello: https://trello.com/b/BXmwFOBt/pokemon-unity-expansion
* Discord server: https://discord.gg/SggHcXP
* Documentation: https://img.shields.io/badge/view%20docs%20on-gitbook-blue.svg
