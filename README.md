# MineTetris

## Introduction

### Choix de WPF pour le projet

Le projet **MineTetris** a été réalisé en utilisant **Windows Presentation Foundation (WPF)**, une technologie de développement d'applications pour Windows qui permet de créer des interfaces graphiques riches. Cela permet une expérience utilisateur fluide et interactive pour des jeux comme Tetris.
  
Le XAML, utilisé dans WPF pour définir l'interface graphique, est similaire au HTML dans la mesure où il permet de structurer et organiser les éléments de l'interface utilisateur de manière déclarative. Par exemple, tout comme en HTML, on peut définir des éléments comme des `Button`, `TextBlock`, ou `Image`, qui sont utilisés pour interagir avec l'utilisateur.

Voici un exemple de comparaison simple :

- **HTML** :
    ```html
    <div class="game-container">
        <button id="startButton">Start</button>
        <div id="score">Score: 0</div>
    </div>
    ```

- **XAML** :
    ```xml
    <Grid>
        <Button Content="Start" Name="startButton" />
        <TextBlock Name="scoreText" Text="Score: 0" />
    </Grid>
    ```

Cette approche permet de développer plus rapidement des interfaces interactives en utilisant une syntaxe claire et expressive, similaire à HTML mais avec des fonctionnalités supplémentaires adaptées aux applications de bureau.

## Explication de l'utilisation du jeu

### Objectif du jeu
Le jeu MineTetris est une version revisitée du classique Tetris, mais inspirée de l'univers de Minecraft. Les blocs géométriques qui tombent, prennent l'apparence de matériaux emblématiques de Minecraft, tels que le diamant, l'or, la redstone, et bien d'autres.

L'objectif est de manipuler ces blocs pour former des lignes horizontales complètes. Lorsque vous complétez une ligne, elle disparaît, et vous marquez des points. En combinant le gameplay addictif de Tetris avec l'esthétique et les éléments visuels de Minecraft, MineTetris apporte une touche unique et familière pour les fans des deux univers.

Les textures et couleurs des blocs plongent les joueurs dans l'atmosphère pixelisée du célèbre jeu de survie, tout en conservant les mécanismes classiques de Tetris qui rendent le jeu intuitif et engageant.

### Contrôles du jeu
Le jeu peut être contrôlé en utilisant les touches du clavier, qui peuvent être personnalisées dans les options. Les contrôles par défaut sont les suivants :

- **Flèche gauche** : Déplacer le bloc vers la gauche.
- **Flèche droite** : Déplacer le bloc vers la droite.
- **Flèche bas** : Faire descendre le bloc plus rapidement.
- **Flèche haut** : Faire pivoter le bloc dans le sens horaire.
- **Z** : Faire pivoter le bloc dans le sens antihoraire.
- **Barre d'espace** : Faire tomber instantanément le bloc.

### Fin du jeu
Le jeu se termine lorsque les blocs s'empilent trop haut et dépassent le haut de l'écran. Le score final est affiché et vous pouvez choisir de recommencer une nouvelle partie ou quitter le jeu.

### Score et Record
Le score est mis à jour en temps réel à mesure que des lignes sont supprimées. Si vous marquez un score plus élevé que votre record actuel, celui-ci est enregistré pour être utilisé lors de la prochaine session de jeu.

### Instructions pour Lancer le Jeu
Pour jouer à MineTetris, vous avez deux options : soit via Visual Studio, soit directement via le fichier .exe. Voici les étapes détaillées :

#### Lancer le Jeu depuis Visual Studio
1. Ouvrir le Projet :
- Lancez Visual Studio et ouvrez votre projet MineTetris.
2. Configurer le Mode de Lancement :
- Assurez-vous que la configuration de la build est définie sur Debug (pour le développement) ou Release (pour une version finale optimisée).
  - Vous pouvez vérifier cela dans la barre d'outils de Visual Studio.
3. Exécuter le Projet :
- Cliquez sur le bouton Start (ou Démarrer) dans la barre d'outils.
- Le jeu s'ouvre dans une fenêtre, prêt à être joué.

#### Lancer le Jeu avec le Fichier .exe
1. Localiser le Fichier .exe :
- Une fois votre projet compilé (voir la section précédente), allez dans le dossier suivant :
    `\bin\Release\`
- Vous trouverez un fichier nommé Tetris.exe.

2. Exécuter le Fichier :
- Double-cliquez sur le fichier Tetris.exe pour lancer le jeu.
- Aucune installation supplémentaire n'est nécessaire, sauf si le .NET Runtime 5.0 n’est pas installé sur votre machine.

3. Si le jeu ne ce lance pas :
- Allez dans "Générer > Publier la sélection" et appuyer sur Publier.
- Une fois la publication terminée, utilisez le fichier .exe qui se trouve dans le dossier suivant :
    `\bin\Release\net5.0-windows\publish\win-x86`
#### Avantages du Lancement via .exe
- Simplicité : Pas besoin de Visual Studio pour lancer le jeu.
- Portabilité : Vous pouvez copier le fichier .exe sur une clé USB ou l'envoyer à un ami (avec les fichiers nécessaires).
- Accessibilité : Idéal pour une utilisation sur des machines sans environnement de développement.