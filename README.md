# BI-Portal
Plataforma de gestión de información BI.

## Resumen del proyecto
BI-Portal es una propuesta en desarrollo para una plataforma de gestión de información orientada a la inteligencia de negocios. Actualmente, se ha construido el módulo de autenticación y seguridad, con un enfoque escalable y adaptable para futuros módulos.

## Módulo de Autenticación y Seguridad
Implementa un sistema de autenticación basado en Claims y Cookies, que permite gestionar usuarios autenticados con control de tiempo de sesión.
Integra un CRUD completo de usuarios, accesible para administradores, con funcionalidades adicionales como buscador, paginación y accesos rápidos para una gestión eficiente.
Las contraseñas son almacenadas de forma segura mediante hashing con BCrypt, garantizando un nivel adecuado de protección.
La plataforma soporta un sistema de roles y permisos, lo que asegura que cada usuario pueda visualizar y acceder únicamente a los módulos correspondientes a su perfil.
El diseño actual permite la expansión futura del sistema de seguridad para mejorar sus capacidades y fortalecer aún más la protección de los datos.


