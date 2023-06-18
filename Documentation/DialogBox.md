# Dialogos
Este documento contiene la informaci�n necesaria para implementar el sistema de dialogos en el juego, desde como manejar los archivos XML con los textos hasta como usar el DialogBox desde el c�digo.  
Los dialogos se pueden utilizar para la comunicaci�n entre personajes, pensamientos de un mismo personaje, informaci�n, relatos del narrador, redacci�n de efectos de sonido, etc.  

---

## Recursos (Archivos XML)
Los archivos XML contienen m�ltiples dialogos con informaci�n adicional de como se deben mostrar.  
Primero se divide en distintos **Conversation** encargados de almacenar la informaci�n de una conversaci�n completa cada uno (desde que se comienza a mostrar en el DialogBox hasta que se cierra)  
Cada conversation debe ser bautizada mediante el par�metro ID el cual no se puede repetir en el mismo archivo.
Dentro de cada Conversation se almacenan cuantos **Dialog** se necesiten, encargados cada uno de contener toda la informaci�n de un solo dialogo.  

```xml
<?xml version="1.0" encoding="utf-8"?>
<XnaContent xmlns:Dialogs="Resources.Dialogs">
  <Asset Type="Dialogs:Conversation_Serial">
    <Conversation>
      <ID>Conversaci�n 1</ID>
      <Dialogs>
        <Dialog>
          <Text>Hola, este es el primer dialogo de la conversaci�n.</Text>
        </Dialog>
        <Dialog>
          <Text>Y este es el segundo y est� en rojo</Text>
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
      <ID>Conversaci�n 2</ID>
      <Dialogs>
        <Dialog>
          <Text>Esta es otra conversaci�n...</Text>
        </Dialog>
        <Dialog>
          <Text>NO{dy:200}-TE{dy:200}-ME{dy:200}-TAS</Text>
        </Dialog>
      </Dialogs>
    </Conversation>
  </Asset>
</XnaContent>
```  

### Par�metros iniciales:  
Cada dialogo individual se le puede configurar una serie de parametros para determinar como se va a visualizar en pantalla.  
Es importante mantener el orden de estos par�metros, en caso contrario no se podr� compilar correctamente. Sin embargo hay par�metros optativos que se detallar�n m�s adeltante.  
El listado de par�metros iniciales es el siguiente:  

#### - Text
El texto que se mostrar� en el DialogBox.  

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
Determina la expresi�n que el personaje va a utilizar.  

```xml
<Face>Normal</Face>
```  

Los tipos de caras disponibles se encuentran en la enumeraci�n FaceType.  
En caso de usar **None** no se mostrar�a ninguna cara.  

#### - FontType
Determina el tipo de fuente que se va a utilizar.

```xml
<FontType>Arial</FontType>
```  

Los tipos de fuentes disponibles se encuentran en la enumeraci�n FaceType.  

#### - FontSize  
Determina el tama�o de la fuente.   

```xml
<FontSize>2</FontSize>  
```

Se recomienda utilizar valores potencia de 2 *(1, 2, 4, 8, etc)*.  

#### - FontColor
Determina el color del texto en hexadecimal.  

```xml
<FontColor>FFFFFF</FontColor>
```

Es un valor **hexadecimal** que representa el R, G y B con dos d�gitos para cada uno.  

#### - Speed
Determina la velocidad en la que va a ir apareciendo el texto. Influye en otras cosas como la velocidad del sonido.

```xml
<Speed>10</Speed>
```

Es un valor entre **1** y **1000**.

#### - SpeakType
Determina el tipo de *"voz"* que el personaje proyectar� al hablar. Es el sonido base que se reproducir� cada X caracteres.

```xml
<SpeakType>Speak1</SpeakType>
```

Los tipos de speak disponibles se encuentran en la enumeraci�n SpeakType.  
En caso de usar **None** no emitir�a ning�n sonido.  

#### - SpeakSpeed
Determina cada cuantos caracteres ejecutar� un nuevo sonido de Speak. Mientras mayor sea el n�mero mas tiempo habr� entre sonido y sonido.  

```xml
<SpeakSpeed>3</SpeakSpeed>
```

Es un valor entre **1** y **100**

#### - SpeakPitch
Determina el pitch *(Altura)* base que se utilizar� para ejecutar el sonido.  

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

Los tipos de posiciones disponibles se encuentran en la enumeraci�n **TextBoxPosition**.  

### Parametros Defaults  
Algunos par�metros no son obligatorios. En caso de no utilizarlos el juego determina que se quiere utilizar la configuraci�n predeterminada para ese mismo.  

La lista de par�metros no obligatorios permitidos son la siguiente:  
- Character
- Face
- FontType, FontSize, FontColor
- Speed
- SpeakSound, SpeakSpeed, SpeakPitch, SpeakPitchVariation
- Location

Cabe destacar que cada personaje puede tener valores default distintos.  

---

### Comandos Inyectados
Los comandos inyectados son instrucciones que se pueden a�adir al texto del par�metro **Text**. Estas sirven para modificar distintos aspectos mientras el DialogBox va mostrando el texto.  
Los comandos inyectodos se enmarcan entre corchetes **{}** y se le a�ade dos letras que determinan el tipo de comando, y un valor optativo separado por dos puntos **:**  
A continuaci�n se detallan los distintos tipo de comandos inyectados disponibles:  

