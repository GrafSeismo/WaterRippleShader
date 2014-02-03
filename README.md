WaterRippleShader
=================
Demonstrate three different water (distortion) shader in XNA 4.0.
All three are based on examples i found in Internet:
* Distortion image and ImageDistortionShader based on http://gamedev.stackexchange.com/a/18202 from Jonathan Dickinson
* Shockwave based on Forum entry http://www.xnamag.de/forum/viewtopic.php?t=2740&highlight=schockwelle
* SineWave based on http://www.mindyourbyte.de/de/xna-de/12-xna-advanced/57-xna-draw-multiple-shaders
My two cents are bringing all togther.


Requirements
============
* Visual Studio 2010
* .NET Framework 4.0
* Game Studio 4.0 (refresh)
* A monitor with 1080p resolution (or change size in code)

Known Issues / TODO's
=====================
For every drip an own complete drawcall (spriteBatch.Begin()->spriteBatch.End()) is needed and slows down application dramatically.
