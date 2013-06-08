# Tank War #

An experiment in using SignalR & Websockets.

Battle other players in real time, controlling your tank's turrent using two sliders to control your angle and power.

Each change to a slider is transmitted via signalR to the GamePadHub and used to calculate the projectile motion of a shell upon firing.

The server runs the game loop, ticking time forward every 25ms, calculating the updated shell positions and detecting shell/tank collisions.

View the action via the ViewPort which is receiving the updated turret settings and shell positions on every game tick. The canvas and sprites are rendered using [sprite.js](https://github.com/batiste/sprite.js/).
