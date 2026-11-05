# CUBERUN – Documento de diseño del videojuego 3D

## 1. Historia del juego

Dos amigos están grabando una película por diversión. Sin embargo, durante la grabación, uno de ellos cae repentinamente en los Backrooms mediante un fenómeno conocido como *Noclip*.

El jugador queda atrapado en este extraño lugar y deberá encontrar la manera de regresar a la realidad.

Para conseguirlo, tendrá que explorar los Backrooms y reunir las tres piezas del **Cubo Clasificador**:

- Triángulo.
- Cubo.
- Esfera.

Cada pieza estará ubicada en una zona diferente y será custodiada por un Bacteria perseguidor. Durante la exploración también podrán aparecer criaturas cuyo único propósito será asustar al jugador.

Una vez reunidas las tres piezas, el jugador deberá regresar al Cubo Clasificador para activar el mecanismo de salida y volver al mundo real.

## 2. Objetivo principal

Encontrar las tres piezas del Cubo Clasificador, superar al Bacteria que las custodia y regresar al punto de salida para activar el mecanismo que permitirá volver al mundo real.

## 3. Mecánicas principales

### 3.1. Sistema de orientación

- Al ingresar a los Backrooms, aparecerán flechas que indicarán dónde se encuentra el Cubo Clasificador.
- Frente al cubo habrá un mensaje en la pared que explicará que se necesitan tres piezas para regresar a la realidad.
- Cada zona tendrá símbolos relacionados con la figura geométrica que se encuentra en su interior.
- Estos símbolos servirán como pistas para orientar al jugador.

### 3.2. Sistema de recolección de piezas

Cada pieza estará ubicada en un escenario diferente, con una ambientación propia. Las tres zonas estarán custodiadas por el mismo tipo de Bacteria perseguidor.

#### A. La esfera – La vitrina

- La esfera estará dentro de una vitrina con forma de cubo.
- El jugador deberá acercarse y presionar la tecla **E** para interactuar.
- Al presionar E, la vitrina se abrirá y la esfera caerá para que el jugador pueda recogerla.
- La zona estará custodiada por un Bacteria perseguidor.

#### B. El cubo – La habitación geométrica

- Estará en una habitación llena de cubos de diferentes tamaños.
- Entre todos ellos habrá un cubo principal.
- El cubo principal tendrá una textura visual relacionada con la geometría 4D (teseracto).
- El jugador deberá identificarlo y recogerlo.
- La habitación estará custodiada por un Bacteria perseguidor.

#### C. El triángulo – La habitación Illuminati

- Habrá una pirámide inspirada en la simbología Illuminati.
- Las paredes tendrán símbolos, dibujos y elementos visuales relacionados con los Illuminati.
- El triángulo estará ubicado en esta zona.
- El jugador deberá encontrarlo y recogerlo.
- La zona estará custodiada por un Bacteria perseguidor.

## 4. Sistema de enemigos: Bacteria

En el juego existirán únicamente **dos tipos de Bacteria**.

### 4.1. Bacteria perseguidor

- Será el único enemigo que podrá matar al jugador.
- Perseguirá al jugador cuando lo detecte.
- Custodiará las zonas donde se encuentran las tres piezas del Cubo Clasificador.
- Será el mismo tipo de criatura en las tres zonas; no serán tres monstruos diferentes.
- Si alcanza al jugador, aparecerá la pantalla **GAME OVER**.

### 4.2. Bacteria de susto

- Aparecerá inesperadamente durante la exploración.
- Su objetivo será asustar al jugador.
- No perseguirá ni podrá matar al jugador.
- Desaparecerá después de provocar el susto.
- Al aparecer, activará efectos de terror:
  - Respiración agitada del personaje.
  - Latidos acelerados del corazón.
  - Movimiento brusco de cámara.
  - Oscurecimiento momentáneo de la pantalla.

## 5. Ambientación y efectos de terror

