# SpriteSheet
Este documento contiene toda la información de como crear y cargar un spritesheet en el proyecto.  
Los spritesheets son conjuntos de sprites dentro de una única textura, lo que, a diferencia de tener los sprites como múltiples archivos, reduce el peso total del proyecto y aliviana el uso de RAM y GPU en tiempo de ejecución.  

## Recursos  
Los SpriteSheets se dividen en la **imagen** con todas las texturas y el **XML** con la información necesaria sobre cada sprite.  

### Textura    
La textura debe contener en su interior todos los sprites que se requieras.  
Hay algunas consideraciones a tener en cuenta a la hora de crear la textura:  
- Preferentemente el tamaño total del a textura debe ser una potencia de 2 en pixeles.  
- Utilizar texturas de 8 bits de profundidad con colores indexados.  
- Se recomienda que en caso de utilizar sprites de un personaje o casos similares se utiliza un mismo tamaño para cada sprite, por ejemplo 32 x 32 px.  
- Agrupar sprites por tipo de animación. Por ejemplo todos los referentes a correr primero, luego los de saltar, etc.  

Se explorara primero la parte del recurso (textura + XML) y después su implementación dentro del código.

### Archivo XML  
Los archivos XML de los spritesheets contienen la información sobre todos los sprites internos de la textura.  
A continuación se da un ejemplo de un XML de spritesheet:  

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
A continuación se contiene se despliega un listado de `Sheets`. 
Cada uno de los Sheet contiene el `Name` haciendo referencia a el nombre del Sprite y `Source` haciendo referencia a su ubicación dentro de la textura. Es importante que ningún nombre se repita. Los Source se componen de: *Posicion X, Posición Y, Ancho, Alto* en pixeles.

#### Nombres Reservados
Existen algunos nombres de Sprite reservados para ciertas funcionalidades específicas:

##### Dialog Face  
Utilizado para las caras que se muestran en el DialogBox.  

`DialogFace_[face]`

**[face]** se remplazaría por cualquier tipo de cara de la enumeración **FaceType**  

---

Los archivos xml de spritesheet suelen ser almacenados dentro de la carpeta *Content\Data\SpriteSheets\...* y requieren ser procesados por el **ContentManager**.  
Las texturas pueden ir ordenadas en subdirectorios dentro de la carpeta *Content\Textures\...* y tambien deben ser procesados por el **ContentManager**.  

## Código  
Los spritesheets son cargados en la clase `SpriteSheets` (*HorrorShorts.Resources.SpriteSheets*) mediante el método **ReLoad()**. Esta clase almacena instancias del tipo `SpriteSheet` (*HorrorShorts.Controls.Sprites.SpriteSheet*)

Al tomar un spritesheet podés pedirle el **Source Rectangle** de un sprite interno mediante el método **Get(string)**, enviandole como parámetro el nombre del sprite que querés tomar (especificado mediante el parámetro `Name` del archivo XML original).

