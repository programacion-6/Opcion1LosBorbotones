# Explicación del UML

Este archivo markdown esta hecho para explicar un poco mas a profundidad las elecciones que tuvimos como equipo al momento de crear nuestro UML. 

Explicaremos a grandes rasgos los patrones que aplicamos.

# Patron arquitectonico

Para la realizacion del UML estuvimos analizando el proyecto que escogimos y nos dimos cuenta de varios detalles, uno de estos es que podemos aplicar una estructura DDD (domain, driven, design), ya que las capas que esta estructura nos ofrece es aplicable al proyecto y de esta forma tendremos un codigo reutilizable y mantenible donde podremos aplicar patrones de diseño, y tambien podremos aplicaremos las reglas del clean code, open close, alta cohesion y bajo acoplamiento, entre otras.
A continuacion mostraremos un ejemplo de las capas que seguira nuestro proyecto:

![Layers Structure](./images/layers_structure.png)

Como vemos en la imagen el usuario se comunicara con la capa de presentacion, dicha capa de presentacion estara comunicado con la capa de infraestructura que tendra la logica que seguira nuestra aplicasion, y la capa de infraestructura estara comunicada con la capa de dominio, la cual tendra nuestras entidades e interfaces que definiran nuestro contrato (interfaces que si o si tendran que ser aplicadas, etc).

En resumen tomamos la desicion de aplicar ese patron arquitectonico ya que podremos tener un mejor codigo el cual sera mantenible y reutilizable, tendremos un proyecto ordenado el cual sera de facil entendimiento y tambien, dentro del equipo, podremos tener tareas menos dependientes y con mayor peso en nuestro tablero.

# Patrones de diseño

Los patrones que utilizamos son los siguientes:

## Strategy Pattern

Aplicamos strategy para el comportamiento que tendran nuestro repositories y nuestros datasources.
Ya que tendremos solo un repository que pedira un datasource como variable, y este datasource es una interfaz el cual debera ser implementada en cada datasource que nos creemos, podremos pasarle cualqueira de estas a nuestro repository

![Repository](./images/repository_impl.png)

Como vemos mi repository necesita de un datasource, mientras que mi datasource es una interfaz.

![IDatasource](./images/datasources.png)

## Factory Pattern

Hacemos uso del patron factory para la creacion de nuestros datasource, ya que definimos en nuestro domain, la interfaz que debera seguir cada datasource que creemos en un futuro. Cada datasource realizara la misma tarea (en este caso la comunicacion con la base de datos) pero a su manera.
Un ejemplo es el siguiente:

![IDatasource](./images/datasources.png)

![DatasourceImpl](./images/datasources_impl.png)

Donde podremos tener multiples datasource los cuales tendran que implementar la interfaz en comun.
Esto es muy importante dentro la aplicacion y se vera el porque cuando hablemos del bridge.


## Bridge Pattern
Hacemos uso del patron bridge para la comunicacion del datasource con el repository, ya que estas son clases estrechamente relacionadas y las dividimos en dos jerarquias separadas.

Un ejemplo es el siguiente:

![Datasource](./images/datasources.png)

![RepositoryImpl](./images/repository_impl.png)

Donde vemos que mi repository hace uso del datasource en forma de puente, mientras que todos los datasource que vaya a yo crear siguen una interfaz en comun y puedo pasarle cualquiera de estos datasource a mi repository.

El motivo de esta estructura se detallara al momento de hablar del datasource y repository pattern

## Command Pattern
![SearcherAndReportInterface](./images/searcher_report_interface.png)
![ReportImpl](./images/report_impl.png)
![SearchImpl](./images/search_impl.png)

Utilizamos el patrón Command para estructurar la lógica del buscador y la generacion de reportes. 
A continuación, se describe cómo se implementa:

1. Interfaz ICommand:

Define el metodo Execute(), que es implementado por todos los comandos concretos.
Provee una interfaz común para ejecutar diversas operaciones sin conocer sus detalles.

2. Interfaz IReport:

Implementa la interfaz ICommand.
Agrega el método GenerateReport(), que debe ser implementado por cualquier clase que genere un reporte especifico.

3. Interfaz ISearcher:

También implementa la interfaz ICommand.
Define el metodo Search(criteria), que debe ser implementado por cualquier clase que realice una operacion de busqueda
basada en criterios específicos.

4. Clases Concretas de Reporte:

- ReportBooksBorrowed: Implementa IReport para generar un reporte de libros prestados.
- ReportBooksOverdue: Implementa IReport para generar un reporte de libros vencidos.
- ReportPatronBorrowed: Implementa IReport para generar un reporte de los libros prestados a un usuario específico.

5. Clases Concretas de Búsqueda:

- PatronSearcher: Implementa ISearcher para buscar usuarios segun criterios específicos.
- BookSearcher: Implementa ISearcher para buscar libros basándose en criterios definidos.
## Datasource and Repository Pattern
!
Este es un patron clave para nuestra arquitectura DDD ya que podremos abstraer la logica del negocio de la fuenta de datos. Esto nos sirve para tener un codigo mucho mas mantenible, ya que el momento en que queramos cambiar de fuente de datos, podremos hacerlo de una forma sencilla y facil, solo implementando la interfaz datasource, mientras que solo tendremos un repository que sera el intermediario entre nuestra UI y el Datsource.

De esta forma al momento de cambiar de fuente de datos no tendremos que hacer grandes cambios al codigo, simplemente deberia pasarle el datasource a mi repository y ahi terminaria mi trabajo, no tendria que tocar nada de la UI. (A menos que cambie la logica del negocio a grandes rasgos)

Un ejemplo de este patron puede ser el siguiente:

![PatternExample](./images/repository_pattern.png)

Donde se detalla lo anteriormente mencionado, tendremos una base de datos X, tendre datasources que sabran como comunicarse con esa base de datos, puedo tener multiple datasources para cada base de datos, ese datasource se lo pasara al repository que sera UNICO y el repository comunicara a la APP con la informacion solicitada

Esta parte no necesita una imagen de ejemplo porque se ve reflejado en gran parte de nuestro UML

## Singleton Pattern

Tambien hicimos uso del singleton pattern para nuestras implementaciones de repositorio, esto porque solo tendremos UN REPOSITORIO por cada entidad que manejemos y para evitarnos el estar instanciando la misma clase una y otra vez, decidimos usar el Singleton Pattern. Para que asi nuestro proyecto sea aun MENOS DEPENDIENTE.

Antes cada instancia de la implementacion del repositorio tenia que crear consigo una instancia del datasource que usemos, pero esto rompia con nuestra logica del Repository Pattern, ya que el rato que queramos cambiar de datasource tendriamos que buscar todas las clases donde instanciamos el repositorio para cambiarle la fuente de datos, cosa que a la larga resultaria muy tedioso de mantener. Mientras que si usamos Singleton y todas las clases que necesiten de esta llamen a su instancia, al momento de querer camiar la fuente de datos, solo deberemos cambiar esta en la linea de codigo donde creamos la instancia y ya no en varios lugares del codigo.

![ReportImpl](./images/singleton.png)

Asi se ve ahora nuestras clases aplicando Singleton