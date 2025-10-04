# 🎓 Bolsa de Trabajo FEUCN – Modelo de Base de Datos
Este repositorio contiene el modelo entidad-relación (MER) y el modelo físico de la base de datos del proyecto Bolsa de Trabajo FEUCN, desarrollado como parte de la asignatura Proyecto Integrador de Software.
El diseño fue realizado en Visual Paradigm y utiliza PostgreSQL como sistema de gestión de bases de datos.

## 📘 Descripción del Proyecto
La Bolsa de Trabajo FEUCN es una iniciativa impulsada por la Federación de Estudiantes de la Universidad Católica del Norte (FEUCN), con el objetivo de apoyar a los estudiantes del campus Antofagasta en la búsqueda de empleos temporales que les permitan solventar sus gastos cotidianos durante su vida universitaria.
Estos trabajos buscan ayudar a cubrir necesidades como:
- Alimentación 🍽️
- Arriendo 🏠
- Aranceles universitarios 💸
- Transporte público 🚍
El sistema conecta a los estudiantes con oferentes (personas o instituciones que publican empleos temporales), facilitando la gestión de ofertas, postulaciones y evaluaciones.

## 🧠 Tecnologías Utilizadas
-Visual Paradigm → Diseño del modelo conceptual y lógico
-PostgreSQL → Implementación del modelo físico

## ⚙️ Cómo Abrir el Proyecto
1. Instalar Visual Paradigm (versión 17.0 o superior).
2. Clonar el repositorio:
   ```bash
   git clone https://github.com/usuario/bolsa-trabajo-feucn-db.git
   ```
3. Abrir el archivo "PIS.vpp" en Visual Paradigm
4. Para generar el esquema SQL:
   ```nginx
   Tools → DB → Generate Database
   ```
   y seleccionar PostgreSQL como DBMS.

## 🧩 Estructura del Modelo
El modelo considera los siguientes elementos principales:
-Usuario (atributos comunes: correo, teléfono, contraseña, rol, estado de sesión y bloqueo)
-Estudiante, Oferente (persona o empresa) y Administrador
-OfertaLaboral → publicación de trabajos temporales
-Postulación → aplicación de estudiantes a ofertas
-Evaluación → retroalimentación entre oferente y postulante
El diseño busca mantener la integridad y trazabilidad de las relaciones entre los actores del sistema, priorizando la simplicidad y la escalabilidad.

## 👥 Autores
Proyecto desarrollado por estudiantes de la carrera de Ingeniería Civil en Computación e Informática
como parte de la asignatura Proyecto Integrador de Software.

### Colaboración con:
🎓 Federación de Estudiantes de la Universidad Católica del Norte (FEUCN)