#### -Delay (dy)
Hace una pausa de X milisegundos en el lugar indicado.

```xml
<Text>Esto es un{dy:500} delay de 500ms</Text>
```

Debe ser un n�mero mayor a **0**

#### -Font Type (ft)
Cambia el tipo de fuente a cualquiera de los validos

```xml
<Text>{ft:Arial}Esto est� en Arial.{ft:ComicSans} Esto esta en ComicSans.</Text>
```

Los tipos de Fuentes validos son los de la enumeraci�n **FontType**

#### - Font Color (fc)
Cambia el color del texto

```xml
<Text>{fc:#FFFFFF}Esto est� en blanco.{fc:#FF0000} Esto esta en rojo.</Text>
```

Debe expresarse en hexadecimal, con valores de 255 (2 caracteres) para el RGB.

#### - Face (fa)
Cambia al cara que se est� mostrando.

```xml
<Text>{fa:Happy}Estoy feliz{fa:Sad} y enojado.</Text>
```

No se puede cambiar la cada del dialogo si inicialmente se especific� al **FaceType** como **None**
Los tipos de caras disponibles son los de la enumeraci�n **FaceType**

#### - Break Line (bl)
Realiza un salto de l�nea

```xml
<Text>L�nea 1.{bl}L�nea 2.</Text>
```

No requiere par�metros adicionales.

#### - Escape Character (ec)
Permite mostrar un caracter reservado.

```xml
<Text>{ec:{}Este texto est� entre llaves{ec:}}</Text>
```

#### - Speak Type (st)
Permite cambiar el sonido que utiliza para "hablar"

```xml
<Text>{st:Voice1}Esta es mi primera voz{st:Voice2}} y esta mi segunda.</Text>
```

Los tipos de voces validos son los de la enumeraci�n **SpeakTypes**

#### - Speak Pitch (sp)
Permite cambiar el pitch (altura) que utiliza para "hablar"

```xml
<Text>{sp:50}Ahora hablo m�s agudo{sp:-50} y ahora m�s grave.</Text>
```

Los valores permitidos son entre **-100** y **100**
**0** se utiliza para desactivar la variaci�n.  

#### - Speak Pitch Variation (sv)
Determina el rango de pitch en el que puede variar cada vez que se reproduzca un sonido al "hablar".  

```xml
<Text>{sv:0}No varia mi voz al hablar{sv:10} y ahora si.</Text>
```

Los valores permitidos son entre **0** y **100**.  
**0** se utiliza para desactivar la variaci�n.  

#### - SpeakSpeed (ss)
Determina cada cuantos caract�res mostrados se ejecutar� un sonido para "hablar". Mientras mayor sea el valor m�s lento hablara.

```xml
<Text>{ss:4}Velocidad 1{ss:1} Velicidad 2.</Text>
```

Los valores permitidos son entre **1** (para hablar sobre cada caract�r) y **100**

#### - TextSpeed (ts)
Determina la velocidad **general** con la que ir� apareciendo el texto sobre el DialogBox.

```xml
<Text>{ts:6}Estoy hablando lento{ts:30}Y ahora r�pido.</Text>
```

Los valores permitidos son entre **1** y **100**

#### - Event (ev)  
Ejecuta un evento dentro del c�digo.  
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
<Text>�Esto es un terremoto?{vb:1}.</Text>
```

Los valores validos van entre **1** y **10**.  

---

## C�digo  
Todos los dialogos se guardan en la carpeta *Content\Data\Localization* y son cargados de forma din�mica en la clase Dialogs mediante el m�todo **Reload()**  
A continuaci�n se dar� una explicaci�n de los controles **DialogBox** y **DialogManagement** encargados de gestionar los di�logos cargados desde los archivos XML o creados en tiempo de ejecuci�n.  

---

### Dialog Box
Este control se puede utilizar para mostrar texto como si un personaje est� hablando.  
Para usarlo simplemente se llama al m�todo **Show** como en el siguiente ejemplo:

```c#
Dialog dialog = ...
dialogBox.Show(dialog);
```

Este c�digo se encarga de mostrar el di�logo con todas las configuraciones que se le hayan dado a la isntancia **dialog**
DialogManagement requiere sus respectivos llamados a LoadContent, Update, PreDraw, Draw y Dispose

---

### Dialog Management
Este control se encarga de gestionar m�ltiples l�neas de dialogo e ir administarndoselas a un DialogBox.  
Para usarlo simplemente se llama al m�todo **Start** como en el siguiente ejemplo:  

```c#
Dialog[] dialogs = ...
dialogManagement.Start(dialogs);
```

Este c�digo se encarga de ir mostrando todos los items del array dialogs en orden.  
DialogManagement requiere sus respectivos llamados a LoadContent, Update, PreDraw, Draw y Dispose  

Se encuentra un DialogManagement listo para su utilizaci�n en Core:  
```c#
Core.DialogManagement.Start(dialogs);
```