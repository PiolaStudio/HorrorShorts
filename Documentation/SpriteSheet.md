# SpriteSheet
Este documento contiene toda la informaci�n de como crear y cargar un spritesheet en el proyecto.  
Los spritesheets son conjuntos de sprites dentro de una �nica textura, lo que, a diferencia de tener los sprites como m�ltiples archivos, reduce el peso total del proyecto y aliviana el uso de RAM y GPU en tiempo de ejecuci�n.  

## Recursos  
Los SpriteSheets se dividen en la **imagen** con todas las texturas y el **XML** con la informaci�n necesaria sobre cada sprite.  

### Textura    
La textura debe contener en su interior todos los sprites que se requieras.  
Hay algunas consideraciones a tener en cuenta a la hora de crear la textura:  
- Preferentemente el tama�o total del a textura debe ser una potencia de 2 en pixeles.  
- Utilizar texturas de 8 bits de profundidad con colores indexados.  
- Se recomienda que en caso de utilizar sprites de un personaje o casos similares se utiliza un mismo tama�o para cada sprite, por ejemplo 32 x 32 px.  
- Agrupar sprites por tipo de animaci�n. Por ejemplo todos los referentes a correr primero, luego los de saltar, etc.  

Se explorara primero la parte del recurso (textura + XML) y despu�s su implementaci�n dentro del c�digo.

### Archivo XML  
Los archivos XML de los spritesheets contienen la informaci�n sobre todos los sprites internos de la textura.  
A continuaci�n se da un ejemplo de un XML de spritesheet:  

```xml
<?xml version="1.0" encoding="utf-8"?>
<XnaContent xmlns:Sprites="Resources.Sprites">
	<Asset Type="Sprites:SpriteSheet_Serial">
		<Texture>Mario</Texture>
		<Sheets>
			<Sheet>
				<Name>Run</Name>
				<Source>0 0 16 16</Source>
			</Sheet>
			<Sheet>
				<Name>Idle</Name>
				<Source>16 0 16 16</Source>
			</Sheet>
			<Sheet>
				<Name>Death</Name>
				<Source>32 0 16 16</Source>
			</Sheet>
		</Sheets>
	</Asset>
</XnaContent>
```

En el campo `<Texture>` se almacena el nombre de la textura a la que hace referencia el SpriteSheet.  
A continuaci�n se contiene se despliega un listado de `Sheets`. 
Cada uno de los Sheet contiene el `Name` haciendo referencia a el nombre del Sprite y `Source` haciendo referencia a su ubicaci�n dentro de la textura. Es importante que ning�n nombre se repita. Los Source se componen de: *Posicion X, Posici�n Y, Ancho, Alto* en pixeles.

#### Nombres Reservados
Existen algunos nombres de Sprite reservados para ciertas funcionalidades espec�ficas:

##### Dialog Face  
Utilizado para las caras que se muestran en el DialogBox.  

`DialogFace_[face]`

**[face]** se remplazar�a por cualquier tipo de cara de la enumeraci�n **FaceType**  

---

Los archivos xml de spritesheet suelen ser almacenados dentro de la carpeta *Content\Data\SpriteSheets\...* y requieren ser procesados por el **ContentManager**.  
Las texturas pueden ir ordenadas en subdirectorios dentro de la carpeta *Content\Textures\...* y tambien deben ser procesados por el **ContentManager**.  

## C�digo  
Los spritesheets son cargados en la clase `SpriteSheets` (*HorrorShorts.Resources.SpriteSheets*) mediante el m�todo **ReLoad()**. Esta clase almacena instancias del tipo `SpriteSheet` (*HorrorShorts.Controls.Sprites.SpriteSheet*)

Al tomar un spritesheet pod�s pedirle el **Source Rectangle** de un sprite interno mediante el m�todo **Get(string)**, enviandole como par�metro el nombre del sprite que quer�s tomar (especificado mediante el par�metro `Name` del archivo XML original).

