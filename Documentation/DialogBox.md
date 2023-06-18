# Dialogos
Este documento contiene la información necesaria para implementar el sistema de dialogos en el juego, desde como manejar los archivos XML con los textos hasta como usar el DialogBox desde el código.  
Los dialogos se pueden utilizar para la comunicación entre personajes, pensamientos de un mismo personaje, información, relatos del narrador, redacción de efectos de sonido, etc.  

---

## Recursos (Archivos XML)
Los archivos XML contienen múltiples dialogos con información adicional de como se deben mostrar.  
Primero se divide en distintos **Conversation** encargados de almacenar la información de una conversación completa cada uno (desde que se comienza a mostrar en el DialogBox hasta que se cierra)  
Cada conversation debe ser bautizada mediante el parámetro ID el cual no se puede repetir en el mismo archivo.
Dentro de cada Conversation se almacenan cuantos **Dialog** se necesiten, encargados cada uno de contener toda la información de un solo dialogo.  

```xml
<?xml version="1.0" encoding="utf-8"?>
<XnaContent xmlns:Dialogs="Resources.Dialogs">
  <Asset Type="Dialogs:Conversation_Serial">
    <Conversation>
      <ID>Conversación 1</ID>
      <Dialogs>
        <Dialog>
          <Text>Hola, este es el primer dialogo de la conversación.</Text>
        </Dialog>
        <Dialog>
          <Text>Y este es el segundo y está en rojo</Text>
		  <FontColor>FF0000</FontColor>
        </Dialog>
        <Dialog>
          <Text>Y yo soy una minita feliz jijiji</Text>
		  <Character>Girl1</Character>
          <Face>Happy</Face>
        </Dialog>
      </Dialogs>
    </Conversation>
    <Conversation>
      <ID>Conversación 2</ID>
      <Dialogs>
        <Dialog>
          <Text>Esta es otra conversación...</Text>
        </Dialog>
        <Dialog>
          <Text>NO{dy:200}-TE{dy:200}-ME{dy:200}-TAS</Text>
        </Dialog>
      </Dialogs>
    </Conversation>
  </Asset>
</XnaContent>
```  

### Parámetros iniciales:  
Cada dialogo individual se le puede configurar una serie de parametros para determinar como se va a visualizar en pantalla.  
Es importante mantener el orden de estos parámetros, en caso contrario no se podrá compilar correctamente. Sin embargo hay parámetros optativos que se detallarán más adeltante.  
El listado de parámetros iniciales es el siguiente:  

#### - Text
El texto que se mostrará en el DialogBox.  

```xml
<Text>Hola Mundo</Text>
```  
#### - Character
Determina el personaje que va a decir el dialogo.  

```xml
<Character>Nombre_Del_Personaje</Character>
```  

En caso de no especificarlo se usa al narrador.

#### - Face  
Determina la expresión que el personaje va a utilizar.  

```xml
<Face>Normal</Face>
```  

Los tipos de caras disponibles se encuentran en la enumeración FaceType.  
En caso de usar **None** no se mostraría ninguna cara.  

#### - FontType
Determina el tipo de fuente que se va a utilizar.

```xml
<FontType>Arial</FontType>
```  

Los tipos de fuentes disponibles se encuentran en la enumeración FaceType.  

#### - FontSize  
Determina el tamaño de la fuente.   

```xml
<FontSize>2</FontSize>  
```

Se recomienda utilizar valores potencia de 2 *(1, 2, 4, 8, etc)*.  

#### - FontColor
Determina el color del texto en hexadecimal.  

```xml
<FontColor>FFFFFF</FontColor>
```

Es un valor **hexadecimal** que representa el R, G y B con dos dígitos para cada uno.  

#### - Speed
Determina la velocidad en la que va a ir apareciendo el texto. Influye en otras cosas como la velocidad del sonido.

```xml
<Speed>10</Speed>
```

Es un valor entre **1** y **1000**.

#### - SpeakType
Determina el tipo de *"voz"* que el personaje proyectará al hablar. Es el sonido base que se reproducirá cada X caracteres.

```xml
<SpeakType>Speak1</SpeakType>
```

Los tipos de speak disponibles se encuentran en la enumeración SpeakType.  
En caso de usar **None** no emitiría ningún sonido.  

#### - SpeakSpeed
Determina cada cuantos caracteres ejecutará un nuevo sonido de Speak. Mientras mayor sea el número mas tiempo habrá entre sonido y sonido.  

```xml
<SpeakSpeed>3</SpeakSpeed>
```

Es un valor entre **1** y **100**

#### - SpeakPitch
Determina el pitch *(Altura)* base que se utilizará para ejecutar el sonido.  

```xml
<SpeakPitch>0</SpeakPitch>
```

En un valor entre **-100** y **100**, siendo **0** el sonido sin modificaciones.

#### -SpeakPitchVariation  
Determina cuanto va a variar el pitch a partir de su valor inicial **SpeakPitch**.  

```xml
<PitchVariation>0</PitchVariation>  
```

En un valor entre **-100** y **100**, siendo **0** sin variaciones.  

#### -Location  
Determina donde va a estar ubicado el DialogBox en la pantalla.  

```xml
<Location>Bottom</Location>  
```

Los tipos de posiciones disponibles se encuentran en la enumeración **TextBoxPosition**.  

### Parametros Defaults  
Algunos parámetros no son obligatorios. En caso de no utilizarlos el juego determina que se quiere utilizar la configuración predeterminada para ese mismo.  

La lista de parámetros no obligatorios permitidos son la siguiente:  
- Character
- Face
- FontType, FontSize, FontColor
- Speed
- SpeakSound, SpeakSpeed, SpeakPitch, SpeakPitchVariation
- Location

