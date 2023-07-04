# Animaciones
*Se recomienda antes de leer este documento leer el documento referente a **SpriteSheets**.*  

Este documento contiene toda la información necesaria para poder crear, cargar e implementar animaciones dentro del proyecto. Estas animaciones hacen referencia a la interpolación de fotogramas desde un spritesheet.  

Primero veremos los recursos necesarios para crear una animación y luego su implementación dentro del código.

## Recursos  
Las animaciones requieren un **archivo** externo que contenga la información sobre estas. Es requerido también que se cuente con una **textura** y un **spritesheet** con los que va a funcionar la animación.

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

Este spritesheet contiene información para cuatro potenciales animaciones: *Idle*, *Run*, *Tornado* y *Climb*.  
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

El archivo divide las múltiples animaciones con la etiqueta `Animation`.  
Cada animacion contiene los parámetros:  
`Name` para determinar el nombre de la animación. No se puede repetir dentro del mismo documento.  
`SpriteSheet` para el nombre del SpriteSheet que va a utilizar.  
`Frames` con la información de los fotogramas de la animación.  

Dentro del **Frames** se contienen los siguientes parámetros:  
`Sheet` con el nombre del fotograma que se quiera usar, referente a los fotográmas del spritesheet utilizado.  
`Duration` con la duración que permanecerá el fotograma antes de pasar al siguiente, especificado en milisegundos.  

Los archivos de animación XML se almacenan en el directorio *Content\Data\Animations\...* y debe ser compilado por el **ContentManager**.  

## Código  
Los archivos de animación son cargados en la clase `Animations` (*HorrorShorts.Resources.Animations*) mediante el método **ReLoad()**.
Esta clase almacena diccionarios `<string, AnimationData>`, siendo la **llave** el nombre de la animacion y el **valor** la información de la animación.

### AnimationSystem
La clase AnimationSystem (*HorrorShorts\Controls\Animations\AnimationSystem.cs*) sirve para reproducir animaciones. Las principales características son:  

**Propiedades:**  
`Source` Contiene el sheet actual de la animación.  
`FrameChanged` Un booleano que es **true** cuando el fotograma acaba de cambiar.  
`BucleType` Especifica el tipo de bucle. Puede ser *None*, *Forward*, *Reverse* o *PingPong*.  
`SpeedMod` Modifica la velocidad de la animación. Por defecto es **1**.  
`State` El estado de la animacion. Puede ser *Playing*, *Stopped*, *Paused*.  

**Métodos:**  
`SetAnimation(AnimationData)` Carga una animación en la clase.  
`SwapAnimation(AnimationData)` Cambia la animación en reproducción.  
`Play()` Empieza a reproducir la animación.  
`Pause()` Pausa la animación.  
`Resume()` Reanuda la animación.  
`Stop` Detiene la animación.  


Cabe aclarar que **AnimationSystem** solo sirve para generar la interpolación entre fotogramas, mas no genera ningún tipo de renderizado ni actualiza los sprites directamente.  
Ademas es posible que para casos particulares sea necesario crear un sistema de animación propio para satisfacer las necesidades específicas.  
