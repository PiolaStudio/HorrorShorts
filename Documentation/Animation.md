# Animaciones
*Se recomienda antes de leer este documento leer el documento referente a **SpriteSheets**.*  

Este documento contiene toda la informaci�n necesaria para poder crear, cargar e implementar animaciones dentro del proyecto. Estas animaciones hacen referencia a la interpolaci�n de fotogramas desde un spritesheet.  

Primero veremos los recursos necesarios para crear una animaci�n y luego su implementaci�n dentro del c�digo.

## Recursos  
Las animaciones requieren un **archivo** externo que contenga la informaci�n sobre estas. Es requerido tambi�n que se cuente con una **textura** y un **spritesheet** con los que va a funcionar la animaci�n.

### Archivo XML  
Antes de exponer el XML de las animaciones es necesario saber el **spritesheet** al cual vamos a estar haciendo referencia:  

```xml
<?xml version="1.0" encoding="utf-8"?>
<XnaContent xmlns:Sprites="Resources.Sprites">
	<Asset Type="Sprites:SpriteSheet_Serial">
		<Texture>Megaman</Texture>
		<Sheets>
			<!--IDLE-->
			<Sheet>
				<Name>Idle_1</Name>
				<Source>0 0 32 32</Source>
			</Sheet>
			<Sheet>
				<Name>Idle_2</Name>
				<Source>32 0 32 32</Source>
			</Sheet>
			<!--RUN-->
			<Sheet>
				<Name>Run_1</Name>
				<Source>0 32 32 32</Source>
			</Sheet>
			<Sheet>
				<Name>Run_2</Name>
				<Source>32 32 32 32</Source>
			</Sheet>
			<Sheet>
				<Name>Run_3</Name>
				<Source>64 32 32 32</Source>
			</Sheet>
			<!--TORNADO-->
			<Sheet>
				<Name>Tornado_1</Name>
				<Source>0 64 32 32</Source>
			</Sheet>
			<Sheet>
				<Name>Tornado_2</Name>
				<Source>32 64 32 32</Source>
			</Sheet>
			<Sheet>
				<Name>Tornado_3</Name>
				<Source>64 64 32 32</Source>
			</Sheet>
			<Sheet>
				<Name>Tornado_4</Name>
				<Source>96 64 32 32</Source>
			</Sheet>
			<!--CLIMB-->
			<Sheet>
				<Name>Climb_1</Name>
				<Source>0 96 32 32</Source>
			</Sheet>
			<Sheet>
				<Name>Climb_2</Name>
				<Source>32 96 32 32</Source>
			</Sheet>
		</Sheets>
	</Asset>
</XnaContent>
```

Este spritesheet contiene informaci�n para cuatro potenciales animaciones: *Idle*, *Run*, *Tornado* y *Climb*.  
Ahora con esto establecido vamos a ver un ejemplo de archivo XML que contiene las animaciones:  

```xml
<?xml version="1.0" encoding="utf-8"?>
<XnaContent xmlns:Sprites="Resources.Sprites">
  <Asset Type="Sprites:Animation_Serial">
    <Animation>
      <Name>Idle</Name>
      <SpriteSheet>Megaman</SpriteSheet>
      <Frames>
        <Frame>
          <Sheet>Idle_1</Sheet>
          <Duration>1000</Duration>
        </Frame>
        <Frame>
          <Sheet>Idle_2</Sheet>
          <Duration>100</Duration>
        </Frame>
      </Frames>
    </Animation>
    <Animation>
      <Name>Run</Name>
      <SpriteSheet>Megaman</SpriteSheet>
      <Frames>
        <Frame>
          <Sheet>Run_1</Sheet>
          <Duration>150</Duration>
        </Frame>
        <Frame>
          <Sheet>Run_2</Sheet>
          <Duration>120</Duration>
        </Frame>
		<Frame>
          <Sheet>Run_3</Sheet>
          <Duration>150</Duration>
        </Frame>
      </Frames>
    </Animation>
	<Animation>
      <Name>Tornado</Name>
      <SpriteSheet>Megaman</SpriteSheet>
      <Frames>
        <Frame>
          <Sheet>Tornado_1</Sheet>
          <Duration>80</Duration>
        </Frame>
		<Frame>
          <Sheet>Tornado_2</Sheet>
          <Duration>80</Duration>
        </Frame>
		<Frame>
          <Sheet>Tornado_3</Sheet>
          <Duration>80</Duration>
        </Frame>
		<Frame>
          <Sheet>Tornado_4</Sheet>
          <Duration>80</Duration>
        </Frame>
      </Frames>
    </Animation>
	<Animation>
      <Name>Climb</Name>
      <SpriteSheet>Megaman</SpriteSheet>
      <Frames>
        <Frame>
          <Sheet>Climb_1</Sheet>
          <Duration>200</Duration>
        </Frame>
		<Frame>
          <Sheet>Climb_2</Sheet>
          <Duration>200</Duration>
        </Frame>
      </Frames>
    </Animation>
  </Asset>
</XnaContent>
```

El archivo divide las m�ltiples animaciones con la etiqueta `Animation`.  
Cada animacion contiene los par�metros:  
`Name` para determinar el nombre de la animaci�n. No se puede repetir dentro del mismo documento.  
`SpriteSheet` para el nombre del SpriteSheet que va a utilizar.  
`Frames` con la informaci�n de los fotogramas de la animaci�n.  

Dentro del **Frames** se contienen los siguientes par�metros:  
`Sheet` con el nombre del fotograma que se quiera usar, referente a los fotogr�mas del spritesheet utilizado.  
`Duration` con la duraci�n que permanecer� el fotograma antes de pasar al siguiente, especificado en milisegundos.  

Los archivos de animaci�n XML se almacenan en el directorio *Content\Data\Animations\...* y debe ser compilado por el **ContentManager**.  

## C�digo  
Los archivos de animaci�n son cargados en la clase `Animations` (*HorrorShorts.Resources.Animations*) mediante el m�todo **ReLoad()**.
Esta clase almacena diccionarios `<string, AnimationData>`, siendo la **llave** el nombre de la animacion y el **valor** la informaci�n de la animaci�n.

### AnimationSystem
La clase AnimationSystem (*HorrorShorts\Controls\Animations\AnimationSystem.cs*) sirve para reproducir animaciones. Las principales caracter�sticas son:  

**Propiedades:**  
`Source` Contiene el sheet actual de la animaci�n.  
`FrameChanged` Un booleano que es **true** cuando el fotograma acaba de cambiar.  
`BucleType` Especifica el tipo de bucle. Puede ser *None*, *Forward*, *Reverse* o *PingPong*.  
`SpeedMod` Modifica la velocidad de la animaci�n. Por defecto es **1**.  
`State` El estado de la animacion. Puede ser *Playing*, *Stopped*, *Paused*.  

**M�todos:**  
`SetAnimation(AnimationData)` Carga una animaci�n en la clase.  
`SwapAnimation(AnimationData)` Cambia la animaci�n en reproducci�n.  
`Play()` Empieza a reproducir la animaci�n.  
`Pause()` Pausa la animaci�n.  
`Resume()` Reanuda la animaci�n.  
`Stop` Detiene la animaci�n.  


Cabe aclarar que **AnimationSystem** solo sirve para generar la interpolaci�n entre fotogramas, mas no genera ning�n tipo de renderizado ni actualiza los sprites directamente.  
Ademas es posible que para casos particulares sea necesario crear un sistema de animaci�n propio para satisfacer las necesidades espec�ficas.  