El ambiente de los Backrooms deberá transmitir una sensación constante de inseguridad, tensión y misterio.

### 5.1. Sistema de iluminación

- Las luces parpadearán de manera irregular.
- Algunas luces se apagarán repentinamente.
- Habrá momentos de oscuridad completa o parcial.
- Las luces podrán volver a encenderse después de unos segundos.
- Se utilizará la iluminación para generar tensión antes de la aparición de los enemigos.

### 5.2. Sonidos ambientales

- Música ambiental inquietante.
- Sonidos repentinos que provoquen miedo.
- Ruidos extraños provenientes de habitaciones cercanas.
- Cortes inesperados de música.
- Sonidos de pasos o movimientos que generen la sensación de que alguien está cerca.
- Sonidos específicos para la aparición de los Bacteria.

### 5.3. Efectos adicionales

- Cambios repentinos en la iluminación.
- Sonidos de interferencia.
- Efectos de cámara durante los sustos.
- Momentos de silencio para aumentar la tensión.

## 6. Sistema de victoria y derrota

### 6.1. Victoria: regreso al mundo real

1. El jugador reúne las tres piezas.
2. Regresa al Cubo Clasificador.
3. Coloca las piezas en sus respectivas posiciones.
4. Se activa el mecanismo de salida.
5. Se produce una transición mediante Noclip.
6. El jugador aparece nuevamente en el mundo real (*Overworld*).

### 6.2. Derrota: el jugador es atrapado

1. El Bacteria perseguidor alcanza al jugador.
2. Aparece la pantalla **GAME OVER**.
3. Se muestran dos opciones:
   - **Reintentar:** comenzar nuevamente la partida.
   - **Salir:** abandonar el juego.

## 7. Flujo general de la partida

1. **Inicio:** dos amigos están grabando una película.
2. **Noclip:** uno de ellos cae en los Backrooms.
3. **Exploración:** el jugador busca las tres piezas siguiendo las pistas.
4. **Recolección:** consigue la esfera, el cubo y el triángulo.
5. **Regreso:** vuelve al Cubo Clasificador.
6. **Activación:** coloca las piezas y activa el mecanismo de salida.
7. **Final:** escapa al mundo real o es atrapado por el Bacteria perseguidor.

## 8. Resumen de escenarios y enemigos

| Zona | Pieza | Características | Enemigo |
|---|---|---|---|
| Vitrina | Esfera | Vitrina cúbica; se interactúa con la tecla E para abrirla y hacer caer la esfera. | Bacteria perseguidor |
| Habitación geométrica | Cubo | Cubos de diferentes tamaños y un cubo principal con textura 4D. | Bacteria perseguidor |
| Habitación Illuminati | Triángulo | Pirámide y símbolos relacionados con los Illuminati en las paredes. | Bacteria perseguidor |
| Exploración general | Ninguna | Apariciones inesperadas para provocar sustos. | Bacteria de susto |

**Aclaración:** Solo hay dos tipos de enemigos en todo el juego: el Bacteria perseguidor y el Bacteria de susto. El perseguidor es el mismo tipo de criatura que custodia las tres piezas; no existen tres guardianes diferentes.

## 9. Elementos que se deben desarrollar

1. Movimiento y control del jugador.
2. Sistema de interacción mediante la tecla E.
3. Sistema de recolección de objetos.
4. Diseño de las tres habitaciones especiales.
5. Inteligencia artificial del Bacteria perseguidor.
6. Aparición del Bacteria de susto.
7. Sistema de respiración y latidos del corazón.
8. Sistema de iluminación parpadeante.
9. Sistema de sonidos ambientales y efectos de terror.
10. Sistema de victoria, derrota y reinicio.

## 10. Referencia de desarrollo

Tutorial de YouTube compartido como referencia:

[Ver tutorial de YouTube](https://www.youtube.com/watch?v=H4dGpz6cnHo&list=PLVAh-MgDVqvDUEq6qDXqORBioE4Yhol_z)
