# Kings Trial

## Descripción
Kings Trial es un juego 2D estratégico desarrollado en Unity donde dos grupos de soldados compiten en un laberinto cúbico por orden de su rey. El objetivo es acumular la mayor cantidad de puntos posibles en un tiempo limitado, mientras intentan sobrebir en un laberinto de fantasia.

## Características Principales

### Mecánicas de Juego
- **Sistema de Turnos**: Los jugadores alternan turnos para realizar una acción
- **Acciones Posibles**:
  - Movimiento de personaje
  - Rotación de caras del cubo
- **Laberinto Dinámico**:
  - Cubo con 6 caras jugables
  - Sistema de rotación similar al cubo Rubik
  - Todas las casillas son accesibles inicialmente

### Personajes
El juego cuenta con 5 piezas únicas, cada una con habilidades especiales:
1. Healer - Cura a todos sus aliados en su rango
2. Destroyer - Destruye paredes abriendo todos los caminos de su casilla
3. Gladiator - Encierra la casilla en la que se encuentra, curandose un poco,y aplicando damage a cualquier enemigo que este en ella
4. Lancer - Hace damage en forma de cruz a las casilla en su rango
5. Thief - Roba los puntos libres en las casillas en su rango

### Elementos del Juego
- **Trampas**: 3 tipos diferentes que pueden aparecer en cualquier cara
  1. Espinas
  2. Teletransporte
  3. Lentitud
- **Sistema de Puntos**:
  - Aparecen aleatoriamente en cualquier cara
  - Solo se pueden recolectar desde la cara superior

## Requisitos Técnicos
- Unity 6

## Instalación
1. Clonar el repositorio
2. Abrir el proyecto en Unity


## Reglas del Juego
1. El juego se divide en dos equipos
2. Cada equipo se mueve por turnos
3. En cada turno, un jugador puede:
   - Mover su pieza
   - Rotar una cara del cubo
4. Los puntos solo pueden recolectarse desde la cara superior
5. El equipo con más puntos al final del tiempo gana

## Para Jugar:
1. Comienza directamente la partida, a la izquierda tendra la seleccion de piezas que puede colocar en el laberinto, los jugadores se turnaran para elegir una por una hasta llegar a 2 por equipo. Las piezas se atacaran solamente al final del turno si estan en la misma casilla o con el uso de habilidades. En cada Turno el jugador puede decidir si moverse o mover el cubo. A la vez que seleccione una pieza tiene que usarla, ademas durante el movimiento si hay 2 piezas en una casilla, una tercera no puede pasar por ahi.
