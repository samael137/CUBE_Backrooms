# Resumen de Avance: Semana 2 – Proyecto CUBERUN 3D

Este documento detalla de manera sencilla, clara y estructurada todo lo desarrollado e implementado durante la **Semana 2** del proyecto *CUBERUN 3D*.

---

## 📑 Índice de Contenidos

1. [Controlador del Jugador en Primera Persona (FPS)](#1-controlador-del-jugador-en-primera-persona-fps)
2. [Corrección Física de Colisión del Jugador](#2-corrección-física-de-colisión-del-jugador)
3. [Efecto de Pasos y Balanceo de Cámara (Headbob)](#3-efecto-de-pasos-y-balanceo-de-cámara-headbob)
4. [Audio de Respiración Reactiva al Cansancio](#4-audio-de-respiración-reactiva-al-cansancio)
5. [Audio Espacial 3D Ambiental en los Pasillos](#5-audio-espacial-3d-ambiental-en-los-pasillos)
6. [Inteligencia Artificial de la Bacteria (NavMesh y Persecución)](#6-inteligencia-artificial-de-la-bacteria-navmesh-y-persecución)
7. [Configuración del Esqueleto y Animador de la Bacteria](#7-configuración-del-esqueleto-y-animador-de-la-bacteria)
8. [Cinemática de Muerte y Jumpscare 3D Frente al Jugador](#8-cinemática-de-muerte-y-jumpscare-3d-frente-al-jugador)
9. [Resumen Visual y Ficha Rápida](#9-resumen-visual-y-ficha-rápida)

---

## 1. Controlador del Jugador en Primera Persona (FPS)

### ¿Qué se usó?
* Componente `CharacterController` de Unity.
* Script propio en C#: `SimpleTerrorPlayer.cs`.
* Ratón y teclado (`WASD` y `Left Shift`).

### ¿Por qué se usó?
Para que el usuario pueda explorar los Backrooms sintiendo que controla a una persona real, con peso, inercia y límites en la mirada, en lugar de un movimiento rígido o como si flotara en el aire.

### ¿Cómo se hizo?
* Se configuró el movimiento horizontal con las teclas `W`, `A`, `S` y `D`.
* Se aplicó una aceleración suave (`Vector3.Lerp`) para que el personaje no frene de golpe, simulando la inercia del cuerpo al caminar o correr.
* Al presionar `Left Shift`, la velocidad aumenta (modo carrera).
* Con el ratón se controla la rotación horizontal del cuerpo y la inclinación vertical de la cámara, limitando la vista vertical entre $-80^\circ$ y $+80^\circ$ para evitar giros imposibles del cuello.
* Se programó el bloqueo automático del cursor al hacer clic en pantalla y su liberación al presionar `Esc`.

> **En pocas palabras:** Un sistema de movimiento fluido que hace que caminar y mirar por el laberinto se sienta natural, humano y cómodo.

---

## 2. Corrección Física de Colisión del Jugador

### ¿Qué se usó?
* Ajuste de propiedades en el componente `CharacterController` (`Center`, `Height` y `Skin Width`).

### ¿Por qué se usó?
Al pulsar *Play*, el personaje aparecía congelado en el suelo y no podía moverse debido a que la mitad de su cuerpo nacía enterrada dentro del piso del laberinto.

### ¿Cómo se hizo?
* Se corrigió la propiedad `Center Y` del colisionador de `0` a `1.0` (la altura total es de $2$ metros, por lo que el centro geométrico debe estar en $1$).
* Se elevó la posición inicial del jugador en el eje vertical a $Y = 1.5$ para que caiga limpiamente de pie sobre el suelo por gravedad.

> **En pocas palabras:** Se arregló la cápsula invisible del jugador para que no quede atascado dentro del piso y pueda caminar libremente.

---

## 3. Efecto de Pasos y Balanceo de Cámara (Headbob)

### ¿Qué se usó?
* Función matemática senoidal (`Mathf.Sin`).
* Componente `AudioSource` y clips de audio de pisadas sobre alfombra húmeda.

### ¿Por qué se usó?
Una cámara estática parece un dron flotando y quita el suspenso. El balanceo y las pisadas son vitales para generar la tensión de estar caminando por los Backrooms.

### ¿Cómo se hizo?
* Al caminar o correr, la cámara sube y baja levemente en un ciclo continuo con una curva senoidal.
* Cada vez que la curva llega al punto más bajo (cuando el pie virtual pisa el suelo), se reproduce el sonido del paso.
* Se añadió una pequeña variación aleatoria de tono (*pitch* entre $0.85$ y $1.15$) para que cada paso suene único y no parezca una grabación robótica repetitiva.

> **En pocas palabras:** La cámara se balancea al ritmo de tus pasos y produce sonidos de pisadas variadas y realistas.

---

## 4. Audio de Respiración Reactiva al Cansancio

### ¿Qué se usó?
* Componente `AudioSource` en bucle (*Loop*).
* Clip de sonido de respiración agitada.
* Interpolación lineal matemática (`Mathf.MoveTowards`).

### ¿Por qué se usó?
Añadir una capa sonora de esfuerzo físico para aumentar el agobio y transmitir al jugador que su personaje se asusta y se cansa al huir.

### ¿Cómo se hizo?
* Se configuró el audio de respiración para que se reproduzca de fondo constantemente.
* Al caminar en reposo, el volumen se mantiene bajo y calmado (aproximadamente en $0.2$).
* Cuando el jugador mantiene presionado `Left Shift` para correr, el volumen sube suavemente hasta $0.8$, haciéndose más notorio y tenso.
* Al soltar la carrera, vuelve poco a poco a su volumen base.

> **En pocas palabras:** El personaje respira agitado cuando corre desesperado y se calma gradualmente cuando camina o frena.

---

## 5. Audio Espacial 3D Ambiental en los Pasillos

### ¿Qué se usó?
* Script `SonidoAleatorio3D.cs`.
* Objetos vacíos con `AudioSource` en pasillos lejanos.
* Configuración de audio 3D (`Spatial Blend: 1.0` y atenuación logarítmica).

### ¿Por qué se usó?
Para romper el silencio absoluto con ruidos siniestros, crujidos o ecos lejanos que obliguen al jugador a mirar hacia atrás constantemente.

### ¿Cómo se hizo?
* Se colocaron emisores en esquinas distantes del mapa.
* Se programó un temporizador que espera un tiempo aleatorio (entre $10$ y $30$ segundos) para disparar un sonido.
* Gracias a la atenuación 3D, el sonido se escucha con volumen y dirección exacta según la distancia y la oreja con la que mire el jugador.

> **En pocas palabras:** Ruidos y crujidos fantasmales que ocurren de vez en cuando a lo lejos para mantener la sensación de peligro constante.

---

## 6. Inteligencia Artificial de la Bacteria (NavMesh y Persecución)

### ¿Qué se usó?
* Paquete `AI Navigation` de Unity.
* Componentes `NavMeshSurface`, `NavMeshModifier` y `NavMeshAgent`.
* Script propio en C#: `BacteriaPerseguidor.cs`.

### ¿Por qué se usó?
Para que el monstruo principal pueda buscar y perseguir al jugador esquivando paredes y esquinas sin perderse ni atravesar muros.

### ¿Cómo se hizo?
1. **Horneo del Suelo:** Se usó `NavMeshSurface` para escanear todo el piso transitable del laberinto.
2. **Exclusión del Techo:** Se añadió un `NavMeshModifier` al techo con la opción `Ignore From Build` para evitar que la IA creyera erróneamente que podía caminar sobre el techo.
3. **Agente en el Monstruo:** Se le colocó un `NavMeshAgent` a la Bacteria con velocidad ajustada para la persecución.
4. **Comportamiento en Script:** Cuando el jugador entra dentro del radio de visión o proximidad, la bacteria calcula la ruta más corta hacia el jugador y avanza hacia él.

> **En pocas palabras:** El mapa tiene un suelo inteligente que le enseña al monstruo cómo perseguirte por los pasillos sin chocarse con las paredes.

---

## 7. Configuración del Esqueleto y Animador de la Bacteria

### ¿Qué se usó?
* Modelo 3D `decayed.fbx` en `Assets/perseguidor/source`.
* Ajuste de importación en tipo **Generic** (esqueleto Bip001).
* Controlador de animaciones (`bacteria_controler.controller`).

### ¿Por qué se usó?
El modelo original tenía sus huesos creados en 3ds Max con ejes específicos. Al intentar usar el modo humanoide de Unity, los ejes se invirtieron provocando que la bacteria caminara de espaldas (paso estilo Michael Jackson).

### ¿Cómo se hizo?
* Se configuró el esqueleto como **Generic** para respetar fielmente las rotaciones y huesos originales de la cabeza (`Bip001_Head`) y cuerpo.
* En el Animator Controller se crearon tres estados principales: `idle`, `walk` y `run`.
* Se conectó un parámetro numérico `Velocidad` que lee en tiempo real lo rápido que se desplaza la criatura:
  * Si está quieta ($Velocidad = 0$) $\rightarrow$ `idle`.
  * Si avanza lento $\rightarrow$ `walk`.
  * Si corre hacia ti $\rightarrow$ `run`.
* Se activó la opción `Loop Time` en las animaciones para que corra de forma fluida y sin detenerse a mitad de camino.

> **En pocas palabras:** Se configuró el modelo en modo Genérico para que sus animaciones se muevan naturalmente hacia adelante y cambien según si está quieto, caminando o corriendo.

---

## 8. Cinemática de Muerte y Jumpscare 3D Frente al Jugador

### ¿Qué se usó?
* Script `BacteriaPerseguidor.cs`.
* Orientación de cámara mediante `Quaternion.Slerp` apuntando a `Bip001_Head`.
* Efecto de sacudida de pantalla (*Camera Shake*).
* Audio de chillido mortal (`sonido_ataque.mp3`).

### ¿Por qué se usó?
Para que el momento en el que el monstruo te atrapa sea impactante, aterrador y cinematográfico en lugar de una colisión plana o aburrida.

### ¿Cómo se hizo?
* **Detección de contacto:** Cuando la bacteria llega a menos de $2.0$ metros del jugador:
  1. La bacteria frena su navegación en seco y pasa de inmediato a su postura `idle` cara a cara con el jugador.
  2. El control del jugador se apaga para que no pueda huir ni girar la cabeza.
  3. La cámara del jugador es forzada suave pero rápidamente a mirar directamente hacia la cabeza del monstruo (`Bip001_Head`).
  4. La cámara tiembla intensamente en primer plano simular el forcejeo y pánico.
  5. Se reproduce a todo volumen el grito de ataque y muerte.

> **En pocas palabras:** Al atraparte, la bacteria se frena frente a tus ojos, te inmoviliza, la cámara te obliga a mirarle la cara mientras tiembla con un fuerte susto sonoro.

---

## 9. Resumen Visual y Ficha Rápida

| Elemento | ¿Qué se usó? | ¿Para qué sirve? | Resultado obtenido |
|---|---|---|---|
| **Jugador FPS** | `SimpleTerrorPlayer.cs` | Controlar al protagonista con teclado y ratón. | Movimiento inercial realista y vista en primera persona. |
| **Colisión Piso** | `CharacterController.center.y = 1` | Evitar que el personaje nazca incrustado. | Caída suave al piso y movimiento desbloqueado. |
| **Pasos y Headbob** | `Mathf.Sin` + Audio aleatorio | Dar peso a los pasos y balanceo humano. | Sonido de pisadas que no se siente repetitivo. |
| **Respiración** | `AudioSource` con volumen dinámico | Expresar agotamiento físico al correr. | Respiración que sube de volumen con Shift. |
| **Sonido 3D** | `SonidoAleatorio3D.cs` | Generar tensión en el mapa. | Ruidos lejanos en los pasillos con orientación espacial. |
| **IA Navegación** | `NavMesh` + `NavMeshAgent` | Que el monstruo persiga al jugador. | Búsqueda de caminos evitando paredes y techos. |
| **Rig Bacteria** | Modo **Generic** en el FBX | Evitar que camine hacia atrás o se deforme. | Animaciones de caminata y carrera naturales. |
| **Muerte 3D** | Freno en `idle` + Enfoque a cabeza | Crear un susto directo cara a cara. | Jumpscare inmersivo con temblor de cámara y grito. |

---

*Documento generado para el seguimiento semanal y académico del proyecto CUBERUN 3D.*
