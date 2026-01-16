# Kings Trial

## Descripción
Kings Trial es un juego 2D estratégico desarrollado en Unity donde dos grupos de soldados compiten en un laberinto cúbico por orden de su rey. El objetivo es acumular la mayor cantidad de puntos posibles en un tiempo limitado, mientras intentan sobrevivir en un laberinto de fantasía.

## Características Principales

### Mecánicas de Juego
- **Sistema de Turnos**: Los jugadores alternan turnos para realizar una acción
- **Acciones Posibles**:
  - Movimiento de personaje
  - Rotación de caras del cubo
  - Uso de habilidades especiales
- **Laberinto Dinámico**:
  - Cubo con 6 caras jugables
  - Sistema de rotación similar al cubo Rubik
  - Todas las casillas son accesibles inicialmente

### Personajes
El juego cuenta con 8 piezas únicas, cada una con habilidades especiales:

1. **Healer** - Cura a todos sus aliados en su rango (HP: 30, Velocidad: 2, Daño: 5)
2. **Destroyer** - Destruye paredes abriendo todos los caminos de su casilla (HP: 50, Velocidad: 2, Daño: 10)
3. **Gladiator** - Encierra la casilla en la que se encuentra, curándose un poco y aplicando daño a cualquier enemigo (HP: 45, Velocidad: 3, Daño: 8)
4. **Lancer** - Hace daño en forma de cruz a las casillas en su rango (HP: 40, Velocidad: 3, Daño: 10)
5. **Thief** - Roba los puntos libres en las casillas en su rango (HP: 15, Velocidad: 4, Daño: 6)
6. **Explorer** - Revela trampas y obtiene un aumento temporal de velocidad (HP: 20, Velocidad: 5, Daño: 4)
7. **Archer** - Disparo de largo alcance con alto daño a un solo objetivo (HP: 25, Velocidad: 2, Daño: 12)
8. **Tank** - Alta resistencia, aturde enemigos adyacentes y se cura (HP: 80, Velocidad: 1, Daño: 6)

### Trampas
El juego cuenta con 4 tipos de trampas que pueden aparecer en cualquier cara:

1. **Espinas (Spikes)** - Causan daño aleatorio (0-10) al pisar
2. **Teletransporte (Teleport)** - Transporta la pieza a una posición aleatoria
3. **Veneno (AffectStats)** - Reduce la velocidad y causa daño menor
4. **Congelación (Freeze)** - Reduce los movimientos restantes del turno

### Sistema de Puntos
- Aparecen aleatoriamente en cualquier cara
- Solo se pueden recolectar desde la cara superior
- Valores de puntos: 1 (común), 3 (raro), 5 (épico)

## Arquitectura del Código

El proyecto utiliza varios patrones de diseño para mantener el código limpio y extensible:

### Patrones Implementados

- **Strategy Pattern**: Para las habilidades de las piezas (`IAbility`) y efectos de trampas (`ITrapEffect`)
- **Factory Pattern**: Para la creación de piezas (`PiecesInitialData`), trampas (`TrapsInitialData`), habilidades (`AbilityFactory`) y efectos de trampas (`TrapEffectFactory`)
- **MVC Pattern**: Separación clara entre Modelos, Controladores y Vistas

### Estructura del Proyecto

```
Assets/Scripts/
├── Core/
│   ├── Abilities/        # Habilidades de las piezas (Strategy Pattern)
│   ├── Controllers/      # Controladores de juego
│   ├── Interface/        # Interfaces para abstracción
│   ├── Models/           # Modelos de datos
│   └── Traps/            # Efectos de trampas (Strategy Pattern)
├── Managers/             # Gestores del juego
└── Visual/               # Componentes visuales
```

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
   - Usar una habilidad especial
4. Los puntos solo pueden recolectarse desde la cara superior
5. El equipo con más puntos al final del tiempo gana

## Para Jugar
1. Comienza directamente la partida
2. A la izquierda tendrá la selección de piezas que puede colocar en el laberinto
3. Los jugadores se turnarán para elegir una por una hasta llegar a 2 por equipo
4. Las piezas se atacarán solamente al final del turno si están en la misma casilla o con el uso de habilidades
5. En cada turno el jugador puede decidir si moverse, usar una habilidad o mover el cubo
6. Durante el movimiento, si hay 2 piezas en una casilla, una tercera no puede pasar por ahí

## Contribución
Las contribuciones son bienvenidas. Por favor, asegúrate de seguir los patrones de diseño establecidos al agregar nuevas funcionalidades.
