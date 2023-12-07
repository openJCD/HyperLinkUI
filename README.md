
# VESSEL_GUI

----

    Stylable Event-Based UI Framework for Monogame applications

----

## Installation
    Installation has not been implemented on NuGet or anything like that yet

## Creating a basic scene
The `Window()` class provides a good starting point here, as it is a nicely decorated container for all manner of widgets.
First though, some basic declarations must be made. 
Start by declaring `settings = new GameSettings()` within the `Game1.Initialise()` method. This argument-free constructor initialises the `GameSettings` class with the default colour scheme shown below (ADD SCREENSHOT HERE), as well as a 640 × 480 resolution and window title "Window".
Now pass your `GraphicsDeviceManager` and `settings` into
`Root screenRoot = new Root(<name of your GraphicsDeviceManager instance>, settings)`.
This creates a new Root for our UI. (A lot of setup so far, huh...) This can be passed as an argument to any `class Container()` to set as that Container's parent. It is also responsible for calling for `Update` and `Draw` calls of all items in the UI Tree. 
Create and load in a `SpriteFont` using the content pipeline and call it `font`
Now create an instance of class Window: 

`Window testwindow = new Window(Root, 0, 0, 100, 100, font)`

(CONTINUE, ADD CODE FOR UPDATE AND DRAW)


    
