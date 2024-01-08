using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TopMai.Handlers;

namespace TopMai.Controllers.Home
{
            [Route("")]
            [ApiController]
            [TypeFilter(typeof(CustomExceptionHandler))]
            public class HomeController : ControllerBase
            {
                        public HomeController()
                        {

                        }

                        // GET: api/<ValuesController>
                        [HttpGet("")]
                        [AllowAnonymous]
                        public ActionResult Get()
                        {

                                    return Ok("topmai");

                        }

                        [HttpGet("terms")]
                        [AllowAnonymous]
                        public ActionResult GetTerms()
                        {

                                    var html = @"Topmai es un app marketplace basado en la compra y venta de articulos nuevos y de segunda mano, facilitando el contacto C2C a través de las herramientas que conforman esta aplicacion. Para el buen funcionamiento de la comunidad, necesitamos que sigas los Términos y Condiciones de uso propuestos, los cuales estaran vigentes desde el momento del lanzamiento de Topmai.
Términos y Condiciones de uso de Topmai.
Los presentes Términos y Condiciones de uso, establecen las condiciones bajo las cuales se ofrece a los usuarios el acceso a la app Topmai, que es una plataforma que permite a los usuarios publicar ofertas para la compra–venta de una amplia variedad de artículos de su propiedad, así como la comunicación entre los usuarios ofertantes y los usuarios interesados en adquirir los artículos ofrecidos y la localización geográfica de los mismos, para completar la transacción.
El uso de la app implica la aceptación íntegra de estos Términos y Condiciones. En caso de no estar de acuerdo con todo o parte de estos Términos y Condiciones, el Usuario debe abstenerse de instalar y utilizar la app.
.Por medio de la aceptación de los presentes Términos y Condiciones, el Usuario manifiesta:
.Que ha leído, entiende y comprende lo aquí expuesto.
.Que asume todas las obligaciones aquí dispuestas.
.Que es mayor de edad y tiene la capacidad legal suficiente para utilizar el Servicio.
La aceptación de estos Términos y Condiciones por parte de los Usuarios es un paso previo e indispensable a la utilización de la app. Topmai se reserva el derecho a actualizar y/o modificar los Términos y Condiciones en cualquier momento y por cualquier motivo a su exclusiva discreción. Topmai notificará acerca de cualquier cambio material en los Términos y Condiciones. Al acceder a la app una vez haya sido notificado al usuario sobre una modificación o actualización, el Usuario acepta quedar obligado por los Términos y Condiciones modificados. Si los Términos y Condiciones modificados no resultan aceptables al Usuario, su única opción es dejar de utilizar la app.
Necesidad de registro
Para poder utilizar la app es necesario el previo registro del Usuario, la aceptación de los presentes Términos y Condiciones y la Política de Privacidad y de cookies.
Los datos introducidos por el Usuario deberán ser exactos, actuales y veraces. El Usuario registrado será responsable en todo momento de la custodia de su contraseña, asumiendo en consecuencia cualesquiera daños y perjuicios que pudieran derivarse de su uso indebido, así como de la cesión, revelación o extravío de la misma, debiendo informar inmediatamente a Topmai en caso de que tenga motivos para creer que su contraseña ha sido utilizada de manera no autorizada o es susceptible de serlo. En cualquier caso, se considera que el Usuario es responsable por el acceso y/o uso de la app en relación con su cuenta, quien responderá en todo caso de dicho acceso y/o uso.
Mediante la aceptación de los Términos y Condiciones, el Usuario consiente que sus datos pasen a formar parte del fichero de Topnai y el tratamiento de esos datos será conforme a lo previsto en la Política de Privacidad.
Normas de utilización del servicio
El Usuario se obliga a utilizar el Servicio conforme a lo establecido en la ley de México, la moral, el orden público y los presentes Términos y Condiciones. Asimismo, se obliga a hacer un uso adecuado del Servicio y a no emplearlo para realizar actividades ilícitas o constitutivas de delito, que atenten contra los derechos de terceros o que infrinjan cualquier norma del ordenamiento jurídico.
El Usuario se obliga a no transmitir, introducir, difundir y/o poner a disposición de terceros, cualquier tipo de material e información (productos, objetos, datos, contenidos, mensajes, dibujos, archivos de sonido e imagen, fotografías, software, etc.) que sean contrarios a la ley, la moral, el orden público y los presentes Términos y Condiciones. A título enunciativo y en ningún caso limitativo o excluyente, el Usuario se compromete a:
No introducir o difundir contenidos o propaganda de carácter racista, xenófobo, pornográfico, de apología del terrorismo, conflicto armado, incitación al odio o que atenten contra los derechos humanos.
No difundir, transmitir o poner a disposición de terceros cualquier tipo de información, elemento o contenido que atente contra los derechos fundamentales y las libertades públicas reconocidos constitucionalmente y en los tratados internacionales.
No difundir, transmitir o poner a disposición de terceros cualquier tipo de información, elemento o contenido que constituya publicidad ilícita o desleal.
No transmitir publicidad no solicitada o autorizada, material publicitario, “correo basura”, “cartas en cadena”, “estructuras piramidales”, o cualquier otra forma de solicitación, excepto en aquellas áreas (tales como espacios comerciales) que hayan sido exclusivamente concebidas para ello.
No introducir o difundir cualquier información y contenidos falsos, engañosos, ambiguos o inexactos de forma que induzca o pueda inducir a error a los receptores de la información.
No suplantar a otros Usuarios del Servicio ni transmitir los datos de acceso a la cuenta ni la contraseña a un tercero sin el consentimiento de Topmai.
No difundir, transmitir o poner a disposición de terceros cualquier tipo de información, elemento o contenido sin autorización de los titulares de los derechos de propiedad intelectual e industrial que puedan recaer sobre tal información, elemento o contenido.
No difundir, transmitir o poner a disposición de terceros cualquier tipo de información, elemento o contenido que suponga una violación del secreto de las comunicaciones y la legislación de datos de carácter personal.
No difundir, transmitir o poner a disposición de terceros fotografías ni cualquier representación o imagen de personas menores de edad.
No publicar anuncios de productos que requieran receta médica o que deban dispensarse bajo la supervisión de un médico (doctor, dentista, optometrista, óptico, farmacéutico o veterinario), así como los productos que puedan influir en la salud del individuo (que presenten contraindicaciones, interacciones, etc.) y perecederos de cualquier tipo.
Disponer de autorización sanitaria de funcionamiento para publicar anuncios relacionados con cualquier actividad sanitaria (necesidad de presentar el número de colegiación o número de registro del centro).
No crear valoraciones de transacciones no efectuadas.
No reportar falsamente o incorrectamente de forma reiterada.
El Usuario se obliga a mantener indemne a Topmai ante cualquier posible reclamación, multa, pena o sanción que pueda venir obligada a soportar como consecuencia del incumplimiento por parte del Usuario de cualquiera de las normas de utilización antes indicadas, reservándose además Topmai el derecho a solicitar la indemnización por daños y perjuicios que corresponda.
Exclusión de Usuarios
Topmai se reserva el derecho a impedir el uso del app, ya sea de forma temporal o definitiva, a cualquier Usuario que infrinja cualquiera de las normas establecidas en estos Términos y Condiciones, la ley o la moral. Discrecionalmente, Topmai también podrá excluir Usuarios e incluso dejar de prestar total o parcialmente el Servicio cuando así lo considere oportuno para mejorar la operativa del Servicio y/o del resto de los Usuarios del mismo.
Exclusión de responsabilidad
Topmai no es propietaria de ninguno de los artículos en venta o vendidos a través de su plataforma y no es parte en la transacción de compraventa llevada a cabo exclusivamente entre compradores y vendedores ni revisa o valida los productos que los Usuarios ofrecen a través de la app, por lo que Topmai no será responsable, ni directa ni indirectamente, ni subsidiariamente, de los daños y perjuicios de cualquier naturaleza derivados de la utilización y contratación de los contenidos y de las actividades de los Usuarios y/o de terceros a través del Servicio así como de la falta de licitud, fiabilidad, utilidad, veracidad, exactitud, exhaustividad y actualidad de los mismos.
Con carácter enunciativo, y en ningún caso limitativo, Topmai no será responsable por los daños y perjuicios de cualquier naturaleza derivados de:
La utilización que los Usuarios hagan del Servicio ni por el estado, origen, posible inexactitud, o posible falsedad de los datos proporcionados por los Usuarios ni de los productos ofrecidos a través del Servicio.
Los contenidos, informaciones, opiniones y manifestaciones de cualquier Usuario o de terceras personas o entidades que se comuniquen o exhiban a través del Servicio (incluye el envío de imágenes a través del chat).
La utilización que los Usuarios puedan hacer de los materiales del Servicio, ya sean prohibidos o permitidos, en infracción de los derechos de propiedad intelectual y/o industrial, información confidencial, de contenidos del Servicio o de terceros.
La realización de actos de competencia desleal y publicidad ilícita.
La eventual pérdida de datos de los Usuarios por causa no atribuible al Servicio.
El acceso de menores de edad a los contenidos incluidos en el Servicio.
La indisponibilidad, errores, fallos de acceso y falta de continuidad del Servicio.
Los fallos o incidencias que pudieran producirse en las comunicaciones, borrado o transmisiones incompletas.
La no operatividad o problemas en la dirección de email facilitada por el Usuario.
Topmai responderá única y exclusivamente frente al Usuario de los Servicios que preste por sí misma y de los contenidos directamente originados e identificados con su copyright, quedando limitada, como máximo, al importe de las cantidades percibidas directamente del Usuario por Topmai, con exclusión en todo caso de responsabilidad por daños indirectos o por lucro cesante.
Cualquier reclamación o controversia que pueda surgir entre los Usuarios del Servicio deberá ser solventada entre éstos, obligándose a mantener a Topmai totalmente indemne, sin perjuicio de lo cual Topmai realizará sus mejores esfuerzos para facilitar a los Usuarios una pronta y satisfactoria solución a través de su Servicio de Atención al Usuario.
Contenidos y servicios enlazados a través del Servicio
El Servicio puede incluir dispositivos técnicos de enlace, directorios e incluso instrumentos de búsqueda que permitan al Usuario acceder a otras páginas y portales de internet (en adelante, “Sitios Enlazados”).
El Usuario reconoce y acepta que el acceso a los Sitios Enlazados será bajo su exclusivo riesgo y responsabilidad y exonera a Topmai de cualquier responsabilidad sobre eventuales vulneraciones de derechos de propiedad intelectual o industrial de los titulares de los Sitios Enlazados. Asimismo, el Usuario exonera a Topmai de cualquier responsabilidad sobre la disponibilidad técnica de las páginas web enlazadas, la calidad, fiabilidad, exactitud y/o veracidad de los servicios, informaciones, elementos y/o contenidos a los que el Usuario pueda acceder.
En estos casos, Topmai solo será responsable de los contenidos y servicios suministrados en los Sitios Enlazados en la medida en que tenga conocimiento efectivo de la ilicitud y no haya desactivado el enlace con la diligencia debida. En el supuesto de que el Usuario considere que existe un Sitio Enlazado con contenidos ilícitos o inadecuados podrá comunicárselo a Topmai, sin que en ningún caso esta comunicación conlleve la obligación de retirar el correspondiente enlace.
En ningún caso, la existencia de Sitios Enlazados debe presuponer la formalización de acuerdos con los responsables o titulares de los mismos, ni la recomendación, promoción o identificación de Topmai con las manifestaciones, contenidos o servicios proveídos. Topmai no conoce los contenidos y servicios de los Sitios Enlazados y, por tanto, no se hace responsable de forma directa o indirecta por los daños producidos por la ilicitud, calidad, desactualización, indisponibilidad, error e inutilidad de los contenidos y/o servicios de los Sitios Enlazados ni por cualquier otro daño que no sea directamente imputable a Topmai.
Propiedad intelectual e industrial
Los derechos de propiedad industrial e intelectual sobre las obras, prestaciones protegidas y cualesquiera contenidos o elementos sobre los que recaigan derechos de propiedad intelectual e industrial que se usen en el Servicio (los “Contenidos del Servicio”) pertenecen a sus legítimos titulares. El Usuario no adquirirá por el uso del Servicio ningún derecho de propiedad intelectual o industrial, ni licencia de uso alguna sobre tales elementos.
Son Contenidos del Servicio, los textos, fotografías, gráficos, imágenes, iconos, tecnología, software, bases de datos, y demás contenidos audiovisuales o sonoros, así como su diseño gráfico y códigos fuente utilizados en el Servicio. Esta enumeración se realiza a título enunciativo y ejemplificativo, no limitativo.
El texto, las imágenes, los gráficos, los ficheros de sonido, los ficheros de animación, los ficheros de vídeo, el software y la apariencia del sitio web de Topmai son objeto de protección por derechos de propiedad intelectual e industrial. Esos elementos no podrán ser válida y legítimamente copiados o distribuidos para uso comercial, ni podrán ser modificados o insertados en otros sitios web sin previa autorización expresa de sus titulares.
El Usuario manifiesta ser titular en exclusiva de todos los derechos que recaen sobre las obras, prestaciones protegidas y cualesquiera otros elementos protegidos por propiedad intelectual o industrial que incorpore en el Servicio (en adelante, los “Contenidos”).
El Usuario cede gratuitamente y en exclusiva a Topmai los derechos de comunicación pública, reproducción, distribución y transformación que tiene sobre los Contenidos, en todas las modalidades de explotación existentes en la fecha de aceptación de estos Términos y Condiciones, por todo el tiempo de vigencia de los derechos objeto de cesión, para el ámbito territorial universal.
Topmai no representa ni tiene vinculación empresarial alguna con las marcas que los Usuarios puedan anunciar en el Servicio. 
Publicación de anuncios
A la hora de publicar sus anuncios el Usuario deberá tener en cuenta algunas consideraciones, incluyendo las siguientes:
No está permitido publicar anuncios con imágenes de contenido sexual.
No está permitido publicar anuncios relacionados con el sexo, el erotismo o el fetichismo, así como tampoco relacionados con masajes, servicios de compañía o amistad.
No está permitido anunciar esquemas piramidales o similares.
Solo se permite anunciar un inmueble por anuncio. En tal caso, el inmueble debe anunciarse en la localidad en la que se encuentre.
No está permitido publicar imágenes o descripciones que no se correspondan con el artículo realmente ofrecido.
No está permitido publicar imágenes de menores de edad.
No está permitido publicar anuncios relacionados con animales.
Para algunas categorías existe un límite máximo de productos que se pueden publicar de manera gratuita.
En cualquier caso, a la hora de publicar sus anuncios el Usuario deberá tomar en consideración las reglas de publicación. 
Servicios de visibilidad
Los Usuarios podrán incrementar la visibilidad de sus anuncios en Topmai a través de la contratación de servicios de preferencias publicitarias («servicios de visibilidad»). Estos son servicios opcionales de los que pueden disfrutar los Usuarios para mejorar el posicionamiento de los artículos anunciados.
Los anuncios destacados son una prestación que ofrece Topmai para que los anuncios tengan más visibilidad en el muro. Existen diferentes tipos de destacados que se pueden consultar aquí.
Servicio de envío de Topmai
Topmai cuenta con un servicio de envío integrado. Este servicio de envío permite realizar envíos de los productos de los Usuarios a través de Topmai de forma segura. Los envíos se realizan con distintos proveedores de transporte dependiendo del tipo de envío y/o servicio. 
Para poder hacer uso de este servicio será imprescindible que toda la transacción se haya efectuado a través de Topmai y que el producto adquirido se ajuste a nuestras reglas de convivencia.
Seguridad
Todos los pagos realizados utilizando el sistema de pagos de Topmai son seguros y están protegidos de tal modo que el dinero no se transferirá al vendedor hasta que hayan transcurrido 48 horas a contar desde la recepción del paquete por parte del comprador y siempre y cuando éste no haya manifestado su disconformidad con el producto recibido. Para poder entrar en el proceso de protección, debes abrir una disputa o contactar con Topmai en un plazo no superior a 48h desde la entrega del paquete. Cualquier reclamación que se haga 48 horas después contando desde la recepción de la notificación de entrega, o en su caso, desde la fecha de recepción que consta en la información de entrega proporcionada por el proveedor logístico, será rechazada. 
Para realizar las entregas de los productos, sus datos personales serán facilitados a terceros operadores de servicio postal que se encargarán de dicha gestión y a las partes que realicen la transacción o contrato correspondiente, de conformidad con lo establecido en la normativa de protección de datos de carácter personal. 
Tus datos podrán ser compartidos y tratados para la prevención del fraude o para la gestión de disputas.
Mediaciones
Topmai es una aplicación que actúa como mero intermediario entre el vendedor y el comprador. Topmai no es propietaria de ninguno de los artículos en venta o vendidos a través de su plataforma y no es parte en la compraventa llevada a cabo exclusivamente entre compradores y vendedores. Topmai no revisa ni valida los productos que los Usuarios ofrecen a través del servicio, por lo que Topmai no será en ningún caso responsable, ni directa ni indirectamente, ni subsidiariamente, de los daños y perjuicios de cualquier naturaleza derivados de la utilización y contratación de los contenidos y de las actividades de los Usuarios y/o de terceros a través de la app, así como tampoco será responsable de la falta de licitud, fiabilidad, utilidad, veracidad, exactitud, exhaustividad y actualidad de los mismos.
Nulidad e ineficacia de las cláusulas
Si cualquier cláusula incluida en los presentes Términos y Condiciones fuese declarada total o parcialmente, nula o ineficaz, tal nulidad o ineficacia tan solo afectará a dicha disposición o a la parte de la misma que resulte nula o ineficaz, subsistiendo los presentes Términos y Condiciones en todo lo demás, considerándose tal disposición total o parcialmente por no incluida.
";
                                    return Ok(html);

                        }