Cabe destacar que cada personaje puede tener valores default distintos.  

---

### Comandos Inyectados
Los comandos inyectados son instrucciones que se pueden añadir al texto del parámetro **Text**. Estas sirven para modificar distintos aspectos mientras el DialogBox va mostrando el texto.  
Los comandos inyectodos se enmarcan entre corchetes **{}** y se le añade dos letras que determinan el tipo de comando, y un valor optativo separado por dos puntos **:**  
A continuación se detallan los distintos tipo de comandos inyectados disponibles:  

#### -Delay (dy)
Hace una pausa de X milisegundos en el lugar indicado.

```xml
<Text>Esto es un{dy:500} delay de 500ms</Text>
```

Debe ser un número mayor a **0**

#### -Font Type (ft)
Cambia el tipo de fuente a cualquiera de los validos

```xml
<Text>{ft:Arial}Esto está en Arial.{ft:ComicSans} Esto esta en ComicSans.</Text>
```

Los tipos de Fuentes validos son los de la enumeración **FontType**

#### - Font Color (fc)
Cambia el color del texto

```xml
<Text>{fc:#FFFFFF}Esto está en blanco.{fc:#FF0000} Esto esta en rojo.</Text>
```

Debe expresarse en hexadecimal, con valores de 255 (2 caracteres) para el RGB.

#### - Face (fa)
Cambia al cara que se está mostrando.

```xml
<Text>{fa:Happy}Estoy feliz{fa:Sad} y enojado.</Text>
```

No se puede cambiar la cada del dialogo si inicialmente se especificó al **FaceType** como **None**
Los tipos de caras disponibles son los de la enumeración **FaceType**

#### - Break Line (bl)
Realiza un salto de línea

```xml
<Text>Línea 1.{bl}Línea 2.</Text>
```

No requiere parámetros adicionales.

#### - Escape Character (ec)
Permite mostrar un caracter reservado.

```xml
<Text>{ec:{}Este texto está entre llaves{ec:}}</Text>
```

#### - Speak Type (st)
Permite cambiar el sonido que utiliza para "hablar"

```xml
<Text>{st:Voice1}Esta es mi primera voz{st:Voice2}} y esta mi segunda.</Text>
```

Los tipos de voces validos son los de la enumeración **SpeakTypes**

#### - Speak Pitch (sp)
Permite cambiar el pitch (altura) que utiliza para "hablar"

```xml
<Text>{sp:50}Ahora hablo más agudo{sp:-50} y ahora más grave.</Text>
```

Los valores permitidos son entre **-100** y **100**
**0** se utiliza para desactivar la variación.  

#### - Speak Pitch Variation (sv)
Determina el rango de pitch en el que puede variar cada vez que se reproduzca un sonido al "hablar".  

```xml
<Text>{sv:0}No varia mi voz al hablar{sv:10} y ahora si.</Text>
```

Los valores permitidos son entre **0** y **100**.  
**0** se utiliza para desactivar la variación.  

#### - SpeakSpeed (ss)
Determina cada cuantos caractéres mostrados se ejecutará un sonido para "hablar". Mientras mayor sea el valor más lento hablara.

```xml
<Text>{ss:4}Velocidad 1{ss:1} Velicidad 2.</Text>
```

Los valores permitidos son entre **1** (para hablar sobre cada caractér) y **100**

#### - TextSpeed (ts)
Determina la velocidad **general** con la que irá apareciendo el texto sobre el DialogBox.

```xml
<Text>{ts:6}Estoy hablando lento{ts:30}Y ahora rápido.</Text>
```

Los valores permitidos son entre **1** y **100**

#### - Event (ev)  
Ejecuta un evento dentro del código.  
//todo

#### - SoundEffect (se)  
Reproduce un efecto de sonido.  

```xml
<Text>Va a sonar un sonido en 3, 2, 1, YA{se:Sound1}.</Text>
```

Es valido cualquier efecto de sonido de la clase **Sounds**.  

#### - VibrateBox (vb)
Agita la caja de dialogos con la fuerza especificada.

```xml
<Text>¿Esto es un terremoto?{vb:1}.</Text>
```

Los valores validos van entre **1** y **10**.  

---

## Código  
Todos los dialogos se guardan en la carpeta *Content\Data\Localization* y son cargados de forma dinámica en la clase Dialogs mediante el método **Reload()**  
A continuación se dará una explicación de los controles **DialogBox** y **DialogManagement** encargados de gestionar los diálogos cargados desde los archivos XML o creados en tiempo de ejecución.  

---

### Dialog Box
Este control se puede utilizar para mostrar texto como si un personaje esté hablando.  
Para usarlo simplemente se llama al método **Show** como en el siguiente ejemplo:

```c#
Dialog dialog = ...
dialogBox.Show(dialog);
```

Este código se encarga de mostrar el diálogo con todas las configuraciones que se le hayan dado a la isntancia **dialog**
DialogManagement requiere sus respectivos llamados a LoadContent, Update, PreDraw, Draw y Dispose

---

### Dialog Management
Este control se encarga de gestionar múltiples líneas de dialogo e ir administarndoselas a un DialogBox.  
Para usarlo simplemente se llama al método **Start** como en el siguiente ejemplo:  

```c#
Dialog[] dialogs = ...
dialogManagement.Start(dialogs);
```

Este código se encarga de ir mostrando todos los items del array dialogs en orden.  
DialogManagement requiere sus respectivos llamados a LoadContent, Update, PreDraw, Draw y Dispose  

Se encuentra un DialogManagement listo para su utilización en Core:  
```c#
Core.DialogManagement.Start(dialogs);
```