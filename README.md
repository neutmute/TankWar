# Tank War #

An experiment in using SignalR & Websockets.

Battle other players in real time, controlling your tank's turrent using two sliders to control your angle and power.

![](https://raw.github.com/neutmute/TankWar/master/Source/TankWar/Content/slides/gamepad.png)

Each change to a slider is transmitted via signalR to the server. Those settings are used to calculate the projectile motion of a shell upon firing. 
View the action in a different browser window via the ViewPort which is receiving and rendering the updated shell positions on every game tick. As many viewport windows can be opened as you wish.

![](https://raw.github.com/neutmute/TankWar/master/Source/TankWar/Content/slides/viewport.png)

The server runs the game loop, ticking time forward every 25ms.
The canvas and sprites are rendered using [sprite.js](https://github.com/batiste/sprite.js/).

![](https://raw.github.com/neutmute/TankWar/master/Source/TankWar/Content/slides/BlockDiagram.gif)

Tweak game parameters and control the game using the admin page.