                        [HttpGet("politics")]
                        [AllowAnonymous]

                        public ActionResult GetPolitics()
                        {

                                    var html = @" POLÍTICA DE PRIVACIDAD

El presente Política de Privacidad establece los términos en que topmai usa y protege la información que es proporcionada por sus usuarios al momento de utilizar su sitio web. Esta compañía está comprometida con la seguridad de los datos de sus usuarios. Cuando le pedimos llenar los campos de información personal con la cual usted pueda ser identificado, lo hacemos asegurando que sólo se empleará de acuerdo con los términos de este documento. Sin embargo esta Política de Privacidad puede cambiar con el tiempo o ser actualizada por lo que le recomendamos y enfatizamos revisar continuamente esta página para asegurarse que está de acuerdo con dichos cambios.

Información que es recogida

Nuestro sitio web podrá recoger información personal por ejemplo: Nombre,  información de contacto como  su dirección de correo electrónica e información demográfica. Así mismo cuando sea necesario podrá ser requerida información específica para procesar algún pedido o realizar una entrega o facturación.

Uso de la información recogida

Nuestro sitio web emplea la información con el fin de proporcionar el mejor servicio posible, particularmente para mantener un registro de usuarios, de pedidos en caso que aplique, y mejorar nuestros productos y servicios.  Es posible que sean enviados correos electrónicos periódicamente a través de nuestro sitio con ofertas especiales, nuevos productos y otra información publicitaria que consideremos relevante para usted o que pueda brindarle algún beneficio, estos correos electrónicos serán enviados a la dirección que usted proporcione y podrán ser cancelados en cualquier momento.

topmai está altamente comprometido para cumplir con el compromiso de mantener su información segura. Usamos los sistemas más avanzados y los actualizamos constantemente para asegurarnos que no exista ningún acceso no autorizado.

Cookies

Una cookie se refiere a un fichero que es enviado con la finalidad de solicitar permiso para almacenarse en su ordenador, al aceptar dicho fichero se crea y la cookie sirve entonces para tener información respecto al tráfico web, y también facilita las futuras visitas a una web recurrente. Otra función que tienen las cookies es que con ellas las web pueden reconocerte individualmente y por tanto brindarte el mejor servicio personalizado de su web.

Nuestro sitio web emplea las cookies para poder identificar las páginas que son visitadas y su frecuencia. Esta información es empleada únicamente para análisis estadístico y después la información se elimina de forma permanente. Usted puede eliminar las cookies en cualquier momento desde su ordenador. Sin embargo las cookies ayudan a proporcionar un mejor servicio de los sitios web, estás no dan acceso a información de su ordenador ni de usted, a menos de que usted así lo quiera y la proporcione directamente . Usted puede aceptar o negar el uso de cookies, sin embargo la mayoría de navegadores aceptan cookies automáticamente pues sirve para tener un mejor servicio web. También usted puede cambiar la configuración de su ordenador para declinar las cookies. Si se declinan es posible que no pueda utilizar algunos de nuestros servicios.

Enlaces a Terceros

Este sitio web pudiera contener en laces a otros sitios que pudieran ser de su interés. Una vez que usted de clic en estos enlaces y abandone nuestra página, ya no tenemos control sobre al sitio al que es redirigido y por lo tanto no somos responsables de los términos o privacidad ni de la protección de sus datos en esos otros sitios terceros. Dichos sitios están sujetos a sus propias políticas de privacidad por lo cual es recomendable que los consulte para confirmar que usted está de acuerdo con estas.

Control de su información personal

En cualquier momento usted puede restringir la recopilación o el uso de la información personal que es proporcionada a nuestro sitio web.  Cada vez que se le solicite rellenar un formulario, como el de alta de usuario, puede marcar o desmarcar la opción de recibir información por correo electrónico.  En caso de que haya marcado la opción de recibir nuestro boletín o publicidad usted puede cancelarla en cualquier momento.

Esta compañía no venderá, cederá ni distribuirá la información personal que es recopilada sin su consentimiento, salvo que sea requerido por un juez con un orden judicial.

topmai Se reserva el derecho de cambiar los términos de la presente Política de Privacidad en cualquier momento. ";
                                    return Ok(html);
                        }
            }
}
